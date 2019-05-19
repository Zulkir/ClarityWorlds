namespace Clarity.Core.AppCore.Configuration
{
    public static class Config
    {
        private static IConfigService configService;
        public static void Initialize(IConfigService actualConfigService) => configService = actualConfigService;
        public static T Get<T>() where T : IConfig, new() => configService.GetConfig<T>();
        public static void Set<T>(T config) where T : IConfig, new() => configService.SetConfig(config);
    }
}