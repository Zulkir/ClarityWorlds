using System;

namespace Clarity.Core.AppCore.Configuration
{
    public class ConfigService : IConfigService
    {
        public T GetConfig<T>() where T : IConfig, new()
        {
            return new T();
        }

        public void SetConfig<T>(T config) where T : IConfig
        {
            throw new NotImplementedException();
        }
    }
}