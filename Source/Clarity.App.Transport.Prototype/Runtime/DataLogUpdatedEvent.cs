using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class DataLogUpdatedEvent : RoutedEventBase, IDataLogUpdatedEvent
    {
        public double StartTimestamp { get; }
        public IReadOnlyList<IDataTable> TablesAffected { get; }

        public DataLogUpdatedEvent(double startTimestamp, IReadOnlyList<IDataTable> tablesAffected)
        {
            StartTimestamp = startTimestamp;
            TablesAffected = tablesAffected;
        }
    }
}