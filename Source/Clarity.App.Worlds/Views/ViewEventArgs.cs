namespace Clarity.App.Worlds.Views
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