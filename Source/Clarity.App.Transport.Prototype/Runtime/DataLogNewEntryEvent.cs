using Clarity.App.Transport.Prototype.Databases;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class DataLogNewEntryEvent : RoutedEventBase, IDataLogNewEntryEvent
    {
        public IDataLogEntry Entry { get; private set; }

        public void Reset(IDataLogEntry entry)
        {
            Entry = entry;
            StopPropagation = false;
        }
    }
}