using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;

namespace Clarity.App.Worlds.Views.Cameras
{
    public class FreeLandDefaultViewpointMechanism : IDefaultViewpointMechanism
    {
        private readonly ISceneNode node;
        private readonly IKeyboardInputProvider keyboardInputProvider;
        private readonly FreeControlledCamera.Props props;
        public ICamera FixedCamera { get; }

        public FreeLandDefaultViewpointMechanism(IKeyboardInputProvider keyboardInputProvider, ISceneNode node)
            : this(node, FreeControlledCamera.Props.Default, keyboardInputProvider)
        {
        }

        public FreeLandDefaultViewpointMechanism(ISceneNode node, FreeControlledCamera.Props props, IKeyboardInputProvider keyboardInputProvider)
        {
            this.props = props;
            this.keyboardInputProvider = keyboardInputProvider;
            this.node = node;
            FixedCamera = CreateControlledViewpoint();
        }

        public ICamera CreateControlledViewpoint()
        {
            return new FreeControlledCamera(node, props, keyboardInputProvider);
        }
    }
}