namespace Clarity.Core.AppCore.Logging
{
    public static class Log
    {
        private static ILogService logService;
        public static void Initialize(ILogService actualLogService) => logService = actualLogService;
        public static void Write(LogMessageType messageType, string message) => logService.Write(messageType, message);
    }
}