using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Media.Skyboxes
{
    public interface ISkyboxLoader
    {
        ISkybox Load(IReadOnlyFileSystem fileSystem, string path, out string[] imageFileRelativePaths);
    }
}