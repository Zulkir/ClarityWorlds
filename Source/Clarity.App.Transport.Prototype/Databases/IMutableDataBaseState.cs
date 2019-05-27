using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IMutableDataBaseState : IDataBaseState
    {
        new IReadOnlyList<IMutableDataTableState> TableStates { get; }
        new IMutableDataTableState GetTableState(IDataTable table);
        new IMutableDataTableState GetTableStateById(int tableId);
    }
}