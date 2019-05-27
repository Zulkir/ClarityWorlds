using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.Interaction
{
    public class FreeTeleportVrNavigationMode : IVrNavigationMode
    {
        private readonly IGlobalObjectService globalObjectService;
        private readonly IStoryService storyService;
        private readonly ICoroutineService coroutineService;

        private GameObject teleport;

        public string UserFriendlyName => "Jumping";
        public bool IsEnabled { get; private set; }

        public FreeTeleportVrNavigationMode(IStoryService storyService, IGlobalObjectService globalObjectService, ICoroutineService coroutineService)
        {
            this.storyService = storyService;
            this.globalObjectService = globalObjectService;
            this.coroutineService = coroutineService;
        }

        public void Initialize()
        {
            teleport = (GameObject)Object.Instantiate(Resources.Load("Teleporting", typeof(GameObject)));
            teleport.name = "Teleporting";

            var layoutInstance = storyService.RootLayoutInstance;
            if (!layoutInstance.AllowsFreeCamera)
                return;
            var collisionMesh = layoutInstance.GetCollisionMesh();
            foreach (var walkableArea in collisionMesh.GetWalkableAreas())
            {
                var smallOffset = new Vector3(0, 0.1f, 0);
                var tpArea = (GameObject)GameObject.Instantiate(Resources.Load("TeleportArea", typeof(GameObject)));
                tpArea.transform.localPosition = walkableArea.Center.ToUnity() + smallOffset;
                tpArea.transform.localScale = new Vector3(walkableArea.HalfSize.Width * 2 + 8, walkableArea.HalfSize.Depth * 2, 1);
            }
            IsEnabled = false;
        }

        public void SetEnabled(bool enable)
        {
            IsEnabled = enable;
        }

        public void Update(FrameTime frameTime)
        {
            if (teleport.activeInHierarchy != IsEnabled)
                teleport.SetActive(IsEnabled);
        }

        public void NavigateTo(ISceneNode node, IStoryPath storyPath, CameraProps initialCameraProps, float? initialFloorHeight, float? targetFloorHeight)
        {
            FixedTeleportVrNavigationMode.TeleportTo(globalObjectService.VrPlayerCarrier, node, targetFloorHeight);
        }

        public void FixedUpdate()
        {
        }

        public async void ShowHints(float seconds)
        {
            var rightHand = globalObjectService.VrRightHand.GetComponentInChildren<Hand>();
            ControllerButtonHints.ShowTextHint(rightHand, SteamVR_Actions.default_Teleport, "Press and choose new location");
            await coroutineService.WaitSeconds(seconds);
            ControllerButtonHints.HideTextHint(rightHand, SteamVR_Actions.default_Teleport);
        }

        public void HideHints()
        {
            var rightHand = globalObjectService.VrRightHand.GetComponentInChildren<Hand>();
            ControllerButtonHints.HideTextHint(rightHand, SteamVR_Actions.default_Teleport);
        }
    }
}