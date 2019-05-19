using Clarity.App.Transport.Prototype.TransportLogs;

namespace Clarity.App.Transport.Prototype.Simulation
{
    public class SimPackage
    {
        public long Id;
        public LogEntry Entry;
        public string FromSite;
        public string ToSite;
        public double DepartureTime;
        public double ArrivalTime;
        public float Size;
        public bool Alive;
    }
}