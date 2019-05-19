using System;
using System.Collections.Generic;
using Clarity.Engine.Objects.Caching;
using JetBrains.Annotations;

namespace Clarity.Engine.Resources
{
    public interface IResource : IDisposable
    {
        ResourceVolatility Volatility { get; }
        [CanBeNull] IResourceSource Source { get; set; }
        IReadOnlyDictionary<string, IResource> Subresources { get; }
        ICacheContainer CacheContainer { get; }
    }
}