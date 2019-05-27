using System;
using System.Collections.Generic;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction.Queries;
using Clarity.App.Worlds.Interaction.Queries.Scene;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.App.Worlds.Views 
{
    public abstract class PresentationView : AmObjectBase<PresentationView, IViewport>, IFocusableView
    {
        private readonly INavigationService navigationService;
        private readonly IStoryService storyService;
        private readonly IUserQueryService userQueryService;
        private readonly IAppModeService appModeService;
        
        public IReadOnlyList<IViewLayer> Layers { get; }
        private readonly ViewLayer mainLayer;
        private readonly ViewLayer userQueryLayer;

        public ISceneNode FocusNode { get; private set; }
        private bool hasFreeCamera;
        private bool hasWarpCamera;

        private readonly UserQuerySceneComponent querySceneComponent;

        private const float TransitionDuration = 0.5f;

        protected PresentationView(INavigationService navigationService, IStoryService storyService, 
            IUserQueryService userQueryService, ICommonNodeFactory commonNodeFactory, 
            IAppModeService appModeService)
        {
            this.navigationService = navigationService;
            this.storyService = storyService;
            this.userQueryService = userQueryService;
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
            if (!(args is IKeyEventArgs kargs)) 
                return false;
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
                        hasWarpCamera = false;
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
            if (kargs.ComplexEventType == KeyEventType.Down && kargs.EventKey == Key.K)
            {
                var layoutInstance = storyService.RootLayoutInstance;
                var cameraProps = mainLayer.Camera.GetProps();
                if (!hasWarpCamera)
                {
                    if (layoutInstance.AllowsFreeCamera)
                    {
                        mainLayer.Camera = layoutInstance.CreateWarpCamera(cameraProps);
                        hasWarpCamera = true;
                        hasFreeCamera = false;
                        return true;
                    }
                }
                else
                {
                    var closestNode = layoutInstance.GetClosestNodeId(cameraProps);
                    navigationService.GoToSpecific(closestNode);
                    FocusOn(storyService.GlobalGraph.NodeObjects[closestNode].GetComponent<IFocusNodeComponent>());
                    hasWarpCamera = false;
                }
            }

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

        public void OnEveryEvent(IRoutedEvent evnt)
        {
            switch (evnt)
            {
                case INavigationEvent navigationEvent:
                    OnNavigationEvent(navigationEvent);
                    break;
                case IUserQueryEvent userQueryEvent:
                    OnQueryServiceUpdated();
                    break;
            }
        }

        private void OnNavigationEvent(INavigationEvent args)
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

        private void OnQueryServiceUpdated()
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