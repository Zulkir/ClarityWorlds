using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;

namespace Clarity.App.Worlds.Views.Cameras
{
    public class LandDefaultViewpointMechanism : IDefaultViewpointMechanism
    {
        private readonly ISceneNode node;
        private TargetedControlledCamera.Props props;

        public TargetedControlledCamera.Props Props
        {
            get { return props; }
            set
            {
                props = value;
                FixedCamera = CreateControlledViewpoint();
            }
        }

        public ICamera FixedCamera { get; private set; }
        
        public LandDefaultViewpointMechanism(ISceneNode node)
        {
            this.node = node;
            Props = TargetedControlledCamera.Props.Default;
        }

        public ICamera CreateControlledViewpoint()
        {
            return new TargetedControlledCamera(node, Props);
        }
    }
}