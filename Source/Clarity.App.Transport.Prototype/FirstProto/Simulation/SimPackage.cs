using Clarity.App.Transport.Prototype.SimLogs;

namespace Clarity.App.Transport.Prototype.FirstProto.Simulation
{
    public class SimPackage
    {
        public long Id;
        public SimLogEntry Entry;
        public string FromSite;
        public string ToSite;
        public double DepartureTime;
        public double ArrivalTime;
        public float Size;
        public bool Alive;
    }
}