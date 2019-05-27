using Clarity.App.Transport.Prototype.DataSources;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class DataSourceChangedEvent : RoutedEventBase, IDataSourceChangedEvent
    {
        public IDataSource NewDataSource { get; }

        public DataSourceChangedEvent(IDataSource newDataSource)
        {
            NewDataSource = newDataSource;
        }
    }
}