using System.Collections.Generic;
using Clarity.App.Transport.Prototype.SimLogs;

namespace Clarity.App.Transport.Prototype.FirstProto.Simulation
{
    public class SimFrameGenerator : ISimFrameGenerator
    {
        private long nextId;

        public IEnumerable<ISimFrame> FromLogEntry(SimLogEntry logEntry)
        {
            var header = logEntry.Header;
            var timestamp = header.Systime;
            switch (logEntry.Header.Code)
            {
                case SimLogEntryCode.Read:
                case SimLogEntryCode.Update:
                case SimLogEntryCode.Create:
                {
                    var body = CrudSimLogEntryBody.Parse(logEntry.BodyStr);
                    var package = new SimPackage
                    {
                        Id = nextId++,
                        Entry = logEntry,
                        Alive = true,
                        FromSite = body.FromSite,
                        ToSite = body.ToSite,
                        DepartureTime = timestamp,
                        ArrivalTime = timestamp + body.CommunicationTime,
                        Size = body.CommunicationCost
                    };
                    yield return new PackageBeginFrame(package.DepartureTime, logEntry, package);
                    yield return new PackageEndFrame(package.ArrivalTime, package);
                    break;
                }
                case SimLogEntryCode.MigrationStart:
                case SimLogEntryCode.NewCopy:
                case SimLogEntryCode.RemoveCopy:
                case SimLogEntryCode.MigrationEnd:
                {
                    var body = MigrationSimLogEntryBody.Parse(logEntry.BodyStr);
                    var package = new SimPackage
                    {
                        Id = nextId++,
                        Entry = logEntry,
                        Alive = true,
                        FromSite = body.SourceSite,
                        ToSite = body.TargetSite,
                        DepartureTime = timestamp,
                        ArrivalTime = timestamp + body.CommunicationTime,
                        Size = body.CommunicationCost
                    };
                    yield return new PackageBeginFrame(package.DepartureTime, logEntry, package);
                    yield return new PackageEndFrame(package.ArrivalTime, package);
                    break;
                }
            }
        }
    }
}