using System;

namespace Clarity.Engine.Resources
{
    public interface ITemporaryCacheService
    {
        TCache GetTemporaryCache<TResource, TCache>(TResource resource, string cacheKey, Func<TResource, TCache> create, float secondsToLive);
    }
}