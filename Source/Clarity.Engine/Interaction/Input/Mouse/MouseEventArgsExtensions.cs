namespace Clarity.Engine.Interaction.Input.Mouse
{
    public static class MouseEventArgsExtensions
    {
        public static bool IsOfType(this IMouseEvent eventArgs, MouseEventType type)
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

        public static bool IsLeftDownEvent(this IMouseEvent eventArgs) =>
            eventArgs.IsOfType(MouseEventType.Down) && eventArgs.EventButtons == MouseButtons.Left;

        public static bool IsLeftUpEvent(this IMouseEvent eventArgs) =>
            eventArgs.IsOfType(MouseEventType.Up) && eventArgs.EventButtons == MouseButtons.Left;

        public static bool IsLeftClickEvent(this IMouseEvent eventArgs) =>
            eventArgs.IsOfType(MouseEventType.Click) && eventArgs.EventButtons == MouseButtons.Left;

        public static bool IsLeftDoubleClickEvent(this IMouseEvent eventArgs) =>
            eventArgs.IsOfType(MouseEventType.DoubleClick) && eventArgs.EventButtons == MouseButtons.Left;

        public static bool IsRightClickEvent(this IMouseEvent eventArgs) =>
            eventArgs.IsOfType(MouseEventType.Click) && eventArgs.EventButtons == MouseButtons.Right;

        public static bool IsClickEvent(this IMouseEvent eventArgs) => 
            eventArgs.IsLeftClickEvent() || eventArgs.IsRightClickEvent();
    }
}