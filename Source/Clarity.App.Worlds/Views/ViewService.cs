using System;
using System.Linq;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.App.Worlds.Views
{
    // todo: get rid of ViewService completely
    // todo: refactor as AmObject
    public class ViewService : IViewService
    {
        // todo: refactor (make model public and get rid of property passthrough?)

        private IStoryService storyService;

        private readonly ViewServiceModel model;

        public ISceneNode ClosestStoryNode { get; private set; }
        public IFocusableView MainView { get; private set; }

        public IRenderGuiControl RenderControl => model.RenderControl;

        public event EventHandler<ViewEventArgs> Update;

        public ViewService(IEventRoutingService eventRoutingService, IRenderLoopDispatcher renderLoopDispatcher, IStoryService storyService)
        {
            this.storyService = storyService;
            model = AmFactory.Create<ViewServiceModel>();
            eventRoutingService.RegisterServiceDependency(typeof(IViewService), typeof(IWorldTreeService));
            eventRoutingService.Subscribe<IAppModeChangedEvent>(typeof(IViewService), nameof(OnAppModeChange), OnAppModeChange);
            renderLoopDispatcher.Update += OnUpdate;
        }

        public ISceneNode SelectedNode
        {
            get { return model.SelectedNode; }
            set
            {
                foreach (var interactionComponent in model.SelectedNode?.SearchComponents<IInteractionComponent>() ?? Enumerable.Empty<IInteractionComponent>())
                    interactionComponent.TryHandleInteractionEvent(CoreInterationEvent.Deselected());
                model.SelectedNode = value;
                foreach (var interactionComponent in model.SelectedNode?.SearchComponents<IInteractionComponent>() ?? Enumerable.Empty<IInteractionComponent>())
                    interactionComponent.TryHandleInteractionEvent(CoreInterationEvent.Selected());
                Update?.Invoke(this, new ViewEventArgs(ViewEventType.SelectedNodeChanged));
            }
        }

        private void OnUpdate(FrameTime frameTime)
        {
            if (model.RenderControl == null)
                return;
            foreach (var viewport in model.RenderControl.Viewports)
                viewport.View.Update(frameTime);

            var mainLayer = MainView.Layers.FirstOrDefault();
            if (mainLayer == null)
                return;
            var mainCamera = mainLayer.Camera;
            var mainScene = mainLayer.VisibleScene;
            var closestStoryNodeId = storyService.RootLayoutInstance.GetClosestNodeId(mainCamera.GetProps());
            ClosestStoryNode = storyService.GlobalGraph.NodeObjects[closestStoryNodeId];
        }

        private void OnAppModeChange(IAppModeChangedEvent evnt)
        {
            SelectedNode = null;
        }

        public void ChangeRenderingArea(IRenderGuiControl newRenderControl, IFocusableView mainView)
        {
            model.RenderControl = newRenderControl;
            MainView = mainView;
            // todo: move somewhere or remove
            Update?.Invoke(this, new ViewEventArgs(ViewEventType.ViewportChanged));
        }
    }
}