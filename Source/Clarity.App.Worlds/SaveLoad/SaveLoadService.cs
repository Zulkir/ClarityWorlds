using System;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.SaveLoad.ReadOnly;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui.MessageBoxes;

namespace Clarity.App.Worlds.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IEventRoutingService eventRoutingService;
        private readonly Lazy<IDefaultStateInitializer> defaultStateInitializerLazy;
        private readonly IWorldTreeService worldTreeService;
        private readonly IAssetService assetService;
        private readonly IReadOnlyWorldBuilder readOnlyWorldBuilder;
        private readonly IMessageBoxService messageBoxService;

        public string FileName { get; set; }
        public bool HasUnsavedChanges { get; private set; }
        public ISaveLoadFormat Format { get; set; }

        private IDefaultStateInitializer DefaultStateInitializer => defaultStateInitializerLazy.Value;
        
        public SaveLoadService(IEventRoutingService eventRoutingService, IWorldTreeService worldTreeService, 
            Lazy<IDefaultStateInitializer> defaultStateInitializerLazy, IAssetService assetService, 
            IEventRoutingService eventRoutingService1, IReadOnlyWorldBuilder readOnlyWorldBuilder, IMessageBoxService messageBoxService)
        {
            this.eventRoutingService = eventRoutingService1;
            this.readOnlyWorldBuilder = readOnlyWorldBuilder;
            this.messageBoxService = messageBoxService;
            this.defaultStateInitializerLazy = defaultStateInitializerLazy;
            this.worldTreeService = worldTreeService;
            this.assetService = assetService;
            eventRoutingService.Subscribe<IWorldTreeUpdatedEvent>(typeof(ISaveLoadService), nameof(OnWorldTreeUpdated), OnWorldTreeUpdated);
            // todo: on asset changes
        }

        private void OnWorldTreeUpdated(IWorldTreeUpdatedEvent evnt) => 
            HasUnsavedChanges = true;

        // todo: make DefaultSceneInitializer / WorldTreeService / AssetService react to events?

        public void New()
        {
            FileName = null;
            assetService.DeleteAll();
            DefaultStateInitializer.InitializeAll();
            eventRoutingService.FireEvent<ISaveLoadEvent>(new SaveLoadEvent(SaveLoadEventType.New, worldTreeService.World, assetService.Assets));
        }

        public void Save(SaveWorldFlags worldFlags)
        {
            var editingWorld = worldTreeService.EditingWorld;
            if (SaveLoadWorldProperties.Get(editingWorld).IsReadOnly)
            {
                messageBoxService.Show("Cannot save a read-only world.", MessageBoxButtons.Ok, MessageBoxType.Error);
                return;
            }

            var editableWorld = worldFlags.HasFlag(SaveWorldFlags.EditableWorld)
                ? editingWorld
                : null;
            var readOnlyWorld = worldFlags.HasFlag(SaveWorldFlags.ReadOnlyWorld)
                ? readOnlyWorldBuilder.BuildReadOnly(editingWorld)
                : null;
            Format.Save(new DefaultFileSaveInfo(FileName, editableWorld, readOnlyWorld, assetService));
            eventRoutingService.FireEvent<ISaveLoadEvent>(new SaveLoadEvent(SaveLoadEventType.Save, worldTreeService.World, assetService.Assets));
        }

        public void Load(LoadWorldPreference worldPreference)
        {
            assetService.DeleteAll();
            Format.Load(new DefaultFileLoadInfo(FileName, assetService, worldTreeService, worldPreference));
            eventRoutingService.FireEvent<ISaveLoadEvent>(new SaveLoadEvent(SaveLoadEventType.Load, worldTreeService.World, assetService.Assets));
        }
    }
}