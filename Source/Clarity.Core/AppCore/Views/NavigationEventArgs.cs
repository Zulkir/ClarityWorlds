namespace Clarity.Core.AppCore.Views
{
    public class NavigationEventArgs : INavigationEventArgs
    {
        public NavigationEventType Type { get; }
        public bool MoveInstantly { get; }
        public bool CausedByFocusing { get; }

        public NavigationEventArgs(NavigationEventType type, bool moveInstantly, bool causedByFocusing)
        {
            Type = type;
            MoveInstantly = moveInstantly;
            CausedByFocusing = causedByFocusing;
        }
    }
}