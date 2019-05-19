using Clarity.Common.Infra.Files;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    public class AssetLoadInfo
    {
        public string AssetName { get; set; }
        public IFileSystem FileSystem { get; set; }
        public string LoadPath { get; set; }
        public string ReferencePath { get; set; }
        public AssetStorageType StorageType { get; set; }
    }
}