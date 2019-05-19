using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using JetBrains.Annotations;

namespace Clarity.Common.Shapes
{
    public class TriangleListShape : IShape3D
    {
        public IReadOnlyList<Vector3> Vertices { get; }
        public int[] Indices { get; }

        public TriangleListShape([NotNull] IReadOnlyList<Vector3> vertices, [NotNull] int[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }
}