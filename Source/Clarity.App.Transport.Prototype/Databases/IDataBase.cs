using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataBase
    {
        IReadOnlyList<IDataTable> Tables { get; }
        bool HasTableWithName(string name);
        IDataTable GetTableByName(string name);
    }
}