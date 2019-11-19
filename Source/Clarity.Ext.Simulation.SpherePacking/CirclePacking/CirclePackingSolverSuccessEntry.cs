using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public struct CirclePackingSolverSuccessEntry
    {
        public IReadOnlyList<Vector2> Configuration;
        public int AttemptsTaken;
        public double SecondsTaken;
        public int CumulativeAttemptsTaken;
        public double CumulativeSecondsTaken;
    }
}