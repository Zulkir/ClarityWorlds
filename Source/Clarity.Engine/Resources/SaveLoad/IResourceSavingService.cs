using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Resources.SaveLoad
{
    public interface IResourceSavingService
    {
        string SuggestFileName(IResource resource, string resourceName = null);
        void SaveResource(IResource resource, IFileSystem fileSystem, string path);
    }
}