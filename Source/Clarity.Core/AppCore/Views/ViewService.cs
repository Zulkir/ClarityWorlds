using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Core.AppCore.Interaction;
using Clarity.Core.AppCore.Interaction.Queries;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Views;

namespace Clarity.Core.AppCore.Views
{
    // todo: refactor as AmObject
    public class ViewService : IViewService
    {
        // todo: refactor (make model public and get rid of property passthrough?)

        private readonly ViewServiceModel model;
        private readonly IWorldTreeService worldTreeService;
        
        public IFocusableView MainView { get; private set; }

        public IRenderGuiControl RenderControl => model.RenderControl;

        public event EventHandler<ViewEventArgs> Update;

        public ViewService(IWorldTreeService worldTreeService, IRenderLoopDispatcher renderLoopDispatcher, INavigationService navigationService, IUserQueryService userQueryService)
        {
            this.worldTreeService = worldTreeService;
            model = AmFactory.Create<ViewServiceModel>();
            model.Updated += OnModelUpdated;
            worldTreeService.UpdatedMed += OnWorldUpdated;
            renderLoopDispatcher.Update += OnUpdate;
            navigationService.Updated += OnNavigationServiceUpdated;
            userQueryService.Updated += OnUserQueryServiceUpdated;
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

        private void OnModelUpdated(IAmEventMessage message)
        {
            //Update?.Invoke(...);
        }

        private void OnUpdate(FrameTime frameTime)
        {
            if (model.RenderControl == null)
                return;
            foreach (var viewport in model.RenderControl.Viewports)
                viewport.View.Update(frameTime);
        }

        private void OnWorldUpdated(IAmEventMessage message)
        {
            // todo: check that the focused node actually still exists
            foreach (var view in EnumerateViews().OfType<IFocusableView>())
                view.OnWorldUpdated(message);
        }

        public void ChangeRenderingArea(IRenderGuiControl newRenderControl, IFocusableView mainView)
        {
            model.RenderControl = newRenderControl;
            MainView = mainView;
            // todo: move somewhere or remove
            Update?.Invoke(this, new ViewEventArgs(ViewEventType.ViewportChanged));
        }

        private IEnumerable<IView> EnumerateViews()
        {
            return RenderControl?.Viewports.Select(x => x.View) ?? Enumerable.Empty<IView>();
        }

        private void OnNavigationServiceUpdated(INavigationEventArgs args)
        {
            foreach (var view in EnumerateViews().OfType<IFocusableView>())
                view.OnNavigationEvent(args);
        }

        private void OnUserQueryServiceUpdated()
        {
            foreach (var view in EnumerateViews().OfType<IFocusableView>())
                view.OnQueryServiceUpdated();
        }
    }
}