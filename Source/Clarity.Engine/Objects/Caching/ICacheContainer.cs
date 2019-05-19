using System;
using System.Collections.Generic;

namespace Clarity.Engine.Objects.Caching
{
    public interface ICacheContainer : IDisposable
    {
        IEnumerable<ICache> GetAll();
        T GetOrAddCache<T>(Func<T> create) where T : ICache;
        TCache GetOrAddCache<TClosure, TCache>(TClosure closure, Func<TClosure, TCache> create)
            where TCache : ICache;
    }
}