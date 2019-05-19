using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.AppModes;
using Clarity.Core.AppCore.Configuration;
using Clarity.Core.AppCore.CopyPaste;
using Clarity.Core.AppCore.Coroutines;
using Clarity.Core.AppCore.Gui;
using Clarity.Core.AppCore.Input;
using Clarity.Core.AppCore.Interaction.Queries;
using Clarity.Core.AppCore.Logging;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Core.AppCore.SaveLoad;
using Clarity.Core.AppCore.SaveLoad.Converters;
using Clarity.Core.AppCore.SaveLoad.NecessitiesProviders;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Core.AppCore.Tools;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Core.AppFeatures.Models;
using Clarity.Core.AppFeatures.StoryLayouts.NestedCircles;
using Clarity.Core.AppFeatures.StoryLayouts.NestedSpheres;
using Clarity.Core.External.ObjLoading;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Platforms;
using Clarity.Engine.Serialization;

namespace Clarity.Core.AppCore.Infra
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
            di.Bind<ITypeAliasesSaveLoader>().To<TypeAliasesSaveLoader>();
            di.Bind<ISaveLoadConverterContainer>().To<SaveLoadConverterContainer>();
            di.Bind<IStoryService>().To<StoryService>();
            di.Bind<IConfigService>().To<ConfigService>();
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