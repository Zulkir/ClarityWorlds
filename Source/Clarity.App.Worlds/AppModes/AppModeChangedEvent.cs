using Clarity.Engine.EventRouting;

namespace Clarity.App.Worlds.AppModes
{
    public class AppModeChangedEvent : RoutedEventBase, IAppModeChangedEvent
    {
        public AppMode NewAppMode { get; }

        public AppModeChangedEvent(AppMode newAppMode)
        {
            NewAppMode = newAppMode;
        }
    }
}