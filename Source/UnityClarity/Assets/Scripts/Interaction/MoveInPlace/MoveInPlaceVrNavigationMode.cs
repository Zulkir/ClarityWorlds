using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Infra;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.Interaction.MoveInPlace
{
    public class MoveInPlaceVrNavigationMode : IVrNavigationMode
    {
        private readonly IStoryService storyService;
        private readonly IGlobalObjectService globalObjectService;
        private readonly ICoroutineService coroutineService;

        private event Action<Transform> LeftButtonPressed;
        private event Action<Transform> LeftButtonReleased;
        private event Action<Transform> RightButtonPressed;
        private event Action<Transform> RightButtonReleased;

        [Header("Control Settings")]

        [Tooltip("If this is checked then the left controller engage button will be enabled to move the play area.")]
        private bool leftController = true;
        [Tooltip("If this is checked then the right controller engage button will be enabled to move the play area.")]
        private bool rightController = true;
        [Tooltip("The device to determine the movement paramters from.")]
        private ControlOptions controlOptions = ControlOptions.ControllersOnly;
        [Tooltip("The method in which to determine the direction of forward movement.")]
        private DirectionalMethod directionMethod = DirectionalMethod.Gaze;

        [Header("Speed Settings")]

        [Tooltip("The speed in which to move the play area.")]
        private float speedScale = 2;
        [Tooltip("The maximun speed in game units. (If 0 or less, max speed is uncapped)")]
        private float maxSpeed = 8;
        [Tooltip("The speed in which the play area slows down to a complete stop when the engage button is released. This deceleration effect can ease any motion sickness that may be suffered.")]
        private float deceleration = 0.1f;
        [Tooltip("The speed in which the play area slows down to a complete stop when falling is occuring.")]
        private float fallingDeceleration = 0.01f;

        [Header("Advanced Settings")]

        [Tooltip("The degree threshold that all tracked objects (controllers, headset) must be within to change direction when using the Smart Decoupling Direction Method.")]
        private float smartDecoupleThreshold = 30f;
        // The cap before we stop adding the delta to the movement list. This will help regulate speed.
        [Tooltip("The maximum amount of movement required to register in the virtual world.  Decreasing this will increase acceleration, and vice versa.")]
        private float sensitivity = 0.02f;

        [Header("Custom Settings")]

        [Tooltip("An optional Body Physics script to check for potential collisions in the moving direction. If any potential collision is found then the move will not take place. This can help reduce collision tunnelling.")]
        private VRTK_BodyPhysics bodyPhysics;

        private readonly List<Transform> trackedObjects = new List<Transform>();
        private readonly Dictionary<Transform, List<float>> movementList = new Dictionary<Transform, List<float>>();
        private readonly Dictionary<Transform, float> previousYPositions = new Dictionary<Transform, float>();

        private Transform playArea;
        private GameObject controllerLeftHand;
        private GameObject controllerRightHand;
        private Transform engagedController;
        private Transform headset;
        private bool leftSubscribed;
        private bool rightSubscribed;
        private bool previousLeftControllerState;
        private bool previousRightControllerState;
        private bool currentlyFalling;

        private int averagePeriod;
        private Vector3 initialGaze;
        private float currentSpeed;
        private Vector3 currentDirection;
        private Vector3 previousDirection;
        private bool movementEngaged;

        public MoveInPlaceVrNavigationMode(IStoryService storyService, IGlobalObjectService globalObjectService, ICoroutineService coroutineService)
        {
            this.storyService = storyService;
            this.globalObjectService = globalObjectService;
            this.coroutineService = coroutineService;
        }

        public string UserFriendlyName => "Ski";
        public bool IsEnabled { get; private set; }

        public void Initialize()
        {
        }

        public void SetEnabled(bool enable)
        {
            if (enable)
                Enable();
            else
                Disable();
        }

        private void Enable()
        {
            trackedObjects.Clear();
            movementList.Clear();
            previousYPositions.Clear();
            initialGaze = Vector3.zero;
            currentDirection = Vector3.zero;
            previousDirection = Vector3.zero;
            averagePeriod = 60;
            currentSpeed = 0f;
            movementEngaged = false;

            if (storyService.RootLayoutInstance.AllowsFreeCamera)
            {
                var collisionMesh = storyService.RootLayoutInstance.GetCollisionMesh();
                bodyPhysics = new VRTK_BodyPhysics(collisionMesh, 0.5f);
            }
            else
            {
                bodyPhysics = null;
            }

            controllerLeftHand = globalObjectService.VrLeftHand;
            controllerRightHand = globalObjectService.VrRightHand;

            SetControllerListeners(controllerLeftHand, leftController, ref leftSubscribed);
            SetControllerListeners(controllerRightHand, rightController, ref rightSubscribed);

            headset = globalObjectService.MainCamera.transform;;

            SetControlOptions(controlOptions);

            playArea = globalObjectService.VrPlayerCarrier.transform;

            // Initialize the lists.
            foreach (var trackedObj in trackedObjects)
            {
                VRTK_SharedMethods.AddDictionaryValue(movementList, trackedObj, new List<float>(), true);
                VRTK_SharedMethods.AddDictionaryValue(previousYPositions, trackedObj, trackedObj.transform.localPosition.y, true);
            }

            IsEnabled = true;
        }

        private void Disable()
        {
            SetControllerListeners(controllerLeftHand, leftController, ref leftSubscribed, true);
            SetControllerListeners(controllerRightHand, rightController, ref rightSubscribed, true);

            controllerLeftHand = null;
            controllerRightHand = null;
            headset = null;
            playArea = null;

            IsEnabled = false;
        }

        public void Update(FrameTime frameTime)
        {
            if (!IsEnabled)
                return;

            CheckControllerState(controllerLeftHand, leftController, ref leftSubscribed, ref previousLeftControllerState);
            CheckControllerState(controllerRightHand, rightController, ref rightSubscribed, ref previousRightControllerState);

            if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.LeftHand))
                LeftButtonPressed?.Invoke(controllerLeftHand.transform);
            if (SteamVR_Actions._default.GrabGrip.GetStateUp(SteamVR_Input_Sources.LeftHand))
                LeftButtonReleased?.Invoke(controllerLeftHand.transform);
            if (SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
                RightButtonPressed?.Invoke(controllerRightHand.transform);
            if (SteamVR_Actions._default.GrabGrip.GetStateUp(SteamVR_Input_Sources.RightHand))
                RightButtonReleased?.Invoke(controllerRightHand.transform);
        }

        public void NavigateTo(ISceneNode node, IStoryPath storyPath, CameraProps initialCameraProps, float? initialFloorHeight, float? targetFloorHeight)
        {
            FixedTeleportVrNavigationMode.TeleportTo(globalObjectService.VrPlayerCarrier, node, targetFloorHeight);
        }

        /// <summary>
        /// Set the control options and modify the trackables to match.
        /// </summary>
        /// <param name="givenControlOptions">The control options to set the current control options to.</param>
        private void SetControlOptions(ControlOptions givenControlOptions)
        {
            controlOptions = givenControlOptions;
            trackedObjects.Clear();

            if (controllerLeftHand != null && controllerRightHand != null && (controlOptions == ControlOptions.HeadsetAndControllers || controlOptions == ControlOptions.ControllersOnly))
            {
                VRTK_SharedMethods.AddListValue(trackedObjects, VRTK_DeviceFinder.GetActualController(controllerLeftHand).transform, true);
                VRTK_SharedMethods.AddListValue(trackedObjects, VRTK_DeviceFinder.GetActualController(controllerRightHand).transform, true);
            }

            if (headset != null && (controlOptions == ControlOptions.HeadsetAndControllers || controlOptions == ControlOptions.HeadsetOnly))
            {
                VRTK_SharedMethods.AddListValue(trackedObjects, headset.transform, true);
            }
        }

        public void FixedUpdate()
        {
            if (!IsEnabled)
                return;

            HandleFalling();
            // If Move In Place is currently engaged.
            if (movementEngaged && !currentlyFalling)
            {
                // Initialize the list average.
                var speed = Mathf.Clamp(((speedScale * 350) * (CalculateListAverage() / trackedObjects.Count)), 0f, maxSpeed);
                previousDirection = currentDirection;
                currentDirection = SetDirection();
                // Update our current speed.
                currentSpeed = speed;
            }
            else if (currentSpeed > 0f)
            {
                currentSpeed -= (currentlyFalling ? fallingDeceleration : deceleration);
            }
            else
            {
                currentSpeed = 0f;
                currentDirection = Vector3.zero;
                previousDirection = Vector3.zero;
            }

            SetDeltaTransformData();
            MovePlayArea(currentDirection, currentSpeed);
        }

        private void CheckControllerState(GameObject controller, bool controllerState, ref bool subscribedState, ref bool previousState)
        {
            if (controllerState != previousState)
            {
                SetControllerListeners(controller, controllerState, ref subscribedState);
            }
            previousState = controllerState;
        }

        private float CalculateListAverage()
        {
            float listAverage = 0;

            foreach (var trackedObj in trackedObjects)
            {
                // Get the amount of Y movement that's occured since the last update.
                var previousYPosition = VRTK_SharedMethods.GetDictionaryValue(previousYPositions, trackedObj);
                var deltaYPostion = Mathf.Abs(previousYPosition - trackedObj.transform.localPosition.y);

                // Convenience code.
                var trackedObjList = VRTK_SharedMethods.GetDictionaryValue(movementList, trackedObj, new List<float>(), true);

                // Cap off the speed.
                VRTK_SharedMethods.AddListValue(trackedObjList,
                    deltaYPostion > sensitivity ? sensitivity : deltaYPostion);

                // Keep our tracking list at m_averagePeriod number of elements.
                if (trackedObjList.Count > averagePeriod)
                    trackedObjList.RemoveAt(0);

                // Average out the current tracker's list.
                var sum = trackedObjList.Sum();
                var avg = sum / averagePeriod;

                // Add the average to the the list average.
                listAverage += avg;
            }

            return listAverage;
        }

        private Vector3 GetHeadsetDirection()
        {
            return (headset != null ? new Vector3(headset.forward.x, 0, headset.forward.z) : Vector3.zero);
        }

        private Vector3 SetDirection()
        {
            switch (directionMethod)
            {
                case DirectionalMethod.SmartDecoupling:
                case DirectionalMethod.DumbDecoupling:
                    return CalculateCouplingDirection();
                case DirectionalMethod.ControllerRotation:
                    return CalculateControllerRotationDirection(DetermineAverageControllerRotation() * Vector3.forward);
                case DirectionalMethod.LeftControllerRotationOnly:
                    return CalculateControllerRotationDirection((controllerLeftHand != null ? controllerLeftHand.transform.rotation : Quaternion.identity) * Vector3.forward);
                case DirectionalMethod.RightControllerRotationOnly:
                    return CalculateControllerRotationDirection((controllerRightHand != null ? controllerRightHand.transform.rotation : Quaternion.identity) * Vector3.forward);
                case DirectionalMethod.EngageControllerRotationOnly:
                    return CalculateControllerRotationDirection((engagedController != null ? engagedController.rotation : Quaternion.identity) * Vector3.forward);
                case DirectionalMethod.Gaze:
                    return GetHeadsetDirection();
                default:
                    return Vector2.zero;
            }
        }

        private Vector3 CalculateCouplingDirection()
        {
            // If we haven't set an inital gaze yet, set it now.
            // If we're doing dumb decoupling, this is what we'll be sticking with.
            if (initialGaze == Vector3.zero)
            {
                initialGaze = GetHeadsetDirection();
            }

            // If we're doing smart decoupling, check to see if we want to reset our distance.
            if (directionMethod != DirectionalMethod.SmartDecoupling)
                return initialGaze;
            
            var curXDir = (headset != null ? headset.rotation.eulerAngles.y : 0f);
            if (curXDir <= smartDecoupleThreshold)
                curXDir += 360;

            var closeEnough = (Mathf.Abs(curXDir - controllerLeftHand.transform.rotation.eulerAngles.y) <= smartDecoupleThreshold) && 
                              (Mathf.Abs(curXDir - controllerRightHand.transform.rotation.eulerAngles.y) <= smartDecoupleThreshold);

            // If the controllers and the headset are pointing the same direction (within the threshold) reset the direction the player's moving.
            if (closeEnough)
                initialGaze = GetHeadsetDirection();
            return initialGaze;
        }

        private Vector3 CalculateControllerRotationDirection(Vector3 calculatedControllerDirection)
        {
            return (Vector3.Angle(previousDirection, calculatedControllerDirection) <= 90f ? calculatedControllerDirection : previousDirection);
        }

        private void SetDeltaTransformData()
        {
            foreach (var trackedObj in trackedObjects)
            {
                // Get delta postions and rotations
                VRTK_SharedMethods.AddDictionaryValue(previousYPositions, trackedObj, trackedObj.transform.localPosition.y, true);
            }
        }

        private void MovePlayArea(Vector3 moveDirection, float moveSpeed)
        {
            var movement = (moveDirection * moveSpeed) * Time.fixedDeltaTime;
            if (playArea == null)
                return;
            var finalPosition = new Vector3(movement.x + playArea.position.x, playArea.position.y, movement.z + playArea.position.z);
            if (CanMove(bodyPhysics, playArea.position, finalPosition))
                playArea.position = finalPosition;
        }

        private static bool CanMove(VRTK_BodyPhysics givenBodyPhysics, Vector3 currentPosition, Vector3 proposedPosition)
        {
            if (givenBodyPhysics == null)
                return true;

            var proposedDirection = (proposedPosition - currentPosition).normalized;
            var distance = Vector3.Distance(currentPosition, proposedPosition);
            return !givenBodyPhysics.SweepCollision(currentPosition, proposedDirection, distance);
        }

        private void HandleFalling()
        {
            if (bodyPhysics != null && bodyPhysics.IsFalling())
                currentlyFalling = true;

            if (bodyPhysics != null && !bodyPhysics.IsFalling() && currentlyFalling)
            {
                currentlyFalling = false;
                currentSpeed = 0f;
            }
        }

        private void EngageButtonPressed(Transform controller)
        {
            engagedController = controller;
            movementEngaged = true;
        }

        private void EngageButtonReleased(Transform controller)
        {
            // If the button is released, clear all the lists.
            foreach (var trackedObj in trackedObjects)
                VRTK_SharedMethods.GetDictionaryValue(movementList, trackedObj, new List<float>()).Clear();
            initialGaze = Vector3.zero;

            movementEngaged = false;
            engagedController = null;
        }

        private Quaternion DetermineAverageControllerRotation()
        {
            // Build the average rotation of the controller(s)
            Quaternion newRotation;

            // Both controllers are present
            if (controllerLeftHand != null && controllerRightHand != null)
                newRotation = AverageRotation(controllerLeftHand.transform.rotation,
                    controllerRightHand.transform.rotation);
            else if (controllerLeftHand != null && controllerRightHand == null)
                newRotation = controllerLeftHand.transform.rotation;
            else if (controllerRightHand != null && controllerLeftHand == null)
                newRotation = controllerRightHand.transform.rotation;
            else
                newRotation = Quaternion.identity;
            return newRotation;
        }

        // Returns the average of two Quaternions
        private static Quaternion AverageRotation(Quaternion rot1, Quaternion rot2)
        {
            return Quaternion.Slerp(rot1, rot2, 0.5f);
        }

        private void SetControllerListeners(GameObject controller, bool controllerState, ref bool subscribedState, bool forceDisabled = false)
        {
            if (controller == null)
                return;
            var enable = !forceDisabled && controllerState;
            ToggleControllerListeners(controller, enable, ref subscribedState);
        }

        private void ToggleControllerListeners(GameObject controller, bool enable, ref bool subscribed)
        {
            if (enable && !subscribed)
            {
                if (controller == controllerLeftHand)
                {
                    LeftButtonPressed += EngageButtonPressed;
                    LeftButtonReleased += EngageButtonReleased;
                }
                else
                {
                    RightButtonPressed += EngageButtonPressed;
                    RightButtonReleased += EngageButtonReleased;
                }
                subscribed = true;
            }
            else if (!enable && subscribed)
            {
                if (controller == controllerLeftHand)
                {
                    LeftButtonPressed -= EngageButtonPressed;
                    LeftButtonReleased -= EngageButtonReleased;
                }
                else
                {
                    RightButtonPressed -= EngageButtonPressed;
                    RightButtonReleased -= EngageButtonReleased;
                }
                subscribed = false;
            }
        }

        public async void ShowHints(float seconds)
        {
            var rightHand = globalObjectService.VrRightHand.GetComponentInChildren<Hand>();
            var leftHand = globalObjectService.VrLeftHand.GetComponentInChildren<Hand>();
            ControllerButtonHints.ShowTextHint(rightHand, SteamVR_Actions.default_GrabGrip, "Hold and move up and down");
            ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_GrabGrip, "Hold and move up and down");
            await coroutineService.WaitSeconds(seconds);
            HideHints();
        }

        public void HideHints()
        {
            var rightHand = globalObjectService.VrRightHand.GetComponentInChildren<Hand>();
            var leftHand = globalObjectService.VrLeftHand.GetComponentInChildren<Hand>();
            ControllerButtonHints.HideTextHint(rightHand, SteamVR_Actions.default_GrabGrip);
            ControllerButtonHints.HideTextHint(leftHand, SteamVR_Actions.default_GrabGrip);
        }
    }
}