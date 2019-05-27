using UnityEngine;
using UnityEngine.XR;
using Clarity.App.Worlds.StoryGraph;
using Assets.Scripts.Infra;
using Assets.Scripts.Helpers;
using ca = Clarity.Common.Numericals.Algebra;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;

namespace Assets.Scripts.Gui
{
    public class VrHeadPositionService : IVrHeadPositionService
    {
        private readonly IStoryService storyService;
        private readonly IGlobalObjectService globalObjectService;
        private bool hintsShown;

        private ca.Transform localPlayerTransform;

        public VrHeadPositionService(IStoryService storyService, IGlobalObjectService globalObjectService, IEventRoutingService eventRoutingService)
        {
            this.storyService = storyService;
            this.globalObjectService = globalObjectService;
            localPlayerTransform = ca.Transform.Identity;
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(IVrHeadPositionService), nameof(OnNewFrame), OnNewFrame);
        }

        private void OnNewFrame(INewFrameEvent evnt)
        {
            if (!XRSettings.enabled)
                return;
            CatchUpWithPlayer();
            UpdateElevatorHints();
        }

        public void ResetHeadPosition()
        {
            CatchUpWithPlayer();

            var cameraGlobalTransform = globalObjectService.MainCamera.transform.ToClarity();
            var playerGlobalTransform = globalObjectService.VrPlayer.transform.ToClarity();
            var cameraLocalTransform = cameraGlobalTransform * playerGlobalTransform.Invert();
            var cameraLocalRotationY = cameraLocalTransform.Rotation.ToUnity().eulerAngles.y;
            cameraLocalTransform.Rotation = Quaternion.Euler(0, cameraLocalRotationY, 0).ToClarity();
            cameraLocalTransform.Offset.Y = 0;
            localPlayerTransform = cameraLocalTransform.Invert();

            globalObjectService.VrPlayer.transform.localRotation = localPlayerTransform.Rotation.ToUnity();
            globalObjectService.VrPlayer.transform.localPosition = localPlayerTransform.Offset.ToUnity();
        }

        public void UpdateElevatorHints()
        {
            var leftHand = globalObjectService.VrLeftHand.GetComponentInChildren<Hand>();
            if (!TryGetCollisionMesh(out var collisionMesh))
                return;
            if (collisionMesh.Zoning.GetZonePropertiesAt(globalObjectService.MainCamera.transform.position.ToClarity()).Gravity == 0f)
            {
                if (!hintsShown)
                {
                    ControllerButtonHints.ShowTextHint(leftHand, SteamVR_Actions.default_ElevatorUp, "Move between floors");
                    hintsShown = true;
                }
            }
            else
            {
                if (hintsShown)
                {
                    ControllerButtonHints.HideTextHint(leftHand, SteamVR_Actions.default_ElevatorUp);
                    hintsShown = false;
                }
            }
        }

        public void RotateRight(float degrees)
        {
            CatchUpWithPlayer();
            localPlayerTransform *= ca.Transform.Rotate(ca.Quaternion.RotationY(Mathf.Deg2Rad * degrees));
            globalObjectService.VrPlayer.transform.localPosition = localPlayerTransform.Offset.ToUnity();
            globalObjectService.VrPlayer.transform.localRotation = localPlayerTransform.Rotation.ToUnity();
        }

        public void TryUseElevator(bool up)
        {
            CatchUpWithPlayer();
            if (!TryGetCollisionMesh(out var collisionMesh))
                return;
            var zoning = collisionMesh.Zoning;
            if (zoning.GetZonePropertiesAt(globalObjectService.MainCamera.transform.position.ToClarity()).Gravity != 0f)
                return;
            var sign = up ? 1 : -1;
            globalObjectService.VrPlayerCarrier.transform.position += new Vector3(0, sign * collisionMesh.ZeroGravityTeleportHeight, 0);
            if (globalObjectService.VrPlayerCarrier.transform.position.y > -collisionMesh.ZeroGravityTeleportHeight / 2)
            {
                SteamVR_Fade.Start(Color.white, 0);
                SteamVR_Fade.Start(Color.clear, 0.5f);
            }
            if (globalObjectService.VrPlayerCarrier.transform.position.y < 0)
                globalObjectService.VrPlayerCarrier.transform.position -= new Vector3(0, globalObjectService.VrPlayerCarrier.transform.position.y, 0);
            var intY = globalObjectService.VrPlayerCarrier.transform.position.y / collisionMesh.ZeroGravityTeleportHeight;
            var roundedY = Mathf.Round(intY);
            var y = roundedY * collisionMesh.ZeroGravityTeleportHeight;
            globalObjectService.VrPlayerCarrier.transform.position = globalObjectService.VrPlayerCarrier.transform.position.With((ref Vector3 v) => v.y = y);
        }

        private bool TryGetCollisionMesh(out ICollisionMesh collisionMesh)
        {
            var layoutInstance = storyService.RootLayoutInstance;
            if (!layoutInstance.AllowsFreeCamera)
            {
                collisionMesh = null;
                return false;
            }
            collisionMesh = layoutInstance.GetCollisionMesh();
            return true;
        }

        private void CatchUpWithPlayer()
        {
            var carrierGlobalTransform = globalObjectService.VrPlayerCarrier.transform.ToClarity();
            var globalPlayerTransform = globalObjectService.VrPlayer.transform.ToClarity();
            var newGlobalCarrierTransform = localPlayerTransform.Invert() * globalPlayerTransform;
            globalObjectService.VrPlayerCarrier.transform.SetPositionAndRotation(newGlobalCarrierTransform.Offset.ToUnity(), newGlobalCarrierTransform.Rotation.ToUnity());
            globalObjectService.VrPlayer.transform.localRotation = localPlayerTransform.Rotation.ToUnity();
            globalObjectService.VrPlayer.transform.localPosition = localPlayerTransform.Offset.ToUnity();
        }
    }
}
