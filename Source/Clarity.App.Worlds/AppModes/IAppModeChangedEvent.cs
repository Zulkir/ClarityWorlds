using Clarity.Engine.EventRouting;

namespace Clarity.App.Worlds.AppModes
{
    public interface IAppModeChangedEvent : IRoutedEvent
    {
        AppMode NewAppMode { get; }
    }
}