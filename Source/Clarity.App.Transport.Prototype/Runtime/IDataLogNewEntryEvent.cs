using Clarity.App.Transport.Prototype.Databases;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IDataLogNewEntryEvent : IRoutedEvent
    {
        IDataLogEntry Entry { get; }
    }
}