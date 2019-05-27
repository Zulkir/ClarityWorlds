namespace Clarity.App.Worlds.AppModes
{
    public interface IAppModeService
    {
        AppMode Mode { get; }
        void SetMode(AppMode mode);
    }
}