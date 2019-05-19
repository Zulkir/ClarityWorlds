using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;

namespace Clarity.Core.AppCore.WorldTree
{
    public class WallDefaultViewpointMechanism : IDefaultViewpointMechanism
    {
        private readonly ISceneNode node;
        private readonly TargetedControlledCameraY.Props props;
        public ICamera FixedCamera { get; }

        public WallDefaultViewpointMechanism(ISceneNode node)
            : this(node, TargetedControlledCameraY.Props.Default)
        {
        }

        public WallDefaultViewpointMechanism(ISceneNode node, TargetedControlledCameraY.Props props)
        {
            this.node = node;
            this.props = props;
            FixedCamera = CreateControlledViewpoint();
        }

        public ICamera CreateControlledViewpoint()
        {
            return new TargetedControlledCameraY(node, props);
        }
    }
}