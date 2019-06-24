using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Configuration;
using Clarity.App.Worlds.CopyPaste;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.DirtyHacks;
using Clarity.App.Worlds.External.ObjLoading;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.Interaction.Queries;
using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.Logging;
using Clarity.App.Worlds.Misc.HighlightOnMouse;
using Clarity.App.Worlds.Models;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.SaveLoad;
using Clarity.App.Worlds.SaveLoad.Converters;
using Clarity.App.Worlds.SaveLoad.NecessitiesProviders;
using Clarity.App.Worlds.SaveLoad.ReadOnly;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryLayouts.Museum;
using Clarity.App.Worlds.StoryLayouts.NestedCircles;
using Clarity.App.Worlds.StoryLayouts.NestedSpheres;
using Clarity.App.Worlds.StoryLayouts.Orbit;
using Clarity.App.Worlds.StoryLayouts.Sphere;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Platforms;
using Clarity.Engine.Serialization;

namespace Clarity.App.Worlds.Infra
{
    public class AppLifecycle : EngineLifecycle
    {
        public void StartAndRun(IEnvironment environment)
        {
            var di = new DiContainer();
            BindDefaults(di);
            BindExtensions(di, environment);
            InitializeStatics(di);
            StartupExtensions(di, environment);
            StartupCore(di);
            Run(di);
        }

        protected override void BindDefaults(IDiContainer di)
        {
            base.BindDefaults(di);
            di.Bind<IInputHandler>().To<InputHandler>();
            di.Bind<IUndoRedoService>().To<UndoRedoService>();
            di.Bind<IViewService>().To<ViewService>();
            di.Bind<IWorldTreeService>().To<WorldTreeService>();
            di.Bind<IToolService>().To<ToolService>();
            di.Bind<IToolFactory>().To<ToolFactory>();
            di.Bind<IDefaultStateInitializer>().To<DefaultStateInitializer>();
            di.Bind<ISaveLoadService>().To<SaveLoadService>();
            di.Bind<IAssetService>().To<AssetService>();
            di.Bind<IAssetFileCache>().To<AssetFileCache>();
            di.Bind<IWorldCopyPasteService>().To<WorldCopyPasteService>();
            di.Bind<IAppModeService>().To<AppModeService>();
            di.Bind<IPresentationWorldBuilder>().To<PresentationWorldBuilder>();
            di.Bind<ICommonNodeFactory>().To<CommonNodeFactory>();
            di.BindMulti<IAssetLoader>().To<ImageAssetLoader>();
            di.BindMulti<IAssetLoader>().To<SkyboxAssetLoader>();
            di.BindMulti<IAssetLoader>().To<ObjGeoModelLoader>();
            di.BindMulti<IAssetLoader>().To<CgModelLoader>();
            di.Bind<ILogService>().To<LogService>();
            di.BindMulti<ISerializationNecessitiesProvider>().To<CommonSerializationNecessitiesProvider>();
            di.BindMulti<ISerializationNecessitiesProvider>().To<AmSerializationNecessitiesProvider>();
            di.BindMulti<ISerializationNecessitiesProvider>().To<WorldSerializationNecessitiesProvider>();
            di.BindMulti<ILogWriter>().To<FileLogWriter>();
            di.Bind<IImageLoader>().To<SysDrawImageLoader>();
            di.Bind<ISaveLoadFactory>().To<SaveLoadFactory>();
            di.BindMulti<ISaveLoadFormat>().To<ZipSaveLoadFormat>();
            di.Bind<ISaveLoadConverterContainer>().To<SaveLoadConverterContainer>();
            di.Bind<IStoryService>().To<StoryService>();
            di.Bind<IConfigService>().To<ConfigService>();
            di.Bind<IConfigFileStorage>().To<StandardConfigFileStorage>();
            di.Bind<ICoroutineService>().To<CoroutineService>();
            di.BindMulti<IStoryLayout>().To<SphereStoryLayout>();
            di.BindMulti<IStoryLayout>().To<OrbitStoryLayout>();
            di.BindMulti<IStoryLayout>().To<MuseumStoryLayout>();
            di.BindMulti<IStoryLayout>().To<NestedSpheresStoryLayout>();
            di.BindMulti<IStoryLayout>().To<NestedCirclesStoryLayout>();
            di.Bind<INavigationService>().To<NavigationService>();
            di.Bind<IUserQueryService>().To<UserQueryService>();
            di.Bind<IPresentationGuiCommands>().To<PresentationGuiCommands>();
            di.Bind<ISceneNodeContextMenuBuilder>().To<SceneNodeContextMenuBuilder>();
            di.Bind<IReadOnlyWorldBuilder>().To<ReadOnlyWorldBuilder>();
            di.Bind<IRtEmbeddingHandlerContainer>().To<RtEmbeddingHandlerContainer>();
            di.Bind<IHighlightOnMouseService>().To<HighlightOnMouseService>();
            di.Bind<IDirtyHackService>().To<DirtyHackService>();
        }

        protected override void InitializeStatics(IDiContainer di)
        {
            base.InitializeStatics(di);
            Log.Initialize(di.Get<ILogService>());
            Config.Initialize(di.Get<IConfigService>());
        }

        protected override void StartupCore(IDiContainer di)
        {
            base.StartupCore(di);
            di.Get<IDefaultStateInitializer>().InitializeAll();
        }

        protected virtual void Run(IDiContainer di)
        {
            di.Get<IGui>().Run();
        }
    }
}