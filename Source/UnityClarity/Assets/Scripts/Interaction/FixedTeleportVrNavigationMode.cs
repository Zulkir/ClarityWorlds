using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using UnityEngine;
using Valve.VR;

namespace Assets.Scripts.Interaction
{
    public class FixedTeleportVrNavigationMode : IVrNavigationMode
    {
        private readonly IGlobalObjectService globalObjectService;
        private readonly IInputService inputService;

        private static readonly bool[] NoKeys = new bool[100];

        public string UserFriendlyName => "Fixed Teleport";
        public bool IsEnabled { get; private set; }

        public FixedTeleportVrNavigationMode(IGlobalObjectService globalObjectService, IInputService inputService)
        {
            this.globalObjectService = globalObjectService;
            this.inputService = inputService;
        }

        public void Initialize()
        {
        }

        public void SetEnabled(bool enable)
        {
            IsEnabled = enable;
        }

        public void Update(FrameTime frameTime)
        {
            if (!IsEnabled)
                return;

            if (SteamVR_Actions.default_NavigateNext.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                inputService.OnInputEvent(new KeyEventArgs
                {
                    ComplexEventType = KeyEventType.Down,
                    KeyModifyers = KeyModifyers.None,
                    EventKey = Key.Right,
                    HasFocus = true,
                    State = new KeyboardState(NoKeys),
                    Viewport = null
                });
            }

            if (SteamVR_Actions.default_NavigatePrev.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                inputService.OnInputEvent(new KeyEventArgs
                {
                    ComplexEventType = KeyEventType.Down,
                    KeyModifyers = KeyModifyers.None,
                    EventKey = Key.Left,
                    HasFocus = true,
                    State = new KeyboardState(NoKeys),
                    Viewport = null
                });
            }
        }

        public void FixedUpdate()
        {
        }

        public void NavigateTo(ISceneNode node, IStoryPath storyPath, CameraProps initialCameraProps, float? initialFloorHeight, float? targetFloorHeight)
        {
            TeleportTo(globalObjectService.VrPlayerCarrier, node, targetFloorHeight);
        }

        public static void TeleportTo(GameObject vrPlayerCarrier, ISceneNode node, float? targetFloorHeight)
        {
            var cFocusNode = node.GetComponent<IFocusNodeComponent>();
            var newCamera = cFocusNode.DefaultViewpointMechanism.CreateControlledViewpoint();
            var newProps = newCamera.GetProps();
            var eye = newProps.Frame.Eye;
            if (targetFloorHeight.HasValue)
                eye.Y = targetFloorHeight.Value;
            SteamVR_Fade.Start(Color.white, 0);
            vrPlayerCarrier.transform.position = eye.ToUnity();
            vrPlayerCarrier.transform.rotation = newProps.Frame.GetRotation().ToUnity();
            SteamVR_Fade.Start(Color.clear, 0.5f);
        }

        public void ShowHints(float seconds)
        {
            // TODO
        }

        public void HideHints()
        {
            // TODO
        }
    }
}