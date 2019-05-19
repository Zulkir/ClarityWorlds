namespace Clarity.Core.AppCore.Logging
{
    public interface ILogWriter
    {
        void Write(LogMessageType messageType, string message);
    }
}