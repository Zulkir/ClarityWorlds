using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public class CirclePackingBorder
    {
        private readonly Vector2[] points;
        private readonly float circleRadius;
        private readonly float circleRadiusSq;

        private IEnumerable<LineSegment2> Segments => EnumerateSegments(points);

        public CirclePackingBorder(IEnumerable<Vector2> points, float circleRadius)
        {
            this.points = Normalize(points);
            this.circleRadius = circleRadius;
            circleRadiusSq = circleRadius.Sq();
        }

        private static Vector2[] Normalize(IEnumerable<Vector2> points)
        {
            var pointsList = points.ToList();
            PurgeDuplicates(pointsList);
            var sumAngles = EnumerateSegments(pointsList)
                .Select(x => x.ToArrowVector()).SequentialPairs(true)
                .Sum(x => SignedAngleBetween(x.First, x.Second));
            if (sumAngles < 0)
                pointsList.Reverse();
            return pointsList.ToArray();
        }

        private static void PurgeDuplicates(List<Vector2> points)
        {
            if (points.IsEmpty())
                return;
            if (points[0] == points[points.Count - 1])
                points.RemoveAt(points.Count - 1);
            for (var i = points.Count - 1; i >= 1; i--)
                if (points[i] == points[i - 1])
                    points.RemoveAt(i);
        }

        private static IEnumerable<LineSegment2> EnumerateSegments(IReadOnlyList<Vector2> points)
        {
            return points.SequentialPairs(true).Select(x => new LineSegment2(x.First, x.Second));
        }

        private static float SignedAngleBetween(Vector2 v1, Vector2 v2)
        {
            return MathHelper.Asin(Vector2.Cross(v1.Normalize(), v2.Normalize()));
        }

        private void FindClosestBorderPoint(Vector2 point, out Vector2 closestBorderPoint, out float distanceSq, out bool isInside)
        {
            closestBorderPoint = default(Vector2);
            distanceSq = float.MaxValue;
            isInside = false;

            foreach (var segment in Segments)
            {
                var pointRel = point - segment.Point1;
                var arrow = segment.ToArrowVector();
                var arrowLength = arrow.Length();
                var arrowNorm = arrow / arrowLength;
                var isInsideForSegment = Vector2.Cross(arrow, pointRel) > 0;

                var distSqPoint1 = pointRel.LengthSquared();
                if (distSqPoint1 < distanceSq)
                {
                    closestBorderPoint = segment.Point1;
                    distanceSq = distSqPoint1;
                    isInside = isInsideForSegment;
                }
                var offsetAlongArrow = Vector2.Dot(pointRel, arrowNorm);
                if (0 < offsetAlongArrow && offsetAlongArrow < arrowLength)
                {
                    var segmentPoint = segment.Point1 + arrowNorm * offsetAlongArrow;
                    var distSqSegment = (point - segmentPoint).LengthSquared();
                    if (distSqSegment < distanceSq)
                    {
                        closestBorderPoint = segmentPoint;
                        distanceSq = distSqSegment;
                        isInside = isInsideForSegment;
                    }
                }
            }
        }

        public bool PointIsValid(Vector2 point)
        {
            FindClosestBorderPoint(point, out var closestBorderPoint, out var distanceSq, out var isInside);
            return isInside && distanceSq >= circleRadiusSq;
        }
        
    }
}