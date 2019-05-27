using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using Valve.VR;
using Valve.VR.InteractionSystem;
using ca = Clarity.Common.Numericals.Algebra;

namespace Assets.Scripts.Interaction
{
    public class FreeSmoothVrNavigationMode : IVrNavigationMode
    {
        private readonly IStoryService storyService;
        private readonly IGlobalObjectService globalObjectService;
        private readonly ICoroutineService coroutineService;
        private ICollisionMesh collisionMesh;

        private const float Speed = 6f;

        public string UserFriendlyName => "Free Smooth";
        public bool IsEnabled { get; private set; }

        public FreeSmoothVrNavigationMode(IStoryService storyService, IGlobalObjectService globalObjectService, ICoroutineService coroutineService)
        {
            this.storyService = storyService;
            this.globalObjectService = globalObjectService;
            this.coroutineService = coroutineService;
        }

        public void Initialize()
        {
            var layoutInstance = storyService.RootLayoutInstance;
            if (!layoutInstance.AllowsFreeCamera)
                return;
            collisionMesh = layoutInstance.GetCollisionMesh();
        }

        public void SetEnabled(bool enable)
        {
            IsEnabled = enable;
        }

        public void Update(FrameTime frameTime)
        {
            if (!IsEnabled)
                return;
            var camera = globalObjectService.MainCamera;
            var projectedForward = new Plane(ca.Vector3.UnitY, ca.Vector3.Zero).Project(camera.transform.forward.ToClarity());
            if (projectedForward.LengthSquared() < MathHelper.Eps8)
                return;
            var dpadOffset = SteamVR_Actions.default_DpadOrientation.GetAxis(SteamVR_Input_Sources.RightHand);
            var forward = projectedForward.Normalize();
            var right = ca.Vector3.Cross(forward, ca.Vector3.UnitY);
            var controllerMovement = forward * dpadOffset.y + right * dpadOffset.x;
            var cameraHeightOffset = 1f;
            var startingPoint = globalObjectService.VrPlayerCarrier.transform.position.ToClarity() + cameraHeightOffset * ca.Vector3.UnitY;
            var offset = controllerMovement * Speed * frameTime.DeltaSeconds;
            var unrestrictedNewPosition = startingPoint + offset;
            var restrictedNewPosition = collisionMesh.RestrictMovement(0.5f, startingPoint, offset);
            restrictedNewPosition.Y = unrestrictedNewPosition.Y - cameraHeightOffset;
            globalObjectService.VrPlayerCarrier.transform.position = restrictedNewPosition.ToUnity();
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
            ControllerButtonHints.ShowTextHint(rightHand, SteamVR_Actions.default_DpadOrientation, "Tilt to move");
            await coroutineService.WaitSeconds(seconds);
            HideHints();
        }

        public void HideHints()
        {
            var rightHand = globalObjectService.VrRightHand.GetComponentInChildren<Hand>();
            ControllerButtonHints.HideTextHint(rightHand, SteamVR_Actions.default_DpadOrientation);
        }
    }
}
