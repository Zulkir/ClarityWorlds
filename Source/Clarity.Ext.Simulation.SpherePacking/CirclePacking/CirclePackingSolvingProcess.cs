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

        public CirclePacker Packer => packer;
        public ICirclePackingSolvingStatus Status => status;

        public CirclePackingSolvingProcess(CirclePackingSolverSettings settings)
        {
            this.settings = settings;
            status = new CirclePackingSolvingStatus();
            packer = new CirclePacker();
            stopwatch = new Stopwatch();
            stopwatch.Reset();
            Initialize();
        }

        private void Initialize()
        {
            stopwatch.Restart();
            packer.ResetAll(settings.CircleRadius, settings.Border, settings.Precision);
            packer.FillWithHoneycomb();
            if (!TryPushSuccess(0))
                throw new Exception("Honeycomb configuration was unsuccessful.");
            
        }

        public void RunFor(int numAttempts)
        {
            var numCirclesToTry = status.Successes.Last().Configuration.Count + 1;
            for (var i = 0; i < numAttempts; i++)
            {
                RunAttempt(numCirclesToTry);
                if (TryPushSuccess(i - 1))
                    break;
                status.AttemptsSinceLastSuccess++;
                status.SecondsSinceLastSuccess = stopwatch.ElapsedMilliseconds / 1000f;
            }
        }

        private enum AttemptStage
        {
            Repulsing1,
            Shaking,
            Repulsing2,
            FillingHoles
        }

        private void RunAttempt(int numCirclesToTry)
        {
            packer.RandomizeCircles(numCirclesToTry);
            var state = AttemptStage.Repulsing1;
            var bestCost = float.MaxValue;
            var lastCostUpdate = 0;
            var shakeStart = 0;
            for (var i = 0; i < settings.MaxIterationsPerAttempt; i++)
            {
                if (state == AttemptStage.Repulsing1 || state == AttemptStage.Repulsing2)
                {
                    packer.RandomFactor = 0;
                    packer.OptimizeStep(out var cost);
                    if (cost < bestCost - settings.MinCostDecrease)
                    {
                        bestCost = cost;
                        lastCostUpdate = i;
                    }
                    if (i - lastCostUpdate > settings.CostDecreaseGracePeriod)
                    {
                        state++;
                        if (state == AttemptStage.Shaking)
                            shakeStart = i;
                    }
                }
                else if (state == AttemptStage.Shaking)
                {
                    packer.RandomFactor = settings.ShakeStrength;
                    if (i - shakeStart >= settings.ShakeIterations)
                        state++;
                }
                else if (state == AttemptStage.FillingHoles)
                {
                    //packer.TryFillHoles();
                    break;
                }
            }
            packer.RefreshStatuses();
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
            status.AttemptsSinceLastSuccess = 0;
            status.SecondsSinceLastSuccess = 0;
            stopwatch.Restart();
            return true;
        }
    }
}