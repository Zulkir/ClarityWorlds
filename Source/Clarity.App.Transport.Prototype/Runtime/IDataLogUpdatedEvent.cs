using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IDataLogUpdatedEvent : IRoutedEvent
    {
        double StartTimestamp { get; }
        IReadOnlyList<IDataTable> TablesAffected { get; }
    }
}