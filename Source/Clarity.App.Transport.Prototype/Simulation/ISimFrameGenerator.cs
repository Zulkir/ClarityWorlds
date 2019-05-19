using System.Collections.Generic;
using Clarity.App.Transport.Prototype.TransportLogs;

namespace Clarity.App.Transport.Prototype.Simulation
{
    public interface ISimFrameGenerator
    {
        IEnumerable<ISimFrame> FromLogEntry(LogEntry logEntry);
    }
}