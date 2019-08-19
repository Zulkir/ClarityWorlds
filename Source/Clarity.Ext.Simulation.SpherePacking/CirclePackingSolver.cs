using System;
using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public struct CirclePackingSolverSuccessEntry
    {
        public IReadOnlyList<Vector2> Configuration;
        public int IterationsTaken;
        public double SecondsTaken;
        public int CumulativeIterationsTaken;
        public double CumulativeSecondsTaken;
    }

    public interface ICirclePackingSolverStatus
    {
        int IterationsSinceLastSuccess { get; }
        float SecondsSinceLastSuccess { get; }
        IReadOnlyList<CirclePackingSolverSuccessEntry> Succsesses { get; }
    }

    public class CirclePackingSolverStatus : ICirclePackingSolverStatus
    {
        public int IterationsSinceLastSuccess { get; set; }
        public float SecondsSinceLastSuccess { get; set; }
        public IReadOnlyList<CirclePackingSolverSuccessEntry> Succsesses { get; }
    }



    public class CirclePackingSolverSettings
    {
        public float CircleRadius { get; set; }
        public float Precision { get; set; }
        public ICirclePackingBorder Border { get; set; }
        public Action<Action> Parallelize { get; set; }

    }

    public class CirclePackingSolver
    {


        public void Solve()
        {
        }
    }
}