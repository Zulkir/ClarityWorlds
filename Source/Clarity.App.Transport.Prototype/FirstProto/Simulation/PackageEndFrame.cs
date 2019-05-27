namespace Clarity.App.Transport.Prototype.FirstProto.Simulation
{
    public class PackageEndFrame : ISimFrame
    {
        public double Timestamp { get; }
        public int? IncedentalEntryIndex => null;
        private readonly SimPackage package;

        public PackageEndFrame(double timestamp, SimPackage package)
        {
            Timestamp = timestamp;
            this.package = package;
        }

        public void Apply(ISimState state) => state.RemovePackage(package.Id);
        public void Undo(ISimState state) => state.AddPackage(package);
    }
}