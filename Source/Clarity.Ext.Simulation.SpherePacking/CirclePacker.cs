using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Coroutines;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public class CirclePacker
    {
        private const float BorderMinDistanceInRadii = 0.2f;
        private const float MaxDensity = 0.907f;

        private readonly ICoroutineService coroutineService;
        private readonly Random random;

        private float circleRadius;
        private float circleArea;
        private CirclePackingBorder border;
        private Vector2[] frontCircleCenters;
        private Vector2[] backCircleCenters;
        private CircleStatus[] frontCircleStatuses;
        private int maxNumCircles;
        private int numCircles;

        private bool circlesConverged;
        private bool runningAsync;
        
        public float RandomFactor { get; set; }
        public int NumIterationPerBreak { get; set; } = 100;
        public int BatchSize { get; set; } = 100;

        public CirclePacker(ICoroutineService coroutineService)
        {
            this.coroutineService = coroutineService;
            random = new Random();
        }

        public float CircleRadius => circleRadius;
        public CirclePackingBorder Border => border;
        public Vector2[] FrontCircleCenters => frontCircleCenters;
        public int MaxNumCircles => maxNumCircles;
        public int NumCircles => numCircles;

        private Vector2 GetRandomVector(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(
                minX + (float)random.NextDouble() * (maxX - minX),
                minY + (float)random.NextDouble() * (maxY - minY));
        }

        public void Reset(float circleRadius, Vector2[] borderPoints)
        {
            this.circleRadius = circleRadius;
            circleArea = new Circle2(Vector2.Zero, circleRadius).Area;
            this.border = new CirclePackingBorder(borderPoints, circleRadius);
            maxNumCircles = (int)(border.Area / circleArea);
            
            frontCircleCenters = Enumerable.Range(0, int.MaxValue)
                .Select(x => GetRandomVector(
                    border.BoundingRect.MinX + circleRadius, border.BoundingRect.MaxX - circleRadius,
                    border.BoundingRect.MinY + circleRadius, border.BoundingRect.MaxY - circleRadius))
                .Where(x => border.PointIsValid(x))
                .Take(maxNumCircles)
                .ToArray();
            numCircles = maxNumCircles;
            backCircleCenters = frontCircleCenters.ToArray();
            frontCircleStatuses = new CircleStatus[numCircles];
            circlesConverged = false;
        }

        public void OptimizeStep()
        {
            if (!circlesConverged)
            {
                // todo: parallelize
                for (int i = 0; i < numCircles; i++)
                {
                    var iLoc = i;
                    var circleToMove = frontCircleCenters[i];
                    var closestNeighbors = frontCircleCenters.Where((x, j) => j != iLoc).Where(x => (x - circleToMove).LengthSquared() <= (2 * circleRadius).Sq());
                    var offset = Vector2.Zero;
                    foreach (var closestNeighbor in closestNeighbors)
                    {
                        var fromNeighbor = circleToMove - closestNeighbor;
                        var dist = fromNeighbor.Length();
                        var minDist = circleRadius;
                        var maxDist = 2 * circleRadius;
                        var normalizedDist = MathHelper.Clamp((dist - minDist) / (maxDist - minDist), 0, 1);
                        var offsetMagnitude = MathHelper.Pow((1 - normalizedDist), 1.5f) * circleRadius / 2;
                        offset += fromNeighbor / dist * offsetMagnitude;
                    }

                    var randomRange = RandomFactor * circleRadius;
                    offset += GetRandomVector(-randomRange, randomRange, -randomRange, randomRange);
                    if (offset.Length() > circleRadius / 2)
                        offset = offset.Normalize() * circleRadius / 2;
                    var unrestrictedNewCenter = circleToMove + offset;
                    backCircleCenters[i] = border.FindClosestValidPoint(unrestrictedNewCenter);
                }
                //circlesConverged = true;
                SwapBuffers();
            }
        }

        private IEnumerable<Vector2> GetNeighbors(Vector2 circleCenter)
        {
            return frontCircleCenters
                .Where(x => x != circleCenter)
                .Where(x => (x - circleCenter).LengthSquared() <= (2 * circleRadius).Sq());
        }

        private void SwapBuffers()
        {
            CodingHelper.Swap(ref frontCircleCenters, ref backCircleCenters);
        }

        private void RefreshStatuses()
        {

        }

        public async void RunOptimization()
        {
            if (runningAsync)
                return;

            var c = 0;
            runningAsync = true;
            while (runningAsync)
            {
                OptimizeStep();
                c++;
                if (c % NumIterationPerBreak == 0)
                    await coroutineService.WaitUpdates(1);
            }
        }

        public void StopOptimization()
        {
            runningAsync = false;
        }
    }
}