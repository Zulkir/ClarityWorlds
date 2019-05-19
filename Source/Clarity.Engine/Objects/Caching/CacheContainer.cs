using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Unmanaged;

namespace Clarity.Engine.Objects.Caching
{
    public class CacheContainer : AutoDisposableBase, ICacheContainer
    {
        private readonly Dictionary<Type, ICache> caches;

        public CacheContainer()
        {
            caches = new Dictionary<Type, ICache>();
        }

        protected override void Dispose(bool explicitly)
        {
            foreach (var cache in caches.Values)
                cache.Dispose();
        }   

        public IEnumerable<ICache> GetAll()
        {
            return caches.Values;
        }

        public T GetOrAddCache<T>(Func<T> create) where T : ICache
        {
            return (T)caches.GetOrAdd(typeof(T), x => create());
        }

        public TCache GetOrAddCache<TClosure, TCache>(TClosure closure, Func<TClosure, TCache> create) where TCache : ICache
        {
            var key = typeof(TCache);
            if (!caches.TryGetValue(key, out var cache))
            {
                cache = create(closure);
                caches.Add(key, cache);
            }
            return (TCache)cache;
        }
    }
}