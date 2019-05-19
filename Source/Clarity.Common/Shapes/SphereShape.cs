using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Shapes
{
    public class SphereShape : IShape3D
    {
        public Vector3 Position { get; }
        public float Radius { get; }
    }
}