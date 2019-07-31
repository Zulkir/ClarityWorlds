using System;
using System.Linq;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public class CirclePacker
    {
        private const float BorderMinDistanceInRadii = 0.2f;
        private const float MaxDensity = 0.907f;

        private float circleRadius;
        private float circleArea;
        private CirclePackingBorder border;
        private Vector2[] circleCenters;
        private int maxNumCircles;
        private int numCircles;

        public float CircleRadius => circleRadius;
        public CirclePackingBorder Border => border;
        public Vector2[] CircleCenters => circleCenters;
        public int MaxNumCircles => maxNumCircles;
        public int NumCircles => numCircles;

        public void Reset(float circleRadius, Vector2[] borderPoints)
        {
            this.circleRadius = circleRadius;
            circleArea = new Circle2(Vector2.Zero, circleRadius).Area;
            this.border = new CirclePackingBorder(borderPoints, circleRadius);
            maxNumCircles = (int)(border.Area / circleArea);
            var rnd = new Random();
            circleCenters = Enumerable.Range(0, int.MaxValue)
                .Select(x => new Vector2(
                    (border.BoundingRect.MinX + circleRadius) + (float)rnd.NextDouble() * (border.BoundingRect.Width - 2 * circleRadius),
                    (border.BoundingRect.MinY + circleRadius) + (float)rnd.NextDouble() * (border.BoundingRect.Height - 2 * circleRadius)))
                .Where(x => border.PointIsValid(x))
                .Take(maxNumCircles)
                .ToArray();
            numCircles = maxNumCircles;
        }
    }
}