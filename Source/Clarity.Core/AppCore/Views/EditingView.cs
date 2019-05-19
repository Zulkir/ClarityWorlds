using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.Core.AppCore.Views 
{
    public abstract class EditingView : AmObjectBase<EditingView, IViewport>, IFocusableView
    {
        private readonly IRayHitIndex rayHitIndex;
        private readonly IViewService viewService;
        private readonly ViewLayer mainLayer;
        private readonly INavigationService navigationService;

        public ISceneNode FocusNode { get; private set; }
        public IReadOnlyList<IViewLayer> Layers { get; }

        private const float TransitionDuration = 0.5f;

        protected EditingView(ICommonNodeFactory commonNodeFactory, IRayHitIndex rayHitIndex, IViewService viewService, INavigationService navigationService)
        {
            this.rayHitIndex = rayHitIndex;
            this.viewService = viewService;
            this.navigationService = navigationService;
            FocusNode = commonNodeFactory.WorldRoot(false);
            var scene = Scene.Create(FocusNode);
            mainLayer = new ViewLayer
            {
                VisibleScene = scene,
                Camera = new PlaneOrthoBoundControlledCamera(FocusNode, PlaneOrthoBoundControlledCamera.Props.Default, false)
            };
            Layers = new[] {mainLayer};
        }

        public void Update(FrameTime frameTime)
        {
            mainLayer.Camera.Update(frameTime);
        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            if (args is IMouseEventArgs margs)
            {
                if (TryHandleMouseByLayer(margs, mainLayer))
                    return true;
            }

            if (args is IKeyEventArgs kargs)
            {
                var selected = viewService.SelectedNode;
                if (selected != null)
                    foreach (var interactionElement in selected.Node.SearchComponents<IInteractionComponent>())
                        if (interactionElement.TryHandleInteractionEvent(kargs))
                            return true;
            }
            
            if (TryHandleInoutByCamera(args, mainLayer.Camera))
                return true;
            
            return false;
        }

        private bool TryHandleMouseByLayer(IMouseEventArgs margs, IViewLayer layer)
        {
            //if (margs.ComplexEventType == MouseEventType.Move)
            //    return false;
            var clickInfo = new RayHitInfo(margs.Viewport, layer, margs.State.Position);
            var hitResult = rayHitIndex.FindEntity(clickInfo);
            if (hitResult.Successful)
            {
                foreach (var interactionElement in hitResult.Node.SearchComponents<IInteractionComponent>())
                    if (interactionElement.TryHandleInteractionEvent(margs))
                        return true;
            }
            else if (margs.IsClickEvent())
            {
                viewService.SelectedNode = null;
            }

            return false;
        }

        private static bool TryHandleInoutByCamera(IInputEventArgs args, ICamera camera)
        {
            if (camera is IControlledCamera controlledCamera && controlledCamera.TryHandleInput(args))
                return true;
            return false;
        }

        public void FocusOn(IFocusNodeComponent aFocusNode)
        {
            var newNode = aFocusNode.Node;
            var newCamera = aFocusNode.DefaultViewpointMechanism.CreateControlledViewpoint();

            if (mainLayer.Camera == null)
            {
                mainLayer.Camera = newCamera;
            }
            else if (newNode.Scene != FocusNode.Scene)
            {
                mainLayer.Camera = new SceneTransitionCamera(mainLayer.Camera, newCamera, TransitionDuration, 
                    () => mainLayer.VisibleScene = newNode.Scene, () => mainLayer.Camera = newCamera);
            }
            else
            {
                mainLayer.Camera = new TransitionCamera(mainLayer.Camera, newCamera, TransitionDuration,
                    () => mainLayer.Camera = newCamera);
            }
            FocusNode = aFocusNode.Node;

            navigationService.OnFocus(aFocusNode.Node.Id);
        }

        public void OnWorldUpdated(IAmEventMessage message)
        {
            if (message.Obj<ISceneNode>().ItemRemoved(x => x.ChildNodes, out var remMessage))
            {
                var parent = remMessage.Object;
                var removedRoot = remMessage.Item;
                if (FocusNode.IsDescendantOf(removedRoot))
                    FocusOn(parent.PresentationInfra().ClosestFocusNode.GetComponent<IFocusNodeComponent>());
            }
        }

        public void OnNavigationEvent(INavigationEventArgs args) {  }

        public void OnQueryServiceUpdated() {  }
    }
}