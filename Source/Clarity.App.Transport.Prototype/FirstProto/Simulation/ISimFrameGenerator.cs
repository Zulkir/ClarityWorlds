using System.Collections.Generic;
using Clarity.App.Transport.Prototype.SimLogs;

namespace Clarity.App.Transport.Prototype.FirstProto.Simulation
{
    public interface ISimFrameGenerator
    {
        IEnumerable<ISimFrame> FromLogEntry(SimLogEntry logEntry);
    }
}