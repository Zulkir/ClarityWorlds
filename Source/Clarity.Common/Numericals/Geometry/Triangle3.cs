using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    public class Triangle3
    {
        public Vector3 V0;
        public Vector3 V1;
        public Vector3 V2;

        // Implements MollerTrumbore algorithm
        public Vector3? Intersect(Ray3 ray)
        {
            var edge1 = V1 - V0;
            var edge2 = V2 - V0;
            var h = Vector3.Cross(ray.Direction.Normalize(), edge2);
            var a = Vector3.Dot(edge1, h);
            if (a > -MathHelper.Eps8 && a < MathHelper.Eps8)
                return null;    // This ray is parallel to this triangle.
            var f = 1.0F / a;
            var s = ray.Point - V0;
            var u = f * (Vector3.Dot(s, h));
            if (u < 0.0 || u > 1.0)
                return null;
            var q = Vector3.Cross(s, edge1);
            var v = f * Vector3.Dot(ray.Direction, q);
            if (v < 0.0 || u + v > 1.0)
                return null;
            // At this stage we can compute t to find out where the intersection point is on the line.
            var t = f * Vector3.Dot(edge2, q);
            return t > MathHelper.Eps8 ? (Vector3?)(ray.Point + ray.Direction * t) : null;
        }
    }
}
