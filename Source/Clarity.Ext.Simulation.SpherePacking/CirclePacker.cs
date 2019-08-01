using System;
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

        private float circleRadius;
        private float circleArea;
        private CirclePackingBorder border;
        private Vector2[] frontCircleCenters;
        private Vector2[] backCircleCenters;
        private int maxNumCircles;
        private int numCircles;

        private bool circlesConverged;
        private bool runningAsync;

        public CirclePacker(ICoroutineService coroutineService)
        {
            this.coroutineService = coroutineService;
        }

        public float CircleRadius => circleRadius;
        public CirclePackingBorder Border => border;
        public Vector2[] FrontCircleCenters => frontCircleCenters;
        public int MaxNumCircles => maxNumCircles;
        public int NumCircles => numCircles;

        public void Reset(float circleRadius, Vector2[] borderPoints)
        {
            this.circleRadius = circleRadius;
            circleArea = new Circle2(Vector2.Zero, circleRadius).Area;
            this.border = new CirclePackingBorder(borderPoints, circleRadius);
            maxNumCircles = (int)(border.Area / circleArea);
            var rnd = new Random();
            frontCircleCenters = Enumerable.Range(0, int.MaxValue)
                .Select(x => new Vector2(
                    (border.BoundingRect.MinX + circleRadius) + (float)rnd.NextDouble() * (border.BoundingRect.Width - 2 * circleRadius),
                    (border.BoundingRect.MinY + circleRadius) + (float)rnd.NextDouble() * (border.BoundingRect.Height - 2 * circleRadius)))
                .Where(x => border.PointIsValid(x))
                .Take(maxNumCircles)
                .ToArray();
            numCircles = maxNumCircles;
            backCircleCenters = frontCircleCenters.ToArray();
            circlesConverged = false;
        }

        public void OptimizeStep()
        {
            if (!circlesConverged)
            {
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
                    if (offset.Length() > circleRadius / 2)
                        offset = offset.Normalize() * circleRadius / 2;
                    var unrestrictedNewCenter = circleToMove + offset;
                    backCircleCenters[i] = border.FindClosestValidPoint(unrestrictedNewCenter);
                }
                //circlesConverged = true;
                SwapBuffers();
            }
        }

        public async void RunOptimization()
        {
            if (runningAsync)
            {
                runningAsync = false;
                return;
            }

            runningAsync = true;
            while (runningAsync)
            {
                OptimizeStep();
                await coroutineService.WaitUpdates(1);
            }
        }

        private void SwapBuffers()
        {
            CodingHelper.Swap(ref frontCircleCenters, ref backCircleCenters);
        }
    }
}