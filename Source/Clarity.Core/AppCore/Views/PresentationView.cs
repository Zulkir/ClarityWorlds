using System;
using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Core.AppCore.AppModes;
using Clarity.Core.AppCore.Interaction.Queries;
using Clarity.Core.AppCore.Interaction.Queries.Scene;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.Core.AppCore.Views 
{
    public abstract class PresentationView : AmObjectBase<PresentationView, IViewport>, IFocusableView
    {
        private readonly INavigationService navigationService;
        private readonly IStoryService storyService;
        private readonly IUserQueryService userQueryService;
        private readonly IRayHitIndex rayHitIndex;
        private readonly IViewService viewService;
        private readonly IAppModeService appModeService;
        
        public IReadOnlyList<IViewLayer> Layers { get; }
        private readonly ViewLayer mainLayer;
        private readonly ViewLayer userQueryLayer;

        public ISceneNode FocusNode { get; private set; }
        private bool hasFreeCamera;

        private readonly UserQuerySceneComponent querySceneComponent;

        private const float TransitionDuration = 0.5f;

        protected PresentationView(INavigationService navigationService, IStoryService storyService, 
            IUserQueryService userQueryService, ICommonNodeFactory commonNodeFactory, IRayHitIndex rayHitIndex, 
            IViewService viewService, IAppModeService appModeService)
        {
            this.navigationService = navigationService;
            this.storyService = storyService;
            this.userQueryService = userQueryService;
            this.rayHitIndex = rayHitIndex;
            this.viewService = viewService;
            this.appModeService = appModeService;

            mainLayer = new ViewLayer();
            userQueryLayer = new ViewLayer();
            Layers = new[] {mainLayer, userQueryLayer};

            querySceneComponent = AmFactory.Create<UserQuerySceneComponent>();
            var uqSceneRoot = commonNodeFactory.WorldRoot(false);
            uqSceneRoot.Components.Add(querySceneComponent);
            var uqScene = Scene.Create(uqSceneRoot);
            var uqCamera = new PlaneOrthoBoundControlledCamera(uqSceneRoot, PlaneOrthoBoundControlledCamera.Props.Default, false);
            userQueryLayer.VisibleScene = uqScene;
            userQueryLayer.Camera = uqCamera;
        }

        public void Update(FrameTime frameTime)
        {
            mainLayer.Camera.Update(frameTime);
        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            if (args is IMouseEventArgs margs)
            {
                if (TryHandleMouseByLayer(margs, userQueryLayer))
                    return true;
                if (TryHandleMouseByLayer(margs, mainLayer))
                    return true;
            }

            if (args is IKeyEventArgs kargs)
            {
                if (kargs.ComplexEventType == KeyEventType.Down && kargs.EventKey == Key.Escape)
                {
                    appModeService.SetMode(AppMode.Editing);
                    return true;
                }

                if (kargs.ComplexEventType == KeyEventType.Down && kargs.EventKey == Key.F)
                {
                    var layoutInstance = storyService.RootLayoutInstance;
                    var cameraProps = mainLayer.Camera.GetProps();
                    if (!hasFreeCamera)
                    {
                        if (layoutInstance.AllowsFreeCamera)
                        {
                            
                            mainLayer.Camera = layoutInstance.CreateFreeCamera(cameraProps);
                            hasFreeCamera = true;
                            return true;
                        }
                    }
                    else
                    {
                        var closestNode = layoutInstance.GetClosestNodeId(cameraProps);
                        navigationService.GoToSpecific(closestNode);
                        FocusOn(storyService.GlobalGraph.NodeObjects[closestNode].GetComponent<IFocusNodeComponent>());
                        hasFreeCamera = false;
                    }
                }
                var selected = viewService.SelectedNode;
                if (selected != null)
                    foreach (var interactionComponent in selected.Node.SearchComponents<IInteractionComponent>())
                        if (interactionComponent.TryHandleInteractionEvent(kargs))
                            return true;
            }

            if (TryHandleInputByCamera(args, userQueryLayer.Camera))
                return true;
            if (TryHandleInputByCamera(args, mainLayer.Camera))
                return true;
            
            return false;
        }

        private bool TryHandleMouseByLayer(IMouseEventArgs margs, IViewLayer layer)
        {
            var clickInfo = new RayHitInfo(margs.Viewport, layer, margs.State.Position);
            var hitResult = rayHitIndex.FindEntity(clickInfo);
            if (hitResult.Successful)
            {
                foreach (var interactionComponent in hitResult.Node.SearchComponents<IInteractionComponent>())
                    if (interactionComponent.TryHandleInteractionEvent(margs))
                        return true;
            }
            else  if (margs.IsClickEvent())
                viewService.SelectedNode = null;

            return false;
        }

        private static bool TryHandleInputByCamera(IInputEventArgs args, ICamera camera)
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

            if (mainLayer.VisibleScene == null)
            {
                mainLayer.VisibleScene = newNode.Scene;
            }

            FocusNode = aFocusNode.Node;

            navigationService.OnFocus(aFocusNode.Node.Id);
        }

        public void OnWorldUpdated(IAmEventMessage message) {  }

        public void OnNavigationEvent(INavigationEventArgs args)
        {
            if (hasFreeCamera || args.CausedByFocusing)
                return;

            var aFocusNode = navigationService.Current.GetComponent<IFocusNodeComponent>();
            var newNode = aFocusNode.Node;
            var newCamera = aFocusNode.DefaultViewpointMechanism.CreateControlledViewpoint();
            if (mainLayer.Camera == null || newNode.Scene != FocusNode.Scene)
                mainLayer.Camera = newCamera;

            // todo: find real one
            var layoutInstance = storyService.RootLayoutInstance;
            var initialCameraProps = mainLayer.Camera.GetProps();
            var interLevel = navigationService.InterLevelTransition;
            var path = layoutInstance.GetPath(initialCameraProps, navigationService.Current.Id, navigationService.State, interLevel);

            switch (args.Type)
            {
                case NavigationEventType.Reset:
                    if (mainLayer.Camera == null)
                        mainLayer.Camera = newCamera;
                    else
                        mainLayer.Camera = new SceneTransitionCamera(mainLayer.Camera, newCamera, TransitionDuration,
                            () => mainLayer.VisibleScene = newNode.Scene, () => mainLayer.Camera = newCamera);
                    break;
                case NavigationEventType.MoveToNextFork:
                    mainLayer.Camera = args.MoveInstantly 
                        ? newCamera 
                        : new StoryPathCamera(path, initialCameraProps, null, OnReachedFork);
                    break;
                case NavigationEventType.MoveToPrevFork:
                    mainLayer.Camera = args.MoveInstantly 
                        ? newCamera 
                        : new StoryPathCamera(path, initialCameraProps, null, OnReachedFork);
                    break;
                case NavigationEventType.MoveToSpecific:
                    mainLayer.Camera = args.MoveInstantly 
                        ? newCamera 
                        : new StoryPathCamera(path, initialCameraProps, newCamera.GetProps(), () => { mainLayer.Camera = newCamera; });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(args.Type), args.Type, null);
            }
        }

        public void OnQueryServiceUpdated()
        {
            querySceneComponent.OnQueryServiceUpdated();
        }

        private async void OnReachedFork()
        {
            var index = await userQueryService.QueryOptions(navigationService.ForkOptions);
            navigationService.GoForOption(index);
        }
    }
}