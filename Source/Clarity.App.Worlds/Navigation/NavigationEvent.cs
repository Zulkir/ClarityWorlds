using Clarity.Engine.EventRouting;

namespace Clarity.App.Worlds.Navigation
{
    public class NavigationEvent : RoutedEventBase, INavigationEvent
    {
        public NavigationEventType Type { get; }
        public bool MoveInstantly { get; }
        public bool CausedByFocusing { get; }

        public NavigationEvent(NavigationEventType type, bool moveInstantly, bool causedByFocusing)
        {
            Type = type;
            MoveInstantly = moveInstantly;
            CausedByFocusing = causedByFocusing;
        }
    }
}