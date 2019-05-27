using System;
using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Resources.SaveLoad
{
    public interface IResourceLoader
    {
        bool TryLoad(Type type, IFileSystem fileSystem, string path, out IResource resource);
    }
}