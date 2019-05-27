using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Assets.Scripts.Views;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Viewports;
using System;
using System.Linq;
using Assets.Scripts.Interaction;
using Clarity.App.Worlds.Interaction.Queries;
using UnityEngine;
using UObject = UnityEngine.Object;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Assets.Scripts.Gui
{
    public class VrInitializerService : IVrInitializerService
    {
        private readonly IDiContainer di;
        private readonly Lazy<IRenderService> renderServiceLazy;
        private readonly INavigationService navigationService;
        private readonly IStoryService storyService;
        private readonly IGlobalObjectService globalObjectService;
        private readonly IUserQueryService userQueryService;
        private readonly IVrInputDispatcher vrInputDispatcher;
        private readonly IRayHitIndex rayHitIndex;
        private readonly IViewService viewService;
        private readonly Lazy<IGui> guiLazy;

        private GameObject minimap;

        
        public bool IsInitialized { get; private set; }
        public event Action Initialized;

        public VrInitializerService(IDiContainer di, Lazy<IRenderService> renderServiceLazy, INavigationService navigationService,IStoryService storyService, 
            IGlobalObjectService globalObjectService, IUserQueryService userQueryService, IViewService viewService,
            IVrInputDispatcher vrInputDispatcher,
            IRayHitIndex rayHitIndex, Lazy<IGui> guiLazy)
        {
            this.di = di;
            this.renderServiceLazy = renderServiceLazy;
            this.navigationService = navigationService;
            this.storyService = storyService;
            this.globalObjectService = globalObjectService;
            this.userQueryService = userQueryService;
            this.vrInputDispatcher = vrInputDispatcher;
            this.rayHitIndex = rayHitIndex;
            this.viewService = viewService;
            this.guiLazy = guiLazy;

            IsInitialized = false;
            var vrSwitcher = globalObjectService.EventObject.GetComponent<VrSwitcher>();
            vrSwitcher.VrInitialized += Init;
        }

        public void Init()
        {
            if (IsInitialized)
                return;
            CleanUpNonVr(out var cameraPosition, out var cameraRotation);
            InitView();
            InitPlayer(cameraPosition, cameraRotation);
            InitUI();
            InitManipulation();
            InitDispatcher();
            IsInitialized = true;
            Initialized?.Invoke();
        }

        private void CleanUpNonVr(out Vector3 cameraPosition, out Quaternion cameraRotation)
        {
            var camera = viewService.MainView.Layers.First().Camera;
            var cameraFrame = camera.GetGlobalFrame();
            cameraPosition = cameraFrame.Eye.ToUnity();
            cameraRotation = cameraFrame.GetRotation().ToUnity();
            UObject.Destroy(globalObjectService.MainCamera.gameObject);
        }

        private void InitUI()
        {
            globalObjectService.VrGuiCanvas = GameObject.Find("VrGuiCanvas");
            globalObjectService.UICarrier.transform.SetParent(globalObjectService.MainCamera.transform, false);
        }

        // todo: switch out of VR mode

        private void InitView()
        {
            var presentationView = new VrPresentationView(storyService, navigationService, globalObjectService, userQueryService, vrInputDispatcher);
            presentationView.FocusOn(navigationService.Current.GetComponent<IFocusNodeComponent>());
            var viewport = AmFactory.Create<Viewport>();
            viewport.Width = 128;
            viewport.Height = 128;
            viewport.View = presentationView;
            guiLazy.Value.RenderControl.SetViewports(
                new[] { viewport },
                new ViewportsLayout
                {
                    RowHeights = new[] { new ViewportLength(100, ViewportLengthUnit.Percent) },
                    ColumnWidths = new[] { new ViewportLength(100, ViewportLengthUnit.Percent) },
                    ViewportIndices = new[,] { { 0 } }
                });
        }

        private void InitPlayer(Vector3 cameraPosition, Quaternion cameraRotation)
        {
            var player = GameObject.Find("Player") ?? (GameObject)GameObject.Instantiate(Resources.Load("Player", typeof(GameObject)));
            player.name = "Player";

            player.transform.position = cameraPosition;
            player.transform.rotation = cameraRotation;
            // There are two cameras on the player prefab
            globalObjectService.VrPlayerCarrier = new GameObject();
            globalObjectService.VrPlayerCarrier.transform.position = player.transform.position;
            globalObjectService.VrPlayerCarrier.transform.rotation = player.transform.rotation;
            globalObjectService.VrPlayer = player;
            player.transform.SetParent(globalObjectService.VrPlayerCarrier.transform, true);
            globalObjectService.MainCamera = player.EnumerateDeep().SelectMany(x => x.GetComponents<Camera>()).Where(x => x.name == "VRCamera").Single();

            var layoutInstance = storyService.RootLayoutInstance;
            if (!layoutInstance.AllowsFreeCamera)
                return;
            var collisionMesh = layoutInstance.GetCollisionMesh();
            var currentWalkableArea = collisionMesh.GetWalkableAreas().Where(x => x.Contains(cameraPosition.ToClarity())).FirstOrNull();
            if (currentWalkableArea.HasValue)
                // todo: fix when walkable areas are fixed
                globalObjectService.VrPlayerCarrier.transform.position = new Vector3(cameraPosition.x, currentWalkableArea.Value.Center.Y, cameraPosition.z);
        }

        private void InitDispatcher()
        {
            vrInputDispatcher.Initialize();
        }

        private void InitManipulation()
        {
            globalObjectService.VrRightHand = GameObject.Find("RightHand");
            globalObjectService.VrLeftHand = GameObject.Find("LeftHand");
        }
    }
}