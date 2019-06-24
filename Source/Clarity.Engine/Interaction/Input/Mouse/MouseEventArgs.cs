using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Interaction.Input.Mouse
{
    public class MouseEventArgs : RoutedEventBase, IMouseEventArgs
    {
        public MouseEventType ComplexEventType { get; set; }
        public MouseButtons EventButtons { get; set; }
        public IMouseState State { get; set; }
        public IntVector2 Delta { get; set; }
        public int WheelDelta { get; set; }
        public KeyModifyers KeyModifyers { get; set; }
        public IViewport Viewport { get; set; }
    }
}