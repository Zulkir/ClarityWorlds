using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Queries.Visual;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class VisualQueriesRuntime : IVisualQueriesRuntime
    {
        public ISceneNode RootNode { get; }
        private readonly List<IVisualQuery> queries;

        public IReadOnlyList<IVisualQuery> Queries => queries;

        public VisualQueriesRuntime(IEventRoutingService eventRoutingService)
        {
            RootNode = AmFactory.Create<SceneNode>();
            queries = new List<IVisualQuery>();
            eventRoutingService.RegisterServiceDependency(typeof(IVisualQueriesRuntime), typeof(IOriginDataLogService));
            eventRoutingService.RegisterServiceDependency(typeof(IVisualQueriesRuntime), typeof(IDataQueriesService));
            //eventRoutingService.Subscribe<INewFrameEvent>(typeof(IVisualQueriesRuntime), nameof(OnNewFrame), OnNewFrame);
            eventRoutingService.Subscribe<IDataLogUpdatedEvent>(typeof(IVisualQueriesRuntime), nameof(OnDataLogUpdated), OnDataLogUpdated);
            eventRoutingService.Subscribe<IDataSourceChangedEvent>(typeof(IVisualQueriesRuntime), nameof(OnDataSourceChanged), OnDataSourceChanged);
        }

        public void AddQuery(IVisualQuery query)
        {
            queries.Add(query);
            RootNode.ChildNodes.Add(query.SceneNode);
        }

        public void RemoveQuery(int index)
        {
            var query = queries[index];
            queries.RemoveAt(index);
            RootNode.ChildNodes.Remove(query.SceneNode);
        }

        public void OnNewFrame(FrameTime frameTime)
        {
            foreach (var query in queries)
                query.Update(frameTime);
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