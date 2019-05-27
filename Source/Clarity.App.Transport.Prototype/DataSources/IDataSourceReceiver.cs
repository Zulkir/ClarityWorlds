using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.DataSources
{
    public interface IDataSourceReceiver
    {
        void Reset(IReadOnlyList<IDataTable> tables);
        void NewEntry(IDataLogEntry entry);
        void BeginPack();
        void EndPack();
    }
}