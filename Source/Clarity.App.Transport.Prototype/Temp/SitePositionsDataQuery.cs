using System;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Databases.LogEntries;
using Clarity.App.Transport.Prototype.Queries.Data;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Transport.Prototype.Temp
{
    public class SitePositionsDataQuery : IDataQuery
    {
        private readonly IAppRuntime runtime;
        private readonly IMutableDataLog resultLog;
        private readonly Filter filter;

        public string Text => "Sites.UniformCircle() * 10 as (X, Y)";
        public double Param0 { get; set; }
        public IDataTable ResultTable { get; }
        private readonly IDataField resultXField;
        private readonly IDataField resultYField;

        private IDataTable sitesTable;
        private IDataField sitesTableIdField;
        private string invalidErrorMessage;

        public IDataLog ResultDataLog => resultLog;
        public event Action<IDataLogUpdatedEvent> DataLogUpdated;

        public SitePositionsDataQuery(IAppRuntime runtime, int tableId)
        {
            this.runtime = runtime;
            ResultTable = new DataTable(tableId, "SitePositions", new []
            {
                new DataFieldDescription(DataFieldType.Int32, "SiteId"),
                new DataFieldDescription(DataFieldType.Float, "X"), 
                new DataFieldDescription(DataFieldType.Float, "Y"), 
            });
            resultXField = ResultTable.Fields[1];
            resultYField = ResultTable.Fields[2];
            resultLog = new MutableDataLog();
            resultLog.Reset(new []{ ResultTable });
            filter = new Filter(this);
        }

        public void OnAttached()
        {
            RebuildResultLog(double.MinValue);
        }

        public void OnTableLayoutChanged()
        {
            var oldSitesTable = sitesTable;
            var oldSitesTableIdField = sitesTableIdField;
            if (!runtime.DataRetrieval.TryGetTable("Sites", out sitesTable))
            {
                invalidErrorMessage = "Table 'Sites' not found.";
                return;
            }
            if (!sitesTable.TryGetField("Id", out sitesTableIdField))
            {
                invalidErrorMessage = "Field 'Id' not found in table 'Sites'.";
                return;
            }
            invalidErrorMessage = null;
            if (sitesTable != oldSitesTable || sitesTableIdField != oldSitesTableIdField)
                RebuildResultLog(double.MinValue);
        }

        public bool CheckIsValid(out string errorMessage)
        {
            errorMessage = invalidErrorMessage;
            return invalidErrorMessage == null;
        }

        public void OnTimestampChanged(double timestamp)
        {
        }

        private class Filter : IDataLogFilter
        {
            private readonly SitePositionsDataQuery self;
            public Filter(SitePositionsDataQuery self) { this.self = self; }
            public bool AcceptsTable(IDataTable table) => table == self.sitesTable;
            public bool AcceptsField(IDataField field) => field == self.sitesTableIdField;
            public bool AcceptsEntry(IDataLogEntry entry) => entry.Table == self.sitesTable && (entry.Field == null || entry.Field == self.sitesTableIdField);
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
            foreach (var readingState in runtime.DataRetrieval.ReadFiltered(filter, startTimestamp, double.MaxValue))
                switch (readingState.Entry)
                {
                    case InsertRowLogEntry insertRowLogEntry:
                        var timestamp = insertRowLogEntry.Timestamp;
                        resultLog.AddEntry(new InsertRowLogEntry(timestamp, ResultTable, insertRowLogEntry.RowKey));
                        var sitesState = readingState.State.GetTableStateById(sitesTable.Id);
                        for (var i = 0; i < sitesState.RowCount; i++)
                        {
                            var siteId = sitesState.GetValueByIndex<int>(i, sitesTableIdField.Index);
                            var angle = i * MathHelper.TwoPi / sitesState.RowCount;
                            var pos = 10 * new Vector2(MathHelper.Cos(angle), MathHelper.Sin(angle));
                            resultLog.AddEntry(new SetValueLogEntry<float>(timestamp, ResultTable, siteId, resultXField,
                                pos.X));
                            resultLog.AddEntry(new SetValueLogEntry<float>(timestamp, ResultTable, siteId, resultYField,
                                pos.Y));
                        }
                        changed = true;
                        break;
                    case DeleteRowLogEntry deleteRowLogEntry:
                        resultLog.AddEntry(new DeleteRowLogEntry(deleteRowLogEntry.Timestamp, sitesTable,
                            deleteRowLogEntry.RowKey));
                        changed = true;
                        break;
                }
            if (changed)
                // todo: optimize allocation
                // todo: use exact timestamp for resultLog
                DataLogUpdated?.Invoke(new DataLogUpdatedEvent(startTimestamp, new[] { ResultTable }));
        }
    }
}