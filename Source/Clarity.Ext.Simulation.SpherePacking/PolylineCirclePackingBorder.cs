using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.SpherePacking
{
    public class PolylineCirclePackingBorder : ICirclePackingBorder
    {
        private readonly Vector2[] points;
        private readonly float circleRadius;
        private readonly float circleRadiusSq;

        public IReadOnlyList<Vector2> Points => points;
        public AaRectangle2 BoundingRect { get; }
        public float Area { get; }

        private IEnumerable<LineSegment2> Segments => EnumerateSegments(points);

        public PolylineCirclePackingBorder(IEnumerable<Vector2> rawPoints, float circleRadius)
        {
            points = Normalize(rawPoints);
            this.circleRadius = circleRadius;
            circleRadiusSq = circleRadius.Sq();
            BoundingRect = AaRectangle2.BoundingRect(points);
            Area = CalculateArea(points);
        }

        private static Vector2[] Normalize(IEnumerable<Vector2> points)
        {
            var pointsList = new List<Vector2>(points);
            PurgeDuplicates(pointsList);
            var sumAngles = EnumerateSegments(pointsList)
                .Select(x => x.ToArrowVector()).SequentialPairs(true)
                .Sum(x => SignedAngleBetween(x.First, x.Second));
            if (sumAngles < 0)
                pointsList.Reverse();
            return pointsList.ToArray();
            // todo: check no self-crossing
        }

        private static float CalculateArea(IEnumerable<Vector2> points)
        {
            var remainingPoints = new List<Vector2>(points);
            if (remainingPoints.Count < 3)
                return 0;

            var area = 0f;
            while (remainingPoints.Count > 3)
            {
                for (var i = remainingPoints.Count - 3; i >= 0; i--)
                {
                    var p1 = remainingPoints[i];
                    var p2 = remainingPoints[i+1];
                    var p3 = remainingPoints[i+2];
                    var signedParallelogramArea = Vector2.Cross(p2 - p1, p3 - p2);
                    if (signedParallelogramArea >= 0)
                    {
                        area += signedParallelogramArea / 2;
                        remainingPoints.RemoveAt(i + 1);
                    }
                }
            }
            return area;
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
            FindClosestBorderPoint(point, out _, out var distanceSq, out var isInside);
            return isInside && distanceSq >= circleRadiusSq;
        }

        public Vector2 FindClosestValidPoint(Vector2 point)
        {
            FindClosestBorderPoint(point, out var closestBorderPoint, out var distanceSq, out var isInside);
            if (distanceSq >= circleRadiusSq - MathHelper.Eps5)
                return point;
            var directionSign = isInside ? 1f : -1f;
            var potentialClosestValidPoint = closestBorderPoint + directionSign * (point - closestBorderPoint).Normalize() * circleRadius;
            return FindClosestValidPoint(potentialClosestValidPoint);
        }
    }
}