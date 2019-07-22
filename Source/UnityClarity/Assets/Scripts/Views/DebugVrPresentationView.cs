using Assets.Scripts.Infra;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction.Queries;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views
{
    public class DebugVrPresentationView : IFocusableView
    {
        private readonly INavigationService navigationService;
        private readonly IStoryService storyService;
        private readonly IUserQueryService userQueryService;
        private readonly IAppModeService appModeService;

        public IReadOnlyList<IViewLayer> Layers { get; }
        private readonly ViewLayer mainLayer;
        private readonly GameObject uiCarrier;
        private readonly GameObject uiCanvas;

        public ISceneNode FocusNode { get; private set; }

        private bool hasFreeCamera;
        private bool hasWarpCamera;

        private const float TransitionDuration = 0.5f;

        public DebugVrPresentationView(INavigationService navigationService, IStoryService storyService,
            IUserQueryService userQueryService, ICommonNodeFactory commonNodeFactory,
            IAppModeService appModeService, IGlobalObjectService globalObjectService)
        {
            this.navigationService = navigationService;
            this.storyService = storyService;
            this.userQueryService = userQueryService;
            this.appModeService = appModeService;
            uiCarrier = globalObjectService.UICarrier;
            uiCanvas = uiCarrier.EnumerateChildren().Single();
            var canvasTransform = uiCanvas.GetComponent<RectTransform>();
            canvasTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            mainLayer = new ViewLayer();
            Layers = new[] { mainLayer };

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
            //clean old query ui
            foreach (Transform child in uiCanvas.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            //fetch new query
            var currentQuery = userQueryService.Queries.LastOrDefault() as OptionsUserQuery;
            if (currentQuery == null || !currentQuery.Options.Any())
                return;

            //generate new query ui
            var uiCanvasSize = uiCanvas.GetComponent<RectTransform>().sizeDelta;
            float OptionMargin = 0.1f * uiCanvasSize.y;
            float OptionHalfHeight = 0.07f * uiCanvasSize.y / 2;
            float OptionHalfWidth = 0.9f * uiCanvasSize.x / 2;
            var optionRects = new List<Rect>();
            var currentY = 1f * uiCanvasSize.y / 2;
            for (var i = 0; i < currentQuery.Options.Count; i++)
            {
                currentY -= OptionMargin;
                currentY -= OptionHalfHeight;
                optionRects.Add(new Rect(0, currentY, OptionHalfWidth, OptionHalfHeight));
                currentY -= OptionHalfHeight;
                currentY -= OptionMargin;
            }
            for (int i = 0; i < currentQuery.Options.Count; i++)
            {
                var optionId = i;
                var option = currentQuery.Options[i];
                var rect = optionRects[i];
                var optionUi = (GameObject)GameObject.Instantiate(Resources.Load("Button", typeof(GameObject)), uiCanvas.transform);
                var optionTrans = optionUi.GetComponent<RectTransform>();
                optionTrans.sizeDelta = new Vector2(rect.width, rect.height);
                optionTrans.anchoredPosition = rect.position;
                optionUi.EnumerateChildren().Single().GetComponent<Text>().text = option;
                optionUi.GetComponent<Button>().onClick.AddListener(() => currentQuery.Choose(optionId));
            }
        }

        private async void OnReachedFork()
        {
            var index = await userQueryService.QueryOptions(navigationService.ForkOptions);
            navigationService.GoForOption(index);
        }
    }
}
