using System;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.App.Worlds.StoryGraph
{
    public class BasicStoryLayoutInstance : IStoryLayoutInstance
    {
        private readonly IWorldTreeService worldTreeService;

        public const float PathDefaultDuration = 1f;

        public BasicStoryLayoutInstance(IWorldTreeService worldTreeService)
        {
            this.worldTreeService = worldTreeService;
        }

        public IStoryPath GetPath(CameraProps initialCameraProps, int endNodeId, NavigationState navigationState, bool interLevel)
        {
            var endNode = worldTreeService.GetById(endNodeId);
            var aEndFocus = endNode.GetComponent<IFocusNodeComponent>();
            var endCameraInfo = aEndFocus.DefaultViewpointMechanism.FixedCamera.GetProps();
            return new DirectStoryPath(initialCameraProps, endCameraInfo, PathDefaultDuration);
        }

        public bool AllowsFreeCamera => false;

        public int GetClosestNodeId(CameraProps cameraProps)
        {
            // todo: implement
            throw new NotImplementedException();
        }

        public IControlledCamera CreateFreeCamera(CameraProps initialCameraProps)
        {
            throw new NotSupportedException();
        }
    }
}