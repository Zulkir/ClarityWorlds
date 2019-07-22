using System.Collections.Generic;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.App.Worlds.Views 
{
    public abstract class EditingView : AmObjectBase<EditingView, IViewport>, IFocusableView
    {
        private readonly ViewLayer mainLayer;
        private readonly INavigationService navigationService;

        public ISceneNode FocusNode { get; private set; }
        public IReadOnlyList<IViewLayer> Layers { get; }

        private const float TransitionDuration = 0.5f;

        protected EditingView(ICommonNodeFactory commonNodeFactory, INavigationService navigationService)
        {
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

        public bool TryHandleInput(IInputEvent args)
        {
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

        public void OnEveryEvent(IRoutedEvent evnt)
        {
            if (evnt is IWorldTreeUpdatedEvent worldTreeUpdatedEvent)
                OnWorldUpdated(worldTreeUpdatedEvent.AmMessage);
        }

        private void OnWorldUpdated(IAmEventMessage message)
        {
            if (message.Obj<ISceneNode>().ItemRemoved(x => x.ChildNodes, out var remMessage))
            {
                var parent = remMessage.Object;
                var removedRoot = remMessage.Item;
                if (FocusNode.IsDescendantOf(removedRoot))
                    FocusOn(parent.PresentationInfra().ClosestFocusNode.GetComponent<IFocusNodeComponent>());
            }
        }
    }
}