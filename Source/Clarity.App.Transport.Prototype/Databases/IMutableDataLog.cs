using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IMutableDataLog : IDataLog
    {
        void Reset(IReadOnlyList<IDataTable> tables, int snapshotInterval = 1000);
        void ClearFrom(double timestamp);
        void AddEntry(IDataLogEntry entry);
    }
}