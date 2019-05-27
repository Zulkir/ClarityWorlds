using Clarity.Engine.EventRouting;

namespace Clarity.App.Worlds.Navigation 
{
    public interface INavigationEvent : IRoutedEvent
    {
        NavigationEventType Type { get; }
        bool MoveInstantly { get; }
        bool CausedByFocusing { get; }
    }
}