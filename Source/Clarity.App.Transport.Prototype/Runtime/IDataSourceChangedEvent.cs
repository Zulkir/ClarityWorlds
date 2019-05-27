using Clarity.App.Transport.Prototype.DataSources;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IDataSourceChangedEvent : IRoutedEvent
    {
        IDataSource NewDataSource { get; }
    }
}