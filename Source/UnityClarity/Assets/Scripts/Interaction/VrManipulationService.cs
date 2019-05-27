using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Clarity.App.Worlds.Interaction.Manipulation3D;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree.MiscComponents;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Transform = Clarity.Common.Numericals.Algebra.Transform;

namespace Assets.Scripts.Interaction
{
    public class VrManipulationService : IVrManipulationService
    {
        private class GripInfo
        {
            public ISceneNode Node { get; }
            public Transform GripTransform { get; }

            public GripInfo(ISceneNode node, Transform gripTransform)
            {
                Node = node;
                GripTransform = gripTransform;
            }
        }

        private readonly IViewService viewService;
        private readonly IGlobalObjectService globalObjectService;
        [CanBeNull] private GripInfo rightGrippedNode;
        [CanBeNull] private GripInfo leftGrippedNode;

        public VrManipulationService(IEventRoutingService eventRoutingService, IViewService viewService, IGlobalObjectService globalObjectService)
        {
            this.viewService = viewService;
            this.globalObjectService = globalObjectService;
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(IVrManipulationService), nameof(OnNewFrame), OnNewFrame);
            eventRoutingService.RegisterServiceDependency(typeof(IRenderService), typeof(IVrManipulationService));
        }

        public IEnumerable<ISceneNode> GetGrabbedObjects()
        {
            if (rightGrippedNode != null)
                yield return rightGrippedNode.Node;
            if (leftGrippedNode != null)
                yield return leftGrippedNode.Node;
        }

        private void OnNewFrame(INewFrameEvent evnt)
        {
            if (!XRSettings.enabled)
                return;

            var rightHand = globalObjectService.VrRightHand;
            var leftHand = globalObjectService.VrLeftHand;

            var scene = viewService.RenderControl.Viewports.Single().View.Layers.First().VisibleScene;
            ProcessHand(scene, rightHand, SteamVR_Input_Sources.RightHand, ref rightGrippedNode);
            ProcessHand(scene, leftHand, SteamVR_Input_Sources.LeftHand, ref leftGrippedNode);
        }

        //todo: Fix left hand preference
        private static void ProcessHand(IScene scene, GameObject hand, SteamVR_Input_Sources inputSource, ref GripInfo gripInfo)
        {
            if (hand == null)
                return;

            var handTransform = new Transform(1, hand.transform.rotation.ToClarity(true), hand.transform.position.ToClarity());

            if (SteamVR_Actions.default_GrabGrip.GetStateDown(inputSource))
            {
                var handPosition = hand.transform.position.ToClarity();
                var node = scene
                    .EnumerateAllNodes()
                    .Where(x => x.HasComponent<ManipulateInPresentationComponent>())
                    .Select(x => x.SearchComponent<ITransformable3DComponent>())
                    .Where(x => x != null)
                    .FirstOrDefault(x => (x.LocalBoundingSphere * x.Node.GlobalTransform).Contains(handPosition))
                    ?.Node;
                if (node != null)
                {
                    var gripTransform = node.GlobalTransform * handTransform.Invert();
                    gripInfo = new GripInfo(node, gripTransform);
                }
            }

            if (!SteamVR_Actions.default_GrabGrip.GetState(inputSource))
            {
                gripInfo = null;
                return;
            }

            if (gripInfo == null)
                return;

            var newGlobalTransform = gripInfo.GripTransform * handTransform;
            gripInfo.Node.Transform = newGlobalTransform *
                (gripInfo.Node.ParentNode?.GlobalTransform.Invert() ?? Transform.Identity);
        }
    }
}