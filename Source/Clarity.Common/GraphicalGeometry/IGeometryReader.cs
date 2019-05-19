using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.OtherTuples;

namespace Clarity.Common.GraphicalGeometry
{
    public interface IGeometryReaderBase
    {
        ManifoldType ManifoldType { get; }
        string InternalPrimitiveTopology { get; }
        IVertexElementInfo GetVertexElemInfo(string semantic);
        void GetVertexData(IntPtr dest, IVertexElementInfo elemInfo, int vertexIndex);

        bool HasCommonSemantic(CommonVertexSemantic semantic);

        Vector3 GetPosition(int vertexIndex);
        Color4 GetColor(int vertexIndex);
        Vector3 GetTangent(int vertexIndex);
        Vector3 GetNormal(int vertexIndex);
        Vector3 GetBinormal(int vertexIndex);
        Vector2 GetTexCoord(int vertexIndex);
        IntVector4 GetBoneIndices4(int vertexIndex);
        Vector4 GetBoneWeights(int vertexIndex);
    }
}