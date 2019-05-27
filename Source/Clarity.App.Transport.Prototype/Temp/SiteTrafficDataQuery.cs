using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Databases.LogEntries;
using Clarity.App.Transport.Prototype.Queries.Data;
using Clarity.App.Transport.Prototype.Queries.Infra;
using Clarity.App.Transport.Prototype.Queries.Infra.Concrete;
using Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Functions;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Transport.Prototype.Temp
{
    public class SiteTrafficDataQuery : IDataQuery
    {
        private readonly IAppRuntime appRuntime;
        private readonly MutableDataLog resultLog;
        private readonly Filter filter;
        private readonly IDataField resultTableTrafficField;
        
        public IDataTable ResultTable { get; }

        private readonly Dictionary<(int from, int to), IInfraQuery<DPolyCubic>> valueQueries;
        private readonly Dictionary<(int from, int to), List<(double timestamp, DPolyCubic value)>> separatedValues;

        private double param0 = 100_000;

        private IDataTable messagesTable;
        private IDataField messagesTableSequenceField;
        private IDataField messagesTableFromField;
        private IDataField messagesTableToField;
        private IDataField messagesTableSizeField;
        private IDataField messagesTableTimeField;
        private string invalidErrorMessage;

        public IDataLog ResultDataLog => resultLog;
        public string Text => $"Messages.GroupBy(From, To).Count().AvgOver({Param0}) as Traffic";

        public double Param0
        {
            get => param0;
            set
            {
                param0 = value;
                OnTableLayoutChanged();
            }
        }

        public event Action<IDataLogUpdatedEvent> DataLogUpdated;

        public SiteTrafficDataQuery(IAppRuntime appRuntime, ITableService tableService)
        {
            this.appRuntime = appRuntime;
            filter= new Filter(this);
            ResultTable = tableService.CreateTable("SiteTraffic", new[]
            {
                new DataFieldDescription(DataFieldType.Int32, "Id"),
                new DataFieldDescription(DataFieldType.Pc64, "Traffic"),
            });
            resultTableTrafficField = ResultTable.Fields.Single(x => x.Name == "Traffic");
            resultLog = new MutableDataLog();
            resultLog.Reset(new []{ResultTable});
            valueQueries = new Dictionary<(int from, int to), IInfraQuery<DPolyCubic>>();
            separatedValues = new Dictionary<(int from, int to), List<(double timestamp, DPolyCubic value)>>();
        }

        public bool CheckIsValid(out string errorMessage)
        {
            errorMessage = invalidErrorMessage;
            return invalidErrorMessage == null;
        }

        public void OnTimestampChanged(double timestamp)
        {
            // todo: set named snapshot to the log
        }

        public void OnAttached()
        {
            RebuildResultLog(double.MinValue);
        }

        public void OnTableLayoutChanged()
        {
            invalidErrorMessage = "error";
            if (!appRuntime.DataRetrieval.TryGetTable("Messages", out messagesTable))
                return;
            if (!messagesTable.TryGetField("Sequence", out messagesTableSequenceField))
                return;
            if (!messagesTable.TryGetField("From", out messagesTableFromField))
                return;
            if (!messagesTable.TryGetField("To", out messagesTableToField))
                return;
            if (!messagesTable.TryGetField("Size", out messagesTableSizeField))
                return;
            if (!messagesTable.TryGetField("Time", out messagesTableTimeField))
                return;

            valueQueries.Clear();
            foreach (var pair in new[] { (1, 2), (1, 3), (2, 1), (2, 3), (3, 1), (3, 2) })
            {
                var countQuery = new CountQuery(
                    new WhereQuery(
                        new EnumerateRowsQuery(messagesTable),
                        e => e.State.Keys.Where(k =>
                        {
                            var from = e.State.GetValue<int>(k, messagesTableFromField.Index);
                            var to = e.State.GetValue<int>(k, messagesTableToField.Index);
                            return ToRowKey((@from, to)) == ToRowKey(pair);
                        })));
                var query =
                    new MultQuery(
                        new AvgOverQueryP64(
                            countQuery,
                            //new SumQueryP64(
                            //    new FloatToP64Query(
                            //        new FuncQuery<float>(
                            //            new EnumerateRowsQuery(messagesTable),
                            //            (s, k) => s.GetValue<int>(k, messagesTableSizeField.Index) / s.GetValue<float>(k, messagesTableTimeField.Index)))),
                            param0),
                        1
                        );
                    ;
                valueQueries.Add(pair, query);
            }

            // todo: choose actual start timestamp
            RebuildResultLog(0);

            invalidErrorMessage = "";
        }

        private class Filter : IDataLogFilter
        {
            private readonly SiteTrafficDataQuery self;
            public Filter(SiteTrafficDataQuery self) { this.self = self; }
            public bool AcceptsTable(IDataTable table) => table == self.messagesTable;
            public bool AcceptsField(IDataField field) => true;
            public bool AcceptsEntry(IDataLogEntry entry) => entry.Table == self.messagesTable;
        }

        public void OnDataLogUpdated(IDataLogUpdatedEvent evnt)
        {
            if (evnt.TablesAffected.Any(x => filter.AcceptsTable(x)))
                RebuildResultLog(evnt.StartTimestamp);
        }

        private void RebuildResultLog(double startTimestamp)
        {
            resultLog.ClearFrom(startTimestamp);
            var changed = false;

            foreach (var kvp in valueQueries)
            {
                var list = separatedValues.GetOrAdd(kvp.Key, x => new List<(double timestamp, DPolyCubic value)>());
                list.Clear();
                var context = new InfraQueryExecutionContext(appRuntime.DataRetrieval);
                // todo: better upper border
                foreach (var timedValue in kvp.Value.ExecuteOverTime(context, startTimestamp, 1_000_000_000))
                    list.Add(timedValue);

            }

            var stateBefore = resultLog.GetStateAt(startTimestamp).GetTableState(ResultTable);

            foreach (var key in valueQueries.Keys)
            {
                var rowKey = ToRowKey(key);
                if (!stateBefore.HasRow(rowKey))
                {
                    // todo: remove max()
                    resultLog.AddEntry(new InsertRowLogEntry(Math.Max(startTimestamp, 0), ResultTable, rowKey));
                    changed = true;
                }
            }

            var asEntryEnumerations = separatedValues.Select(x => x.Value.Select(y =>
                new SetValueLogEntry<DPolyCubic>(y.timestamp, ResultTable, ToRowKey(x.Key), resultTableTrafficField,
                    y.value)));

            foreach (var dataLogEntry in DataLogEntryEnumerationMerger.Merge(asEntryEnumerations, AcceptAllDataLogFilter.Singleton))
            {
                resultLog.AddEntry(dataLogEntry);
                changed = true;
            }
            
            separatedValues.Clear();
            
            if (changed)
                // todo: optimize allocation
                // todo: use exact timestamp for resultLog
                DataLogUpdated?.Invoke(new DataLogUpdatedEvent(startTimestamp, new[] { ResultTable }));
        }

        public static int ToRowKey((int from, int to) pair) => pair.from * 10000 + pair.to;
        public static (int from, int to) ToFromToPair(int rowKey) => (rowKey / 10000, rowKey % 10000);
    }
}