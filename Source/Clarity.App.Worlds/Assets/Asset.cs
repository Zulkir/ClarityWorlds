using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.Assets
{
    public class Asset : IAsset
    {
        public string Name { get; }
        public IResource Resource { get; }
        public AssetStorageType StorageType { get; }
        public AssetHashMd5 FilesHashMd5 { get; }
        public string ReferencePath { get; }
        public string AssetCachePath { get; }

        public Asset(string name, IResource resource, AssetStorageType storageType, AssetHashMd5 filesHashMd5, string referencePath, string assetCachePath)
        {
            Name = name;
            Resource = resource;
            StorageType = storageType;
            FilesHashMd5 = filesHashMd5;
            ReferencePath = referencePath;
            AssetCachePath = assetCachePath;
            resource.Source = new FromAssetResourceSource(this);
        }
    }
}