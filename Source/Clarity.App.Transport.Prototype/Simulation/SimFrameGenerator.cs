using System.Collections.Generic;
using Clarity.App.Transport.Prototype.TransportLogs;

namespace Clarity.App.Transport.Prototype.Simulation
{
    public class SimFrameGenerator : ISimFrameGenerator
    {
        private long nextId;

        public IEnumerable<ISimFrame> FromLogEntry(LogEntry logEntry)
        {
            var header = logEntry.Header;
            var timestamp = header.Systime;
            switch (logEntry.Header.Code)
            {
                case LogEntryCode.Read:
                case LogEntryCode.Update:
                case LogEntryCode.Create:
                {
                    var body = CrudLogEntryBody.Parse(logEntry.BodyStr);
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
                case LogEntryCode.MigrationStart:
                case LogEntryCode.NewCopy:
                case LogEntryCode.RemoveCopy:
                case LogEntryCode.MigrationEnd:
                {
                    var body = MigrationLogEntryBody.Parse(logEntry.BodyStr);
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