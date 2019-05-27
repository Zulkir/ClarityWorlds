using System.Linq;
using Assets.Scripts.Rendering;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.SaveLoad;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Viewports;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class UcGui : IGui
    {
        private readonly IDiContainer di;
        private readonly IEventSender eventSender;

        public IRenderGuiControl RenderControl { get; }

        public UcGui(IDiContainer di, IEventSender eventSender, IEventRoutingService eventRoutingService)
        {
            this.di = di;
            this.eventSender = eventSender;
            RenderControl = new UcRenderGuiControl();
            eventRoutingService.RegisterServiceDependency(typeof(UcGui), typeof(IWorldTreeService));
            //eventRoutingService.Subscribe<IAppModeChangedEvent>(typeof(UcGui), nameof(OnAppModeChanged), OnAppModeChanged);
            eventRoutingService.SubscribeToAllAfter(typeof(UcGui).FullName, OnEveryEvent, true);
        }

        public void SwitchToPresentationMode()
        {
        }

        public void SwitchToEditMode()
        {
        }

        public void Run()
        {
            //var assetService = di.Get<IAssetService>();
            //var assetLoaders = di.GetMulti<IAssetLoader>();
            //var imageLoader = assetLoaders.ThatAre<UcImageAssetLoader>().First();

            SetupLoop();

            var saveLoadService = di.Get<ISaveLoadService>();
            saveLoadService.Format = di.GetMulti<ISaveLoadFormat>().First(x => x is ZipSaveLoadFormat);
            saveLoadService.FileName = "C:/clarity/UnityTestWorld.cw";
            saveLoadService.Load(LoadWorldPreference.ReadOnlyOnly);
            var appModeService = di.Get<IAppModeService>();
            appModeService.SetMode(AppMode.Presentation);
            var worldTreeService = di.Get<IWorldTreeService>();
            
            var presentationView = AmFactory.Create<PresentationView>();
            presentationView.FocusOn(worldTreeService.PresentationWorld.Scenes.First().Root.GetComponent<IFocusNodeComponent>());
            var viewport = AmFactory.Create<Viewport>();
            viewport.View = presentationView;
            RenderControl.SetViewports(
                new [] {viewport}, 
                new ViewportsLayout
                {
                    RowHeights = new[] {new ViewportLength(100, ViewportLengthUnit.Percent)},
                    ColumnWidths = new[] {new ViewportLength(100, ViewportLengthUnit.Percent)},
                    ViewportIndices = new[,]{{0}}
                });
        }

        private void SetupLoop()
        {
            var renderLoopDispatcher = di.Get<IRenderLoopDispatcher>();
            var inputProviders = di.GetMulti<IUcInputProvider>();
            eventSender.OnGUIEvent += () =>
            {
                foreach (var inputProvider in inputProviders)
                    inputProvider.OnGui();
            };
            eventSender.UpdateEvent += () =>
            {
                foreach (var inputProvider in inputProviders)
                    inputProvider.OnUpdate();
                renderLoopDispatcher.OnLoop(new FrameTime(Time.realtimeSinceStartup, Time.deltaTime));
            };
        }

        private void OnEveryEvent(IRoutedEvent evnt)
        {
            if (RenderControl.Viewports == null) 
                return;
            foreach (var viewport in RenderControl.Viewports)
                viewport.View.OnEveryEvent(evnt);
        }
    }
}