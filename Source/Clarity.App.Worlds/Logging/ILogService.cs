namespace Clarity.App.Worlds.Logging
{
    public interface ILogService
    {
        void Write(LogMessageType messageType, string message);
    }
}