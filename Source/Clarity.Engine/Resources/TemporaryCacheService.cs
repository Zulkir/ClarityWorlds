using System;
using System.Collections.Generic;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Resources
{
    public class TemporaryCacheService : ITemporaryCacheService
    {
        #region Nested Types
        private struct Key : IEquatable<Key>
        {
            public object Resource;
            public string CacheKey;

            public Key(object resource, string cacheKey)
            {
                Resource = resource;
                CacheKey = cacheKey;
            }

            public bool Equals(Key other) => Resource == other.Resource && CacheKey == other.CacheKey;
            public override bool Equals(object obj) => obj is Key key && Equals(key);
            public override int GetHashCode() => ((Resource?.GetHashCode() ?? 0) * 397) ^ (CacheKey?.GetHashCode() ?? 0);
        }

        private struct Value
        {
            public object Cache;
            public float DeathTimestamp;

            public Value(object cache, float deathTimestamp)
            {
                Cache = cache;
                DeathTimestamp = deathTimestamp;
            }
        }
        #endregion

        private readonly IRenderLoopDispatcher renderLoopDispatcher;
        private readonly Dictionary<Key, Value> caches;
        private readonly List<Key> deathNote;

        public TemporaryCacheService(IRenderLoopDispatcher renderLoopDispatcher)
        {
            this.renderLoopDispatcher = renderLoopDispatcher;
            caches = new Dictionary<Key, Value>();
            deathNote = new List<Key>();
            renderLoopDispatcher.AfterAll += OnUpdate;
        }

        public TCache GetTemporaryCache<TResource, TCache>(TResource resource, string cacheKey, Func<TResource, TCache> create, float secondsToLive)
        {
            var key = new Key(resource, cacheKey);
            if (caches.TryGetValue(key, out var value))
                return (TCache)value.Cache;
            var cache = create(resource);
            value = new Value(cache, renderLoopDispatcher.CurrentTimestamp + secondsToLive);
            caches.Add(key, value);
            return cache;
        }

        private void OnUpdate(FrameTime frameTime)
        {
            foreach (var kvp in caches)
                if (kvp.Value.DeathTimestamp > frameTime.TotalSeconds)
                    deathNote.Add(kvp.Key);
            foreach (var key in deathNote)
                caches.Remove(key);
            deathNote.Clear();
        }
    }
}