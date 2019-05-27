using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IDataRetrievalService
    {
        double MinOverallTimestamp { get; }
        double MaxOverallTimestamp { get; }

        IEnumerable<IDataTable> AllTables();
        IDataLog GetLogForTable(IDataTable table);

        bool TryGetTable(string tableName, out IDataTable table);
        IEnumerable<IDataLogEntry> GetFilteredEntries(IDataLogFilter filter, double startTimestamp, double endTimestamp);
        IEnumerable<DataLogReadingState> ReadFiltered(IDataLogFilter filter, double startTimestamp, double endTimestamp);
    }
}