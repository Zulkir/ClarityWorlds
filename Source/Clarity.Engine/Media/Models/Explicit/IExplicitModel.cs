using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Media.Models.Explicit
{
    public interface IExplicitModel
    {
        Vector3[] Positions { get; }
        Vector3[] Normals { get; }
        Vector2[] TexCoords { get; }
        Vector3[] Tangents { get; }
        Vector3[] Binormals { get; }

        int[] Indices { get; }
        ExplicitModelIndexSubrange[] IndexSubranges { get; }
        ExplicitModelPrimitiveTopology Topology { get; }
    }
}