namespace Clarity.App.Worlds.Logging
{
    public interface ILogWriter
    {
        void Write(LogMessageType messageType, string message);
    }
}