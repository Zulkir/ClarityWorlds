using System.Collections.Generic;

namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public class CirclePackingSolvingStatus : ICirclePackingSolvingStatus
    {
        public int AttemptsSinceLastSuccess { get; set; }
        public float SecondsSinceLastSuccess { get; set; }

        public List<CirclePackingSolverSuccessEntry> Successes { get; } = new List<CirclePackingSolverSuccessEntry>();
        IReadOnlyList<CirclePackingSolverSuccessEntry> ICirclePackingSolvingStatus.Successes => Successes;
    }
}