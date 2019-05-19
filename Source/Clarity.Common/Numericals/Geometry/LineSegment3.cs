using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LineSegment3 // todo: IEquatable<>
    {
        public Vector3 Point1;
        public Vector3 Point2;

        public LineSegment3(Vector3 point1, Vector3 point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public float Length => Difference.Length();
        public float LengthSq => Difference.LengthSquared();

        public Vector3 Difference => Point2 - Point1;

        public override string ToString() => $"{{{Point1}, {Point2}}}";
    }
}