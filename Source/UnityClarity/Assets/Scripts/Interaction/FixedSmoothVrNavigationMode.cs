using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using Valve.VR;

namespace Assets.Scripts.Interaction
{
    public class FixedSmoothVrNavigationMode : IVrNavigationMode
    {
        private readonly IGlobalObjectService globalObjectService;
        private readonly IInputService inputService;
        private ICamera currentPathCamera;
        private float floorHeightOffset;
        private bool hasFinished;

        private static readonly bool[] NoKeys = new bool[100];

        public string UserFriendlyName => "Fixed Smooth";
        public bool IsEnabled { get; private set; }

        public FixedSmoothVrNavigationMode(IGlobalObjectService globalObjectService, IInputService inputService)
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
            if (enable)
            {
                currentPathCamera = null;
                hasFinished = true;
            }
        }

        public void Update(FrameTime frameTime)
        {
            if (!IsEnabled)
                return;
            if (currentPathCamera == null)
            {
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
            else
            {
                currentPathCamera.Update(frameTime);

                var player = globalObjectService.VrPlayerCarrier;
                var frame = currentPathCamera.GetGlobalFrame();
                var eye = frame.Eye;
                eye.Y -= floorHeightOffset;
                player.transform.position = eye.ToUnity();
                //player.transform.rotation = newProps.Frame.GetRotation().ToUnity();

                if (hasFinished)
                {
                    //player.transform.rotation = frame.GetRotation().ToUnity();
                    currentPathCamera = null;
                }
            }
        }

        public void NavigateTo(ISceneNode node, IStoryPath storyPath, CameraProps initialCameraProps, float? initialFloorHeight, float? targetFloorHeight)
        {
            var cFocusNode = node.GetComponent<IFocusNodeComponent>();
            var newCamera = cFocusNode.DefaultViewpointMechanism.CreateControlledViewpoint();
            if (initialFloorHeight.HasValue)
                initialCameraProps.Frame.Eye.Y = initialFloorHeight.Value + 2;
            currentPathCamera = new StoryPathCamera(storyPath, initialCameraProps, newCamera.GetProps(), () => hasFinished = true);
            floorHeightOffset = initialFloorHeight.HasValue 
                ? 2//initialCameraProps.Frame.Eye.Y - floorHeight.Value 
                : 0;
            hasFinished = false;
        }

        public void FixedUpdate()
        {
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
