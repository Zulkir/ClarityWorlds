using System.Collections.Generic;
using Clarity.Engine.Objects.Caching;

namespace Clarity.Engine.Resources
{
    public abstract class ResourceBase : IResource
    {
        public IResourceSource Source { get; set; }
        public ResourceVolatility Volatility { get; }

        public Dictionary<string, IResource> Subresources { get; }
        public ICacheContainer CacheContainer { get; }

        IReadOnlyDictionary<string, IResource> IResource.Subresources => Subresources;

        protected ResourceBase(ResourceVolatility volatility)
        {
            Volatility = volatility;
            Subresources = new Dictionary<string, IResource>();
            CacheContainer = new CacheContainer();
        }

        public virtual void Dispose()
        {
            CacheContainer.Dispose();
            foreach (var subresource in Subresources.Values)
                subresource.Dispose();
        }

        public void OnModified(object args)
        {
            foreach (var cache in CacheContainer.GetAll())
                cache.OnMasterEvent(args);
        }

        public void AddSubresource(string key, IResource subresource)
        {
            subresource.Source = new SubresourceResourceSource(this, key);
            Subresources.Add(key, subresource);
        }
    }
}