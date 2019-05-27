using System.Collections.Generic;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph;
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
        private readonly List<BuildingWallSegment> globalWallSegments;

        public BuildingStoryLayoutInstance(IInputService inputService, BuildingStoryLayoutPlacementAlgorithm placementAlgorithm, List<BuildingWallSegment> globalWallSegments)
        {
            this.placementAlgorithm = placementAlgorithm;
            this.globalWallSegments = globalWallSegments;
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

        public IControlledCamera CreateFreeCamera(CameraProps initialCameraProps)
        {
            return new BuildingFreeCamera(initialCameraProps, inputService, globalWallSegments, placementAlgorithm);
        }
    }
}