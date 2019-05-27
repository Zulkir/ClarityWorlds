using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Interaction.Input.VRController
{
    public class VRControllerEventArgs : IVRControllerEventArgs
    {
        public VRControllerEventType EventType { get; set; }
        public VRControllerButtons EventButtons { get; set; }
        public Vector3 Delta { get; set; }
        public IVRControllerState State { get; set; }
        public IViewport Viewport { get; set; }
    }
}
