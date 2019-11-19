using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LineSegment2 // todo: IEquatable<>
    {
        public Vector2 Point1;
        public Vector2 Point2;

        public LineSegment2(Vector2 point1, Vector2 point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public Vector2 ToArrowVector() => Point2 - Point1;
        public Line2 ToLine() => new Line2(Point1, Point2 - Point1);

        public Vector2? Intersect(Ray2 ray)
        {
            var lineIntersection = ToLine().Intersect(ray.ToLine());
            if (!lineIntersection.HasValue)
                return null;
            if (Vector2.Dot(lineIntersection.Value - Point1, Point2 - lineIntersection.Value) > 0 &&
                Vector2.Dot(lineIntersection.Value - ray.Point, ray.Direction) > 0)
                return lineIntersection.Value;
            return null;
        }

        public Vector2? Intersect(LineSegment2 other)
        {
            var lineIntersection = ToLine().Intersect(other.ToLine());
            if (!lineIntersection.HasValue)
                return null;
            if (Vector2.Dot(lineIntersection.Value - Point1, Point2 - lineIntersection.Value) > 0 &&
                Vector2.Dot(lineIntersection.Value - other.Point1, other.Point2 - lineIntersection.Value) > 0)
                return lineIntersection.Value;
            return null;
        }
    }
}