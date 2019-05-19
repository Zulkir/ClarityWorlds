namespace Clarity.Core.AppCore.Views
{
    public class ViewEventArgs
    {
        public ViewEventType Type { get; }

        public ViewEventArgs(ViewEventType type)
        {
            Type = type;
        }
    }
}