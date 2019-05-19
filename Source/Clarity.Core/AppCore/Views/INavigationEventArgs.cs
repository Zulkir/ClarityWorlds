namespace Clarity.Core.AppCore.Views 
{
    public interface INavigationEventArgs
    {
        NavigationEventType Type { get; }
        bool MoveInstantly { get; }
        bool CausedByFocusing { get; }
    }
}