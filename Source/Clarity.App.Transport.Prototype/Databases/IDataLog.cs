using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataLog
    {
        IReadOnlyList<IDataTable> Tables { get; }
        double StartTimestamp { get; }
        double EndTimestamp { get; }
        IDataBaseState GetStateAt(double timestamp);
        IEnumerable<IDataLogEntry> GetEntries(double startTimestamp, double endTimestamp);
        IEnumerable<DataLogReadingState> Read(double startTimestamp, double endTimestamp);
        void PrepareStateCache(string key, double timestamp);
    }
}