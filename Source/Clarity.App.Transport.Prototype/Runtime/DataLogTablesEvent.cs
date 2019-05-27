using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class DataLogTablesEvent : RoutedEventBase, IDataLogTablesEvent
    {
        public IReadOnlyList<IDataTable> AddedTables { get; private set; }
        public IReadOnlyList<IDataTable> ResetTables { get; private set; }
        public IReadOnlyList<IDataTable> DroppedTables { get; private set; }

        public void Reset(IReadOnlyList<IDataTable> addedTables, IReadOnlyList<IDataTable> resetTables, IReadOnlyList<IDataTable> droppedTables)
        {
            AddedTables = addedTables;
            ResetTables = resetTables;
            DroppedTables = droppedTables;
            StopPropagation = false;
        }
    }
}