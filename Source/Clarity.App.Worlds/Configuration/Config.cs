namespace Clarity.App.Worlds.Configuration
{
    public static class Config
    {
        private static IConfigService configService;
        public static void Initialize(IConfigService actualConfigService) => configService = actualConfigService;
        public static T Get<T>() where T : class, IConfig, new() => configService.GetConfig<T>();
        public static void Save<T>() where T : class, IConfig, new() => configService.SaveConfig<T>();
    }
}