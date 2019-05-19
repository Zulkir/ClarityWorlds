namespace Clarity.Core.AppCore.Configuration
{
    public interface IConfigService
    {
        T GetConfig<T>() where T : IConfig, new();
        void SetConfig<T>(T config) where T : IConfig;
    }
}