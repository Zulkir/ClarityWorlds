using Clarity.App.Worlds.Assets;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.App.Worlds.SaveLoad
{
    [TrwSerialize]
    public class AssetSaveLoadInfo
    {
        [TrwSerialize]
        public string AssetName { get; set; }
        
        [TrwSerialize]
        public AssetStorageType StorageType { get; set; }

        [TrwSerialize]
        public string ReferencePath { get; set; }

        [TrwSerialize]
        public string LocalCopyPath { get; set; }

        public AssetSaveLoadInfo()
        {
            
        }

        public AssetSaveLoadInfo(IAsset asset, string localCopyPath)
        {
            AssetName = asset.Name;
            StorageType = asset.StorageType;
            ReferencePath = asset.ReferencePath;
            LocalCopyPath = localCopyPath;
        }
    }
}