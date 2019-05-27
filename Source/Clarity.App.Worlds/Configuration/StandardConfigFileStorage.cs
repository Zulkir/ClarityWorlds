using Clarity.Common.Infra.Files;

namespace Clarity.App.Worlds.Configuration
{
    public class StandardConfigFileStorage : IConfigFileStorage
    {
        public IFileSystem FileSystem { get; }

        public StandardConfigFileStorage()
        {
            FileSystem = new ActualFileSystem("Config");
        }
    }
}