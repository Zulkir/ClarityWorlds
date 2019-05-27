using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.Assets
{
    public interface IAsset
    {
        string Name { get; }
        IResource Resource { get; }
        AssetStorageType StorageType { get; }
        AssetHashMd5 FilesHashMd5 { get; }
        string ReferencePath { get; }
        string AssetCachePath { get; }
    }
}