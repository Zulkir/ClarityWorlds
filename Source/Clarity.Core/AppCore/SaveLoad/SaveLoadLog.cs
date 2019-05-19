using Clarity.Core.AppCore.Logging;

namespace Clarity.Core.AppCore.SaveLoad
{
    public static class SaveLoadLog
    {
        private static int tabs;

        public static void Tab() => tabs += 4;
        public static void Untab() => tabs -= 4;

        public static void Info(string text) => Log.Write(LogMessageType.Info, "SL:" + new string(' ', tabs) + text);
    }
}