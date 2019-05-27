using System;
using System.Linq;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.App.Worlds.StoryGraph
{
    public class BasicStoryLayoutInstance : IStoryLayoutInstance
    {
        private readonly IStoryGraph storyGraph;

        public const float PathDefaultDuration = 1f;

        public BasicStoryLayoutInstance(IStoryGraph storyGraph)
        {
            this.storyGraph = storyGraph;
        }

        public IStoryPath GetPath(CameraProps initialCameraProps, int endNodeId, NavigationState navigationState, bool interLevel)
        {
            var endNode = storyGraph.NodeObjects[storyGraph.Root].Scene.World.GetNodeById(endNodeId);
            var aEndFocus = endNode.GetComponent<IFocusNodeComponent>();
            var endCameraInfo = aEndFocus.DefaultViewpointMechanism.FixedCamera.GetProps();
            return new DirectStoryPath(initialCameraProps, endCameraInfo, PathDefaultDuration);
        }

        public bool AllowsFreeCamera => false;

        public bool AllowsWarpCamera => false;

        public int GetClosestNodeId(CameraProps cameraProps)
        {
            return storyGraph.Leaves
                .Select(x => storyGraph.NodeObjects[x])
                .Minimal(x => (x.GlobalTransform.Offset - cameraProps.Frame.Eye).LengthSquared())
                .Id;
        }

        public ICollisionMesh GetCollisionMesh() => throw new NotSupportedException();
        public IControlledCamera CreateFreeCamera(CameraProps initialCameraProps) => throw new NotSupportedException();
        public IControlledCamera CreateWarpCamera(CameraProps initialCameraProps) => throw new NotSupportedException();
    }
}