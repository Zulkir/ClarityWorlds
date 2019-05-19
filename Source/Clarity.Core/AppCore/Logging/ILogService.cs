namespace Clarity.Core.AppCore.Logging
{
    public interface ILogService
    {
        void Write(LogMessageType messageType, string message);
    }
}