using System.Collections.Generic;

namespace Clarity.App.Worlds.Logging
{
    public class LogService : ILogService
    {
        private readonly IReadOnlyList<ILogWriter> logs;

        public LogService(IReadOnlyList<ILogWriter> logs)
        {
            this.logs = logs;
        }

        public void Write(LogMessageType messageType, string message)
        {
            foreach (var log in logs)
                log.Write(messageType, message);
        }
    }
}