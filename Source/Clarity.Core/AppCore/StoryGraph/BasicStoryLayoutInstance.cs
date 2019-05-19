using System;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Core.AppCore.StoryGraph
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