using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;

namespace Clarity.App.Worlds.Views.Cameras
{
    public class WallDefaultViewpointMechanismZ : IDefaultViewpointMechanism
    {
        private readonly ISceneNode node;
        private readonly TargetedControlledCamera.Props props;
        public ICamera FixedCamera { get; }

        public WallDefaultViewpointMechanismZ(ISceneNode node)
            : this(node, TargetedControlledCamera.Props.Default)
        {
        }

        public WallDefaultViewpointMechanismZ(ISceneNode node, TargetedControlledCamera.Props props)
        {
            this.node = node;
            this.props = props;
            FixedCamera = CreateControlledViewpoint();
        }

        public ICamera CreateControlledViewpoint()
        {
            return new TargetedControlledCamera(node, props);
        }
    }
}