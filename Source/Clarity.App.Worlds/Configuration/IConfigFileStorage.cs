using Clarity.Common.Infra.Files;

namespace Clarity.App.Worlds.Configuration
{
    public interface IConfigFileStorage
    {
        IFileSystem FileSystem { get; }
    }
}