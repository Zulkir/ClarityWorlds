using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public struct CirclePackingSolverSuccessEntry
    {
        public IReadOnlyList<Vector2> Configuration;
        public int AttemptsTaken;
        public double SecondsTaken;
        public int CumulativeAttemptsTaken;
        public double CumulativeSecondsTaken;
    }

    public interface ICirclePackingSolvingStatus
    {
        int AttemptsSinceLastSuccess { get; }
        float SecondsSinceLastSuccess { get; }
        IReadOnlyList<CirclePackingSolverSuccessEntry> Successes { get; }
    }

    public class CirclePackingSolvingStatus : ICirclePackingSolvingStatus
    {
        public int AttemptsSinceLastSuccess { get; set; }
        public float SecondsSinceLastSuccess { get; set; }

        public List<CirclePackingSolverSuccessEntry> Successes { get; } = new List<CirclePackingSolverSuccessEntry>();
        IReadOnlyList<CirclePackingSolverSuccessEntry> ICirclePackingSolvingStatus.Successes => Successes;
    }

    public delegate void CirclePackingCallback(ICirclePackingSolvingStatus status, out bool stop);

    public class CirclePackingSolverSettings
    {
        public float CircleRadius { get; set; }
        public float Precision { get; set; }
        public ICirclePackingBorder Border { get; set; }
        //public Action<Action> Parallelize { get; set; }
        public CirclePackingCallback Callback { get; set; }
        public int MaxIterationsPerAttempt { get; set; }
        public int CostDecreaseGracePeriod { get; set; }
        public float MinCostDecrease { get; set; }
        public int AttemptsPerCallback { get; set; }
    }

    public class CirclePackingSolvingProcess
    {
        private readonly CirclePackingSolverSettings settings;
        private readonly CirclePackingSolvingStatus status;
        private readonly CirclePacker packer;
        private readonly Stopwatch stopwatch;

        public CirclePackingSolvingProcess(CirclePackingSolverSettings settings)
        {
            this.settings = settings;
            status = new CirclePackingSolvingStatus();
            packer = new CirclePacker();
            stopwatch = new Stopwatch();
            stopwatch.Reset();
        }

        public void Run()
        {
            stopwatch.Restart();
            packer.ResetAll(settings.CircleRadius, settings.Border);
            packer.FillWithHoneycomb();
            if (!TryPushSuccess(0))
                throw new Exception("Honeycomb configuration was unsuccessful.");
            var stop = false;
            while (!stop)
            {
                var numCirclesToTry = status.Successes.Last().Configuration.Count + 1;
                for (var i = 0; i < settings.AttemptsPerCallback; i++)
                {
                    packer.RandomizeCircles(numCirclesToTry);
                    packer.RunOptimization(settings.MaxIterationsPerAttempt, settings.CostDecreaseGracePeriod, settings.MinCostDecrease);
                    if (TryPushSuccess(i - 1))
                        break;
                }
                settings.Callback(status, out stop);
            }
        }

        private bool TryPushSuccess(int attemptsTaken)
        {
            if (!packer.IsSuccessfulConfiguration)
                return false;
            var prevSuccess = status.Successes.HasItems()
                ? status.Successes.Last()
                : new CirclePackingSolverSuccessEntry();
            var elapsed = (double)stopwatch.ElapsedMilliseconds / 1000;
            status.Successes.Add(new CirclePackingSolverSuccessEntry
            {
                Configuration = packer.FrontCircleCenters.Take(packer.NumCircles).ToArray(),
                AttemptsTaken = attemptsTaken,
                CumulativeAttemptsTaken = prevSuccess.CumulativeAttemptsTaken + attemptsTaken,
                SecondsTaken = elapsed,
                CumulativeSecondsTaken = prevSuccess.CumulativeSecondsTaken + elapsed
            });
            return true;
        }
    }

    public class CirclePackingSolver
    {
        public void Solve(CirclePackingSolverSettings settings) => 
            new CirclePackingSolvingProcess(settings).Run();
    }
}