using System;
using System.Linq;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.Interaction.Queries;
using Clarity.App.Worlds.Navigation;
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

        private readonly ViewServiceModel model;
        
        public IFocusableView MainView { get; private set; }

        public IRenderGuiControl RenderControl => model.RenderControl;

        public event EventHandler<ViewEventArgs> Update;

        public ViewService(IEventRoutingService eventRoutingService, IRenderLoopDispatcher renderLoopDispatcher, INavigationService navigationService, IUserQueryService userQueryService)
        {
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
                    interactionComponent.TryHandleInteractionEvent(CoreInterationEventArgs.Deselected());
                model.SelectedNode = value;
                foreach (var interactionComponent in model.SelectedNode?.SearchComponents<IInteractionComponent>() ?? Enumerable.Empty<IInteractionComponent>())
                    interactionComponent.TryHandleInteractionEvent(CoreInterationEventArgs.Selected());
                Update?.Invoke(this, new ViewEventArgs(ViewEventType.SelectedNodeChanged));
            }
        }

        private void OnUpdate(FrameTime frameTime)
        {
            if (model.RenderControl == null)
                return;
            foreach (var viewport in model.RenderControl.Viewports)
                viewport.View.Update(frameTime);
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