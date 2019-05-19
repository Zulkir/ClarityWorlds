namespace Clarity.Engine.Interaction.Input.Mouse
{
    public static class MouseEventArgsExtensions
    {
        public static bool IsOfType(this IMouseEventArgs eventArgs, MouseEventType type)
        {
            if (eventArgs.ComplexEventType == type)
                return true;
            switch (eventArgs.ComplexEventType)
            {
                case MouseEventType.Click:
                    return type == MouseEventType.Click || type == MouseEventType.Up;
                case MouseEventType.DoubleClick:
                    return type == MouseEventType.DoubleClick || type == MouseEventType.Click || type == MouseEventType.Up;
            }
            return false;
        }

        public static bool IsLeftDownEvent(this IMouseEventArgs eventArgs) =>
            eventArgs.IsOfType(MouseEventType.Down) && eventArgs.EventButtons == MouseButtons.Left;

        public static bool IsLeftUpEvent(this IMouseEventArgs eventArgs) =>
            eventArgs.IsOfType(MouseEventType.Up) && eventArgs.EventButtons == MouseButtons.Left;

        public static bool IsLeftClickEvent(this IMouseEventArgs eventArgs) =>
            eventArgs.IsOfType(MouseEventType.Click) && eventArgs.EventButtons == MouseButtons.Left;

        public static bool IsLeftDoubleClickEvent(this IMouseEventArgs eventArgs) =>
            eventArgs.IsOfType(MouseEventType.DoubleClick) && eventArgs.EventButtons == MouseButtons.Left;

        public static bool IsRightClickEvent(this IMouseEventArgs eventArgs) =>
            eventArgs.IsOfType(MouseEventType.Click) && eventArgs.EventButtons == MouseButtons.Right;

        public static bool IsClickEvent(this IMouseEventArgs eventArgs) => 
            eventArgs.IsLeftClickEvent() || eventArgs.IsRightClickEvent();
    }
}