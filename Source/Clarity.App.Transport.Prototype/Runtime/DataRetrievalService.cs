using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class DataRetrievalService : IDataRetrievalService
    {
        private readonly IOriginDataLogService originDataLogService;
        private readonly IDataQueriesService dataQueriesService;

        public double MinOverallTimestamp => 
            dataQueriesService.Queries.Select(x => x.ResultDataLog.StartTimestamp).ConcatSingle(originDataLogService.DataLog.StartTimestamp).Min();
        public double MaxOverallTimestamp =>
            dataQueriesService.Queries.Select(x => x.ResultDataLog.EndTimestamp).ConcatSingle(originDataLogService.DataLog.EndTimestamp).Max();

        public DataRetrievalService(IOriginDataLogService originDataLogService, IDataQueriesService dataQueriesService)
        {
            this.originDataLogService = originDataLogService;
            this.dataQueriesService = dataQueriesService;
        }

        public IEnumerable<IDataTable> AllTables()
        {
            return originDataLogService.DataLog.Tables.Concat(dataQueriesService.Queries.Select(x => x.ResultTable));
        }

        public IDataLog GetLogForTable(IDataTable table)
        {
            return originDataLogService.DataLog.Tables.Contains(table) 
                ? originDataLogService.DataLog 
                : dataQueriesService.Queries.First(x => x.ResultTable == table).ResultDataLog;
        }

        public bool TryGetTable(string tableName, out IDataTable table)
        {
            foreach (var existingTable in originDataLogService.DataLog.Tables.Concat(dataQueriesService.Queries.Select(x => x.ResultTable)))
                if (existingTable.Name == tableName)
                {
                    table = existingTable;
                    return true;
                }
            table = null;
            return false;
        }

        public IEnumerable<DataLogReadingState> ReadFiltered(IDataLogFilter filter, double startTimestamp, double endTimestamp)
        {
            var sourceLogs = GetSourceLogs(filter);
            var states = sourceLogs
                .Select(x => x.GetStateAt(startTimestamp))
                .SelectMany(x => x.TableStates)
                .Where(x => filter.AcceptsTable(x.Table))
                .Select(x => x as IMutableDataTableState ?? new MutableDataTableState(x.Table, x))
                .ToArray();
            var tables = states.Select(x => x.Table).ToArray();
            var combinedState = new MutableDataBaseState(tables, states);
            var entries = DataLogEntryEnumerationMerger.Merge(sourceLogs.Select(x => x.GetEntries(startTimestamp, endTimestamp)), filter);
            return new DataLogEnumerable(combinedState, entries);
        }

        public IEnumerable<IDataLogEntry> GetFilteredEntries(IDataLogFilter filter, double startTimestamp, double endTimestamp)
        {
            var sourceLogs = GetSourceLogs(filter);
            return DataLogEntryEnumerationMerger.Merge(sourceLogs.Select(x => x.GetEntries(startTimestamp, endTimestamp)), filter);
        }

        private List<IDataLog> GetSourceLogs(IDataLogFilter filter)
        {
            var sourceLogs = new List<IDataLog>();
            if (originDataLogService.DataLog.Tables.Any(filter.AcceptsTable))
                sourceLogs.Add(originDataLogService.DataLog);
            sourceLogs.AddRange(dataQueriesService.Queries.Where(x => filter.AcceptsTable(x.ResultTable)).Select(x => x.ResultDataLog));
            return sourceLogs;
        }
    }
}