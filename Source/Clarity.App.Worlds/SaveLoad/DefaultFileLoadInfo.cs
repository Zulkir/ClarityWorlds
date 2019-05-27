using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Logging;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad
{
    public class DefaultFileLoadInfo : IFileLoadInfo
    {
        public IFileSystem FileSystem { get; }
        public string FilePath { get; }
        private readonly LoadWorldPreference worldPreference;
        private readonly IWorldTreeService worldTreeService;
        private readonly IAssetService assetService;

        public bool PreferReadOnly => worldPreference == LoadWorldPreference.PreferReadOnly || 
                                      worldPreference == LoadWorldPreference.ReadOnlyOnly;

        public DefaultFileLoadInfo(string filePath, IAssetService assetService, IWorldTreeService worldTreeService, LoadWorldPreference worldPreference)
        {
            FileSystem = new ActualFileSystem();
            FilePath = filePath;
            this.assetService = assetService;
            this.worldTreeService = worldTreeService;
            this.worldPreference = worldPreference;
        }

        public IAsset OnFoundAsset(AssetLoadInfo assetLoadInfo)
        {
            var result = assetService.Load(assetLoadInfo);
            if (!result.Successful)
                // todo: IUserNotificationService
                // todo: substitute default asset
                Log.Write(LogMessageType.HandledError, "Error while loading an asset from local storage:\n" + result.Message);
            return result.Asset;
        }

        public void OnLoadedWorld(IWorld world)
        {
            if (worldPreference == LoadWorldPreference.ReadOnlyOnly)
                // todo: IUserNotificationService
                // todo: reset world
                Log.Write(LogMessageType.HandledError, "ReadOnly world not found in the file.");
            worldTreeService.World = world;
        }

        public void OnLoadedReadOnlyWorld(IWorld world)
        {
            if (worldPreference == LoadWorldPreference.EditableOnly)
                // todo: IUserNotificationService
                // todo: reset world
                Log.Write(LogMessageType.HandledError, "Editable world not found in the file.");
            worldTreeService.World = world;
        }

        public void OnLoadedNoWorld()
        {
            // todo: reset world
            Log.Write(LogMessageType.HandledError, "No world found in the file.");
        }
    }
}