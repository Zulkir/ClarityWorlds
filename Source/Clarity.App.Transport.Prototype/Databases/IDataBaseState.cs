using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataBaseState
    {
        IReadOnlyList<IDataTable> Tables { get; }
        IReadOnlyList<IDataTableState> TableStates { get; }
        IDataTableState GetTableState(IDataTable table);
        IDataTableState GetTableStateById(int tableId);
        IMutableDataBaseState CloneAsMutable();
    }
}