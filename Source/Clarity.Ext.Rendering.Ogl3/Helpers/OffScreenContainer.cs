using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Rendering.Ogl3.Helpers
{
    public class OffScreenContainer : IOffScreenContainer
    {
        private class DictItem
        {
            public IOffScreen OffScreen { get; }
            public float LastUsedTimestamp { get; set; }
            public float TimeToLive { get; set; }

            public DictItem(IGraphicsInfra infra)
            {
                OffScreen = new OffScreen(infra);
            }
        }

        private readonly IGraphicsInfra infra;
        private readonly Dictionary<Pair<object>, DictItem> dict;
        private readonly List<Pair<object>> deathNote;
        private float currentTimestamp;

        public OffScreenContainer(IGraphicsInfra infra, IEventRoutingService eventRoutingService)
        {
            this.infra = infra;
            dict = new Dictionary<Pair<object>, DictItem>();
            deathNote= new List<Pair<object>>();
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(OffScreenContainer), nameof(Update), Update);
        }

        public IOffScreen Get(object service, object surface, int width, int height, int samples, float ttl)
        {
            var key = Tuples.SameTypePair(service, surface);
            var item = dict.GetOrAdd(key, x => new DictItem(infra));
            item.LastUsedTimestamp = currentTimestamp;
            item.TimeToLive = ttl;
            item.OffScreen.Prepare(width, height, samples);
            return item.OffScreen;
        }

        private void Update(INewFrameEvent newFrameEvent)
        {
            currentTimestamp = newFrameEvent.FrameTime.TotalSeconds;
            foreach (var kvp in dict)
                if (kvp.Value.LastUsedTimestamp + kvp.Value.TimeToLive < currentTimestamp)
                {
                    deathNote.Add(kvp.Key);
                    // todo: Dispose by GC?
                    kvp.Value.OffScreen.Dispose();
                }
            foreach (var key in deathNote)
                dict.Remove(key);
            deathNote.Clear();
        }
    }
}