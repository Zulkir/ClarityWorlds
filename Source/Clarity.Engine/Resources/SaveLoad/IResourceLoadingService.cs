using System;
using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Resources.SaveLoad
{
    public interface IResourceLoadingService
    {
        IResource Load(Type type, IFileSystem fileSystem, string path);
    }
}