namespace Clarity.App.Worlds.AppModes
{
    public interface IAppModeService
    {
        AppMode Mode { get; }
        AppNavigationMode NavigationMode { get; }
        void SetMode(AppMode mode);
        void SetNavigationMode(AppNavigationMode navigationMode);
    }
}