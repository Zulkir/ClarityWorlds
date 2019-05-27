using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.StoryLayout.Building
{
    public class BuildingStoryLayoutInstance : IStoryLayoutInstance
    {
        private readonly IInputService inputService;
        private readonly BuildingStoryLayoutPlacementAlgorithm placementAlgorithm;
        private readonly IStoryGraph sg;
        private readonly ICollisionMesh collisionMesh;
        private readonly IStoryLayoutZoning zoning;


        public BuildingStoryLayoutInstance(IInputService inputService, BuildingStoryLayoutPlacementAlgorithm placementAlgorithm, 
            ICollisionMesh collisionMesh, IStoryLayoutZoning zoning)
        {
            this.placementAlgorithm = placementAlgorithm;
            this.collisionMesh = collisionMesh;
            this.zoning = zoning;
            this.inputService = inputService;
            sg = placementAlgorithm.StoryGraph;
        }

        public IStoryPath GetPath(CameraProps initialCameraProps, int endNodeId, NavigationState navigationState, bool interLevel)
        {
            return new BuildingStoryPath(placementAlgorithm, sg, initialCameraProps, endNodeId, navigationState, interLevel);
        }

        public int GetClosestNodeId(CameraProps cameraProps)
        {
            return sg.Leaves.Minimal(x => (placementAlgorithm.GetGlobalTransform(x).Offset - cameraProps.Target).LengthSquared());
        }

        public bool AllowsFreeCamera => true;

        public bool AllowsWarpCamera => true;

        public ICollisionMesh GetCollisionMesh()
        {
            return collisionMesh;
        }

        public IControlledCamera CreateFreeCamera(CameraProps initialCameraProps)
        {
            return new BuildingFreeCamera(initialCameraProps, inputService, collisionMesh, zoning);
        }

        public IControlledCamera CreateWarpCamera(CameraProps initialCameraProps)
        {
            return new BuildingFreeCamera(initialCameraProps, inputService, collisionMesh, zoning);
            //return new BuildingWarpCamera(initialCameraProps, collisionMesh, placementAlgorithm, inputService);
        }
    }
}