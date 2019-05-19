using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;

namespace Clarity.Core.AppCore.WorldTree
{
    public class SphereDefaultViewpointMechanism : IDefaultViewpointMechanism
    {
        private readonly ISceneNode node;
        private readonly LookAroundCamera.Props props;
        public ICamera FixedCamera { get; }

        public SphereDefaultViewpointMechanism(ISceneNode node, LookAroundCamera.Props props)
        {
            this.node = node;
            this.props = props;
            FixedCamera = CreateControlledViewpoint();
        }

        public ICamera CreateControlledViewpoint()
        {
            return new LookAroundCamera(node, props);
        }
    }
}