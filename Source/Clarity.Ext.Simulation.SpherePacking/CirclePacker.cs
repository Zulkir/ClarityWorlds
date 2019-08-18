using System;
using System.Linq;
using Clarity.App.Worlds.Coroutines;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public class CirclePacker
    {
        private const float BorderMinDistanceInRadii = 0.2f;
        private const float MaxDensity = 0.907f;
        private const float Precision = 1e-2f;

        private readonly ICoroutineService coroutineService;
        private readonly Random random;

        private float circleRadius;
        private float circleArea;
        private CirclePackingBorder border;
        private Vector2[] frontCircleCenters;
        private Vector2[] backCircleCenters;
        private CirclePackingCircleGrid frontCirclesGrid;
        private CircleStatus[] frontCircleStatuses;
        private int maxNumCircles;
        private int numCircles;

        private bool circlesConverged;
        private bool runningAsync;
        
        public float RandomFactor { get; set; }
        public int NumIterationPerBreak { get; set; } = 10;
        public int BatchSize { get; set; } = 100;
        public int MaxIterations { get; set; } = int.MaxValue;

        public CirclePacker(ICoroutineService coroutineService)
        {
            this.coroutineService = coroutineService;
            random = new Random();
        }

        public float CircleRadius => circleRadius;
        public CirclePackingBorder Border => border;
        public Vector2[] FrontCircleCenters => frontCircleCenters;
        public CircleStatus[] FrontCircleStatuses => frontCircleStatuses;
        public int MaxNumCircles => maxNumCircles;
        public int NumCircles => numCircles;

        private Vector2 GetRandomVector(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(
                minX + (float)random.NextDouble() * (maxX - minX),
                minY + (float)random.NextDouble() * (maxY - minY));
        }

        public void Reset(float circleRadius, int maxInitialCircles, Vector2[] borderPoints)
        {
            this.circleRadius = circleRadius;
            circleArea = new Circle2(Vector2.Zero, circleRadius).Area;
            this.border = new CirclePackingBorder(borderPoints, circleRadius);
            maxNumCircles = (int)(border.Area / circleArea);
            var numInitialCircles = Math.Min(maxInitialCircles, maxNumCircles);
            
            frontCirclesGrid = new CirclePackingCircleGrid(circleRadius, border.BoundingRect);
            frontCircleCenters = Enumerable.Range(0, int.MaxValue)
                .Select(x => GetRandomVector(
                    border.BoundingRect.MinX + circleRadius, border.BoundingRect.MaxX - circleRadius,
                    border.BoundingRect.MinY + circleRadius, border.BoundingRect.MaxY - circleRadius))
                .Where(x => frontCirclesGrid.TryFit(x))
                .Where(x => border.PointIsValid(x))
                .Take(numInitialCircles)
                .ToArray();
            numCircles = numInitialCircles;
            backCircleCenters = frontCircleCenters.ToArray();
            frontCirclesGrid.Rebuild(frontCircleCenters, numCircles);
            frontCircleStatuses = new CircleStatus[numCircles];
            RefreshStatuses();
            circlesConverged = false;
        }

        public void OptimizeStep()
        {
            if (!circlesConverged)
            {
                // todo: parallelize
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
        }

        public async void RunOptimization()
        {
            if (runningAsync)
                return;

            runningAsync = true;
            for (int i = 0; i < MaxIterations; i++)
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