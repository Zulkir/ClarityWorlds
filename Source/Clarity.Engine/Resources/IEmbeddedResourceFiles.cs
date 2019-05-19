using Clarity.Common.Infra.Files;

namespace Clarity.Engine.Resources
{
    public interface IEmbeddedResourceFiles
    {
        IReadOnlyFileSystem FileSystem { get; }
    }
}