using Clarity.App.Transport.Prototype.TransportLogs;

namespace Clarity.App.Transport.Prototype.Simulation
{
    public class PackageBeginFrame : ISimFrame
    {
        public double Timestamp { get; }
        public int? IncedentalEntryIndex { get; }
        private readonly SimPackage package;

        public PackageBeginFrame(double timestamp, LogEntry entry, SimPackage package)
        {
            Timestamp = timestamp;
            IncedentalEntryIndex = entry.Header.Sequence;
            this.package = package;
        }

        public void Apply(ISimState state) => state.AddPackage(package);
        public void Undo(ISimState state) => state.RemovePackage(package.Id);
    }
}