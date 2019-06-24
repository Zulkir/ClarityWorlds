using Clarity.Engine.EventRouting;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Interaction.Input.Keyboard
{
    public class KeyEventArgs : RoutedEventBase, IKeyEventArgs
    {
        public KeyEventType ComplexEventType { get; set; }
        public Key EventKey { get; set; }
        public IKeyboardState State { get; set; }
        public KeyModifyers KeyModifyers { get; set; }
        public string Text { get; set; }
        public bool HasFocus { get; set; }
        public IViewport Viewport { get; set; }
    }
}