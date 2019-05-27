using Clarity.App.Worlds.Assets;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad
{
    public interface IFileLoadInfo
    {
        IFileSystem FileSystem { get; }
        string FilePath { get; }
        bool PreferReadOnly { get; }
        IAsset OnFoundAsset(AssetLoadInfo assetLoadInfo);
        void OnLoadedWorld(IWorld world);
        void OnLoadedReadOnlyWorld(IWorld world);
        void OnLoadedNoWorld();
    }
}