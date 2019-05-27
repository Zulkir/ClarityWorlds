using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Queries.Data;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class DataQueriesService : IDataQueriesService
    {
        private readonly IEventRoutingService eventRoutingService;
        private readonly List<IDataQuery> queries;

        public IReadOnlyList<IDataQuery> Queries => queries;

        public DataQueriesService(IEventRoutingService eventRoutingService)
        {
            this.eventRoutingService = eventRoutingService;
            queries = new List<IDataQuery>();
            eventRoutingService.RegisterServiceDependency(typeof(IDataQueriesService), typeof(IOriginDataLogService));
            eventRoutingService.Subscribe<IDataLogUpdatedEvent>(typeof(IDataQueriesService), nameof(OnDataLogUpdated), OnDataLogUpdated);
            eventRoutingService.Subscribe<IDataSourceChangedEvent>(typeof(IDataQueriesService), nameof(OnDataSourceChanged), OnDataSourceChanged);
        }

        public void AddQuery(IDataQuery query)
        {
            queries.Add(query);
            query.DataLogUpdated += OnQueryDataLogUpdated;
            query.OnAttached();
        }

        public void RemoveQuery(int index)
        {
            queries[index].DataLogUpdated -= OnQueryDataLogUpdated;
            queries.RemoveAt(index);
        }

        private void OnQueryDataLogUpdated(IDataLogUpdatedEvent evnt)
        {
            eventRoutingService.FireEvent(evnt);
        }

        public void OnTimestampChanged(double timestamp)
        {
            foreach (var query in Queries)
                query.OnTimestampChanged(timestamp);
        }

        private void OnDataSourceChanged(IDataSourceChangedEvent evnt)
        {
            foreach (var query in Queries)
                query.OnTableLayoutChanged();
        }

        private void OnDataLogUpdated(IDataLogUpdatedEvent evnt)
        {
            foreach (var query in Queries)
                query.OnDataLogUpdated(evnt);
        }
    }
}