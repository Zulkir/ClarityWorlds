using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Coroutines;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public class CirclePacker
    {
        private const float BorderMinDistanceInRadii = 0.2f;
        private const float MaxDensity = 0.907f;
        private const float Precision = 1e-2f;
        
        private readonly Random random;

        private float circleRadius;
        private float circleArea;
        private ICirclePackingBorder border;
        private Vector2[] frontCircleCenters;
        private Vector2[] backCircleCenters;
        private CirclePackingCircleGrid frontCirclesGrid;
        private CircleStatus[] frontCircleStatuses;
        private int maxNumCircles;
        private int numCircles;
        
        private bool runningAsync;
        
        public float RandomFactor { get; set; }
        public int NumIterationPerBreak { get; set; } = 10;
        public int BatchSize { get; set; } = 100;
        public int MaxIterations { get; set; } = int.MaxValue;

        public bool IsSuccessfulConfiguration { get; private set; }

        public CirclePacker()
        {
            random = new Random();
        }

        public float CircleRadius => circleRadius;
        public ICirclePackingBorder Border => border;
        public Vector2[] FrontCircleCenters => frontCircleCenters;
        public CircleStatus[] FrontCircleStatuses => frontCircleStatuses;
        public int MaxNumCircles => maxNumCircles;
        public int NumCircles => numCircles;

        public void ResetAll(float circleRadius, ICirclePackingBorder border)
        {
            this.circleRadius = circleRadius;
            circleArea = new Circle2(Vector2.Zero, circleRadius).Area;
            this.border = border;
            maxNumCircles = (int)(border.Area / circleArea);
            backCircleCenters = new Vector2[maxNumCircles];
            frontCircleCenters = new Vector2[maxNumCircles];
            frontCirclesGrid = new CirclePackingCircleGrid(circleRadius, border.BoundingRect);
            frontCircleStatuses = new CircleStatus[maxNumCircles];
        }

        public void FillWithHoneycomb()
        {
            numCircles = 0;
            frontCirclesGrid.Reset();
            foreach (var circleCenter in GenerateHoneycomb(border.BoundingRect, circleRadius).Where(border.PointIsValid))
                frontCircleCenters[numCircles++] = circleCenter;
            frontCirclesGrid.Rebuild(frontCircleCenters, numCircles);
        }

        private static IEnumerable<Vector2> GenerateHoneycomb(AaRectangle2 boundingRect, float circleRadius)
        {
            var doubleRadius = 2 * circleRadius;
            var minIndexX = (int)(boundingRect.MinX / doubleRadius) - 2;
            var maxIndexX = (int)(boundingRect.MaxX / doubleRadius) + 2;
            var minIndexY = (int)(boundingRect.MinY / doubleRadius) - 2;
            var maxIndexY = (int)(boundingRect.MaxY / doubleRadius) + 2;
            var h = doubleRadius * MathHelper.Cos(MathHelper.Pi / 6);
            for (var j = minIndexY; j <= maxIndexY; j++)
            {
                var zeroCircleCenter = j % 2 == 0
                    ? new Vector2(0, h * j)
                    : new Vector2(circleRadius, h * j);
                for (var i = minIndexX; i <= maxIndexX; i++)
                    yield return zeroCircleCenter + new Vector2(i * doubleRadius, 0);
            }
        }

        public void RandomizeCircles(int maxInitialCircles)
        {
            var numInitialCircles = Math.Min(maxInitialCircles, maxNumCircles);
            FillFrontWithRandomCircles(numInitialCircles);
            RefreshStatuses();
        }

        private Vector2 GetRandomVector(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(
                minX + (float)random.NextDouble() * (maxX - minX),
                minY + (float)random.NextDouble() * (maxY - minY));
        }

        private void FillFrontWithRandomCircles(int numCirclesToFill)
        {
            numCircles = 0;
            frontCirclesGrid.Reset();
            while (numCircles < numCirclesToFill)
            {
                var point = GetRandomVector(
                    border.BoundingRect.MinX + circleRadius, border.BoundingRect.MaxX - circleRadius,
                    border.BoundingRect.MinY + circleRadius, border.BoundingRect.MaxY - circleRadius);
                if (border.PointIsValid(point) && frontCirclesGrid.TryFit(point))
                {
                    frontCircleCenters[numCircles] = point;
                    numCircles++;
                }
            }
            frontCirclesGrid.Rebuild(frontCircleCenters, numCircles);
        }

        public void OptimizeStep()
        {
            for (var i = 0; i < numCircles; i++)
            {
                var iLoc = i;
                var circleToMove = frontCircleCenters[iLoc];
                var neighborIndices = frontCirclesGrid.GetNeighborIndices(iLoc);
                var offset = Vector2.Zero;
                foreach (var neighborIndex in neighborIndices)
                {
                    var neighborCenter = frontCircleCenters[neighborIndex];
                    var fromNeighbor = circleToMove - neighborCenter;

                    var maxDist = 2 * circleRadius;
                    var dist = fromNeighbor.Length();
                    if (dist >= maxDist)
                        continue;

                    offset += fromNeighbor * (maxDist - dist) * 0.5f;
                }

                var randomRange = RandomFactor * circleRadius;
                offset += GetRandomVector(-randomRange, randomRange, -randomRange, randomRange);
                if (offset.Length() > circleRadius / 2)
                    offset = offset.Normalize() * circleRadius / 2;
                var unrestrictedNewCenter = circleToMove + offset;
                backCircleCenters[iLoc] = border.FindClosestValidPoint(unrestrictedNewCenter);
            }
            SwapBuffers();
        }

        private void SwapBuffers()
        {
            CodingHelper.Swap(ref frontCircleCenters, ref backCircleCenters);
            frontCirclesGrid.Rebuild(frontCircleCenters, numCircles);
        }

        private void RefreshStatuses()
        {
            for (var i = 0; i < numCircles; i++)
            {
                var center = frontCircleCenters[i];
                var closestDistanceSq = frontCirclesGrid.GetNeighborIndices(i)
                    .Select(x => (frontCircleCenters[x] - center).LengthSquared())
                    .MinOrNull() ?? float.MaxValue;
                frontCircleStatuses[i] = new CircleStatus(MathHelper.Sqrt(closestDistanceSq));
            }
            var distanceThreshold = circleRadius * 2 - Precision;
            IsSuccessfulConfiguration = frontCircleStatuses.Take(numCircles).All(x => x.MinDistance > distanceThreshold);
        }

        public void RunOptimization(int maxIterations, int costGracePeriod, float minCostDecrease)
        {
            // todo: make actually follow params
            var maxEvaluations = maxIterations / costGracePeriod;
            var iterationsPerEvaluation = maxIterations / maxEvaluations;

            runningAsync = false;
            var prevCost = EvaluateCost();
            for (var e = 0; e < maxEvaluations; e++)
            {
                for (var i = 0; i < iterationsPerEvaluation; i++)
                    OptimizeStep();
                var cost = EvaluateCost();
                if (prevCost - cost < minCostDecrease)
                    // todo: next stage (shake, replace, etc.)
                    break;
            }
            RefreshStatuses();
        }

        public float EvaluateCost()
        {
            var cost = 0.0;
            for (var i = 0; i < numCircles; i++)
            {
                var circleCenter = frontCircleCenters[i];
                var neighborIndices = frontCirclesGrid.GetNeighborIndices(i);
                foreach (var neighborIndex in neighborIndices)
                {
                    var neighborCenter = frontCircleCenters[neighborIndex];
                    var fromNeighbor = circleCenter - neighborCenter;
                    var maxDist = 2 * circleRadius;
                    var dist = fromNeighbor.Length();
                    cost += Math.Max(0, maxDist - dist);
                }
            }
            return (float)cost;
        }

        public async void RunOptimizationAsync(ICoroutineService coroutineService)
        {
            if (runningAsync)
                return;

            runningAsync = true;
            for (var i = 0; i < MaxIterations; i++)
            {
                if (!runningAsync)
                    break;
                OptimizeStep();
                if ((i+1) % NumIterationPerBreak == 0)
                {
                    RefreshStatuses();
                    await coroutineService.WaitUpdates(1);
                }
            }
            runningAsync = false;
        }

        public void StopOptimization()
        {
            runningAsync = false;
        }

        public void DeleteCircle()
        {
            numCircles--;
        }
    }
}