using System;
using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public class CirclePacker
    {
        private const float BorderMinDistanceInRadii = 0.2f;
        private const float MaxDensity = 0.907f;

        private float circleRadius;
        private CirclePackingBorder border;
        private Vector2[] circleCenters;
        private int numCircles;

        public Vector2[] CircleCenters => circleCenters;
        public int NumCircles => numCircles;

        public void Initialize(float circleRadius, Vector2[] borderPoints)
        {
            this.circleRadius = circleRadius;
            this.border = new CirclePackingBorder(borderPoints, circleRadius);
        }

        private static IEnumerable<Vector2> NormalizeBorder(IEnumerable<Vector2> roughBorder, float maxDistance)
        {
            using (var e = roughBorder.GetEnumerator())
            {
                if (!e.MoveNext())
                    throw new ArgumentException("'roughBorder' contains no elements");
                var firstPoint = e.Current;
                yield return firstPoint;
                var prevPoint = firstPoint;
                while (e.MoveNext())
                {
                    var nextPoint = e.Current;
                    var numMidPoints = (int)((nextPoint - prevPoint).Length() / maxDistance);
                    for (var i = 0; i < numMidPoints; i++)
                        yield return Vector2.Lerp(prevPoint, nextPoint, (float)(i + 1) / (numMidPoints + 1));
                    yield return nextPoint;
                    prevPoint = nextPoint;
                }

                {
                    var numMidPoints = (int)((firstPoint - prevPoint).Length() / maxDistance);
                    for (var i = 0; i < numMidPoints; i++)
                        yield return Vector2.Lerp(prevPoint, firstPoint, (float)(i + 1) / (numMidPoints + 1));
                }
            }
        }
    }
}