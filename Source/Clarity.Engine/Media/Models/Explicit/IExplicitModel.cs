using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.OtherTuples;

namespace Clarity.Engine.Media.Models.Explicit
{
    public interface IExplicitModel : IModel3D
    {
        Vector3[] Positions { get; }
        Vector3[] Normals { get; }
        Vector3[] Tangents { get; }
        Vector2[] TexCoords { get; }
        Color4[] Colors { get; }
        Vector4[] BoneWeights { get; }
        IntVector4[] BoneIndices { get; }

        Vector2[] TexCoordsSecondary { get; }
        Vector3[] TexCoordsCube { get; }
        Vector3[] TexCoords3D { get; }

        Vector4[] AbstractFloats0 { get; }
        Vector4[] AbstractFloats1 { get; }
        Vector4[] AbstractFloats2 { get; }
        Vector4[] AbstractFloats3 { get; }

        IntVector4[] AbstractInts0 { get; }
        IntVector4[] AbstractInts1 { get; }
        IntVector4[] AbstractInts2 { get; }
        IntVector4[] AbstractInts3 { get; }

        int[] Indices { get; }
        ExplicitModelIndexSubrange[] IndexSubranges { get; }
        ExplicitModelPrimitiveTopology Topology { get; }

        bool IndicesAreTrivial { get; }
        int MinIndexSize { get; }

        IEnumerable<Tuple3<int>> EnumerateTriangles();
        IExplicitModel GenerateTangents();
    }
}