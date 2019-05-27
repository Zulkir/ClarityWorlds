using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IDataLogTablesEvent : IRoutedEvent
    {
        IReadOnlyList<IDataTable> AddedTables { get; }
        IReadOnlyList<IDataTable> ResetTables { get; }
        IReadOnlyList<IDataTable> DroppedTables { get; }
    }
}