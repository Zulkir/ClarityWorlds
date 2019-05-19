using Clarity.Common.Infra.Files;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.SaveLoad
{
    public interface IFileLoadInfo
    {
        IFileSystem FileSystem { get; }
        string FilePath { get; }
        IAsset OnFoundAsset(AssetLoadInfo assetLoadInfo);
        void OnLoadedWorld(IWorld world);
    }
}