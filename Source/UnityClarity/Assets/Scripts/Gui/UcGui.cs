using System.Linq;
using Assets.Scripts.Infra;
using Assets.Scripts.Rendering;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.SaveLoad;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
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
        private readonly IEventRoutingService eventRoutingService;
        private readonly UcRenderGuiControl renderControl;

        public IRenderGuiControl RenderControl => renderControl;

        public UcGui(IDiContainer di, IEventRoutingService eventRoutingService)
        {
            this.di = di;
            renderControl = new UcRenderGuiControl();
            this.eventRoutingService = eventRoutingService;
            eventRoutingService.RegisterServiceDependency(typeof(UcGui), typeof(IWorldTreeService));
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
            var eventSender = di.Get<IGlobalObjectService>().EventObject.GetComponent<EventSender>();
            SetupLoop(eventSender);

            var saveLoadService = di.Get<ISaveLoadService>();
            saveLoadService.Format = di.GetMulti<ISaveLoadFormat>().First(x => x is ZipSaveLoadFormat);
            saveLoadService.FileName = SceneParameters.PresentationFilePath;
            saveLoadService.Load(LoadWorldPreference.ReadOnlyOnly);
            var appModeService = di.Get<IAppModeService>();
            appModeService.SetMode(AppMode.Presentation);
            var worldTreeService = di.Get<IWorldTreeService>();
            var storyGraph = di.Get<IStoryService>().GlobalGraph;
            var firstNodeId = storyGraph.Leaves.First();
            while (storyGraph.Previous[firstNodeId].HasItems())
                firstNodeId = storyGraph.Previous[firstNodeId].First();
            di.Get<INavigationService>().GoToSpecific(firstNodeId);
            var presentationView = AmFactory.Create<PresentationView>();
            presentationView.FocusOn(worldTreeService.GetById(firstNodeId).GetComponent<IFocusNodeComponent>());
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
            var viewService = di.Get<IViewService>();
            viewService.ChangeRenderingArea(RenderControl, presentationView);
            if (SceneParameters.IsTutorial)
            {
                var tutorialScenario = di.Instantiate<TutorialScenario>();
                var vrInitializationService = di.Get<IVrInitializerService>();
                vrInitializationService.Initialized += () => tutorialScenario.RunTutorial();
            }
        }

        private void SetupLoop(IEventSender eventSender)
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
                renderControl.OnUpdate();
                foreach (var inputProvider in inputProviders)
                    inputProvider.OnUpdate();
                var frameTime = new FrameTime(Time.realtimeSinceStartup, Time.deltaTime);
                eventRoutingService.FireEvent<INewFrameEvent>(new NewFrameEvent(frameTime));
                // todo: remove this
                renderLoopDispatcher.OnLoop(frameTime);
            };
            eventSender.LateUpdateEvent += () =>
            {
                var frameTime = new FrameTime(Time.realtimeSinceStartup, Time.deltaTime);
                eventRoutingService.FireEvent<ILateUpdateEvent>(new LateUpdateEvent(frameTime));
            };
            eventSender.FixedUpdateEvent += () =>
            {
                eventRoutingService.FireEvent<IFixedUpdateEvent>(new FixedUpdateEvent());
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