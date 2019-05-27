using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Resources.SaveLoad
{
    public interface IResourceSaver
    {
        bool CanSaveNatively(IResource resource);
        bool CanSaveByConversion(IResource resource);
        string SuggestFileName(IResource resource, string resourceName = null);
        void Save(IResource resource, IFileSystem fileSystem, string path);
    }
}