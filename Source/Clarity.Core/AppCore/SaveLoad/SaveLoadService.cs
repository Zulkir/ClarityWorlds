using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Core.AppCore.WorldTree;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly Lazy<IDefaultStateInitializer> defaultStateInitializerLazy;
        private readonly IWorldTreeService worldTreeService;
        private readonly IAssetService assetService;

        public string FileName { get; set; }
        public bool HasUnsavedChanges { get; private set; }
        public ISaveLoadFormat Format { get; set; }

        private IDefaultStateInitializer DefaultStateInitializer => defaultStateInitializerLazy.Value;
        
        public SaveLoadService(IWorldTreeService worldTreeService, Lazy<IDefaultStateInitializer> defaultStateInitializerLazy, IWorldTreeService worldTreeService1, IAssetService assetService)
        {
            this.defaultStateInitializerLazy = defaultStateInitializerLazy;
            this.worldTreeService = worldTreeService1;
            this.assetService = assetService;
            worldTreeService.UpdatedHigh += OnSceneServiceUpdated;
            // todo: on asset changes
        }

        private void OnSceneServiceUpdated(IAmEventMessage eventMessage) => 
            HasUnsavedChanges = true;

        public void New()
        {
            FileName = null;
            assetService.DeleteAll();
            DefaultStateInitializer.InitializeAll();
        }

        public void Save()
        {
            Format.Save(new DefaultFileSaveInfo(FileName, worldTreeService.World, assetService));
        }

        public void Load()
        {
            assetService.DeleteAll();
            Format.Load(new DefaultFileLoadInfo(FileName, assetService, worldTreeService));
        }
    }
}