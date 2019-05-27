using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;

namespace Clarity.App.Worlds.Views.Cameras
{
    public class OrthoDefaultViewpointMechanism : IDefaultViewpointMechanism
    {
        private readonly ISceneNode node;
        private readonly PlaneOrthoBoundControlledCamera.Props props;
        public ICamera FixedCamera { get; }

        public OrthoDefaultViewpointMechanism(ISceneNode node)
            : this(node, PlaneOrthoBoundControlledCamera.Props.Default)
        {
        }

        public OrthoDefaultViewpointMechanism(ISceneNode node, PlaneOrthoBoundControlledCamera.Props props)
        {
            this.node = node;
            this.props = props;
            FixedCamera = CreateControlledViewpoint();
        }

        public ICamera CreateControlledViewpoint()
        {
            return new PlaneOrthoBoundControlledCamera(node, props, true);
        }
    }
}