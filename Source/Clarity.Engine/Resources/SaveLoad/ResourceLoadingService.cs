using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Resources.SaveLoad 
{
    public class ResourceLoadingService : IResourceLoadingService
    {
        private readonly IReadOnlyList<IResourceLoader> loaders;

        public ResourceLoadingService(IReadOnlyList<IResourceLoader> loaders)
        {
            this.loaders = loaders.Reverse().ToArray();
        }

        public IResource Load(Type type, IFileSystem fileSystem, string path)
        {
            foreach (var loader in loaders)
                if (loader.TryLoad(type, fileSystem, path, out var resource))
                    return resource;
            throw new NotSupportedException($"No loader is able to load resource '{path}'.");
        }
    }
}