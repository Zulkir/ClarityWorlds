using Clarity.App.Transport.Prototype.DataSources;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class DataSourceRuntime : IDataSourceRuntime
    {
        private readonly IEventRoutingService eventRoutingService;

        public IDataSource Source { get; private set; }

        public DataSourceRuntime(IEventRoutingService eventRoutingService)
        {
            this.eventRoutingService = eventRoutingService;
            Source = new EmptyDataSource();
        }

        public void ChangeDataSource(IDataSource newDataSource)
        {
            Source.Dispose();
            Source = newDataSource;
            eventRoutingService.FireEvent<IDataSourceChangedEvent>(new DataSourceChangedEvent(newDataSource));
        }
    }
}