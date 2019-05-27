using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Infra;
using Assets.Scripts.Interaction;
using Clarity.App.Worlds.Interaction.Queries;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Views;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.Views
{
    public class VrPresentationView : IFocusableView
    {
        private readonly IStoryService storyService;
        private readonly INavigationService navigationService;
        private readonly IUserQueryService userQueryService;
        private readonly IGlobalObjectService globalObjectService;
        private readonly IVrInputDispatcher vrInputDispatcher;
        private readonly VrCamera camera;
        private readonly ViewLayer layer;

        public ISceneNode FocusNode { get; private set; }
        public IReadOnlyList<IViewLayer> Layers { get; }

        public VrPresentationView(IStoryService storyService, INavigationService navigationService, IGlobalObjectService globalObjectService, 
            IUserQueryService userQueryService, IVrInputDispatcher vrInputDispatcher)
        {
            this.storyService = storyService;
            this.navigationService = navigationService;
            this.userQueryService = userQueryService;
            this.globalObjectService = globalObjectService;
            this.vrInputDispatcher = vrInputDispatcher;
            camera = new VrCamera(globalObjectService);
            
            layer = new ViewLayer
            {
                Camera = camera,
                VisibleScene = null
            };
            Layers = new IViewLayer[]
            {
                layer
            };
        }

        public void Update(FrameTime frameTime)
        {
            layer.Camera.Update(frameTime);
        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            return false;
        }

        public void FocusOn(IFocusNodeComponent cFocusNode)
        {
            var newNode = cFocusNode.Node;
            if (layer.VisibleScene == null)
                layer.VisibleScene = newNode.Scene;
            navigationService.OnFocus(newNode.Id);
        }

        public void OnEveryEvent(IRoutedEvent evnt)
        {
            var navigationEvent = evnt as INavigationEvent;
            if (navigationEvent != null)
                OnNavigationEvent(navigationEvent);
            var userQueryEvent = evnt as IUserQueryEvent;
            if (userQueryEvent != null)
                OnQueryServiceUpdated();
        }

        private void OnQueryServiceUpdated()
        {
            var canvasObj = globalObjectService.VrGuiCanvas;

            //clean old query ui
            foreach (Transform child in canvasObj.transform)
                GameObject.Destroy(child.gameObject);

            //fetch new query
            var currentQuery = userQueryService.Queries.LastOrDefault() as OptionsUserQuery;
            if (currentQuery == null || !currentQuery.Options.Any())
                return;

            //generate new query ui
            const float buttonDistance = 0.16f;
            
            var currentY = 0f;
            for (var i = 0; i < currentQuery.Options.Count; i++)
            {
                var iLoc = i;
                var option = currentQuery.Options[i];
                var optionUi = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/VrGuiButton"), canvasObj.transform);
                var rectTransform = optionUi.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, currentY);
                var cText = optionUi.EnumerateChildren().Select(x => x.GetComponent<Text>()).Single(x => x != null);
                cText.text = option;
                optionUi.GetComponent<UIElement>().onHandClick.AddListener(h =>
                {
                    Debug.Log("Clicked: " + option);
                    currentQuery.Choose(iLoc);
                });
                currentY -= buttonDistance;
            }
        }

        private void OnNavigationEvent(INavigationEvent evnt)
        {
            var cFocusNode = navigationService.Current.GetComponent<IFocusNodeComponent>();
            var newCamera = cFocusNode.DefaultViewpointMechanism.CreateControlledViewpoint();

            //todo: refactor this mess
            var initialFloorHeight = (float?)null;
            var targetFloorHeight = (float?)null;
            var cameraPosition = camera.GetEye();
            // todo: find real one
            var layoutInstance = storyService.RootLayoutInstance;
            if (layoutInstance.AllowsFreeCamera)
            {
                var collisionMesh = layoutInstance.GetCollisionMesh();
                var currentWalkableArea = collisionMesh.GetWalkableAreas().Where(x => x.Contains(cameraPosition)).FirstOrNull();
                if (currentWalkableArea.HasValue)
                {
                    // todo: fix when walkable areas are fixed
                    initialFloorHeight = currentWalkableArea.Value.Center.Y;
                }
                var targetWalkableArea = collisionMesh.GetWalkableAreas().Where(x => x.Contains(newCamera.GetEye())).FirstOrNull();
                if (targetWalkableArea.HasValue)
                {
                    // todo: fix when walkable areas are fixed
                    targetFloorHeight = targetWalkableArea.Value.Center.Y;
                }
            }
            
            var initialCameraProps = camera.GetProps();
            initialCameraProps.Frame.Eye.X = globalObjectService.VrPlayerCarrier.transform.position.x;
            initialCameraProps.Frame.Eye.Z = -globalObjectService.VrPlayerCarrier.transform.position.z;
            var interLevel = navigationService.InterLevelTransition;
            var path = layoutInstance.GetPath(initialCameraProps, navigationService.Current.Id, navigationService.State, interLevel);

            switch (evnt.Type)
            {
                case NavigationEventType.Reset:
                case NavigationEventType.MoveToSpecific:
                    vrInputDispatcher.NavigateTo(navigationService.Current, path, initialCameraProps, initialFloorHeight, targetFloorHeight);
                    break;
                case NavigationEventType.MoveToNextFork:
                case NavigationEventType.MoveToPrevFork:
                    OnReachedFork();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async void OnReachedFork()
        {
            var index = await userQueryService.QueryOptions(navigationService.ForkOptions);
            navigationService.GoForOption(index);
        }
    }
}