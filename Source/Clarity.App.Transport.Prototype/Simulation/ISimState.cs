using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Simulation
{
    public interface ISimState
    {
        double Timestamp { get; set; }

        IEnumerable<string> GetSites();
        IEnumerable<SimPackage> GetPackages();

        void AddPackage(SimPackage package);
        void RemovePackage(long id);
    }
}