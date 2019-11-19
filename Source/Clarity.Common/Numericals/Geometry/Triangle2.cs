using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    public struct Triangle2
    {
        public Vector2 P0;
        public Vector2 P1;
        public Vector2 P2;

        public Triangle2(Vector2 p0, Vector2 p1, Vector2 p2)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
        }
    }
}