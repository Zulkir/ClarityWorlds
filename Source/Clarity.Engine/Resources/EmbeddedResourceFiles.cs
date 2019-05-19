using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Resources
{
    public class EmbeddedResourceFiles : IEmbeddedResourceFiles
    {
        public IReadOnlyFileSystem FileSystem { get; }

        public EmbeddedResourceFiles()
        {
            FileSystem = new ActualFileSystem("../../Resources/");
        }
    }
}