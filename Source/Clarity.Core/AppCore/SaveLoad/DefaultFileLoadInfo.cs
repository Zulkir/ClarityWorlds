using Clarity.Common.Infra.Files;
using Clarity.Core.AppCore.Logging;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class DefaultFileLoadInfo : IFileLoadInfo
    {
        public IFileSystem FileSystem { get; }
        public string FilePath { get; }
        private readonly IWorldTreeService worldTreeService;
        private readonly IAssetService assetService;

        public DefaultFileLoadInfo(string filePath, IAssetService assetService, IWorldTreeService worldTreeService)
        {
            FileSystem = new ActualFileSystem();
            FilePath = filePath;
            this.assetService = assetService;
            this.worldTreeService = worldTreeService;
        }

        public IAsset OnFoundAsset(AssetLoadInfo assetLoadInfo)
        {
            var result = assetService.Load(assetLoadInfo);
            if (!result.Successful)
                // todo: IUserNotificationService
                Log.Write(LogMessageType.HandledError, "Error while loading an asset from local storage:\n" + result.Message);
            return result.Asset;
        }

        public void OnLoadedWorld(IWorld world)
        {
            worldTreeService.World = world;
        }
    }
}