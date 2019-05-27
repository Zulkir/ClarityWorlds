using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class MutableDataBaseState : IMutableDataBaseState
    {
        public IReadOnlyList<IDataTable> Tables { get; }
        private readonly IMutableDataTableState[] tableStates;
        private readonly IMutableDataTableState[] tableStatesById;

        IReadOnlyList<IDataTableState> IDataBaseState.TableStates => TableStates;
        public IReadOnlyList<IMutableDataTableState> TableStates => tableStates;

        public MutableDataBaseState(IReadOnlyList<IDataTable> tables, IDataBaseState initialState = null)
            : this(tables, tables.Select(x => new MutableDataTableState(x, initialState?.GetTableState(x).CloneAsMutable())).Cast<IMutableDataTableState>().ToArray())
        {
        }

        public MutableDataBaseState(IReadOnlyList<IDataTable> tables, IMutableDataTableState[] preparedMutableStates)
        {
            Tables = tables;
            tableStates = preparedMutableStates;
            var maxId = tableStates.HasItems() ? tableStates.Max(x => x.Table.Id) : -1;
            tableStatesById = new IMutableDataTableState[maxId + 1];
            foreach (var state in tableStates)
                tableStatesById[state.Table.Id] = state;
        }

        IDataTableState IDataBaseState.GetTableState(IDataTable table) => GetTableState(table);
        public IMutableDataTableState GetTableState(IDataTable table) =>
            tableStatesById[table.Id] ?? throw new ArgumentOutOfRangeException(nameof(table));

        IDataTableState IDataBaseState.GetTableStateById(int tableId) => GetTableStateById(tableId);
        public IMutableDataTableState GetTableStateById(int tableId) =>
            tableStatesById[tableId] ?? throw new ArgumentOutOfRangeException(nameof(tableId));

        public IMutableDataBaseState CloneAsMutable() => new MutableDataBaseState(Tables, this);
    }
}