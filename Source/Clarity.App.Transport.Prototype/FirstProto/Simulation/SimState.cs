using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.App.Transport.Prototype.FirstProto.Simulation
{
    public class SimState : ISimState
    {
        private readonly List<SimPackage> packages;

        public double Timestamp { get; set; }

        public SimState()
        {
            packages = new List<SimPackage>();
        }

        public IEnumerable<string> GetSites()
        {
            return GetPackages().SelectMany(x => new[] {x.FromSite, x.ToSite}).Distinct();
        }

        public IEnumerable<SimPackage> GetPackages()
        {
            return packages.Where(x => x.Alive);
        }

        public void AddPackage(SimPackage package)
        {
            packages.Add(package);
        }

        public void RemovePackage(long id)
        {
            var removeIndex = packages.IndexOf(x => x.Id == id) ?? throw new IndexOutOfRangeException();
            packages.RemoveAt(removeIndex);
        }
    }
}