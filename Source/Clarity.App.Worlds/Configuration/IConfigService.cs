namespace Clarity.App.Worlds.Configuration
{
    public interface IConfigService
    {
        T GetConfig<T>() where T : class, IConfig, new();
        void SaveConfig<T>() where T : class, IConfig, new();
    }
}