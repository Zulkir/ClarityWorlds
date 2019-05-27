using Clarity.Common.CodingUtilities;
using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Media.Skyboxes
{
    public interface ISkyboxLoader
    {
        bool TryLoad(IReadOnlyFileSystem fileSystem, string path, out ISkybox skybox, out string[] imageFileRelativePaths, out ErrorInfo error);
    }
}