using System;
using System.Diagnostics;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
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
}