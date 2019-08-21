using System.Collections.Generic;

namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public interface ICirclePackingSolvingStatus
    {
        int AttemptsSinceLastSuccess { get; }
        float SecondsSinceLastSuccess { get; }
        IReadOnlyList<CirclePackingSolverSuccessEntry> Successes { get; }
    }
}