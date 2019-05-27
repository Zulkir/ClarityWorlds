using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Visualization.Graphics;
using IritNet;

namespace Clarity.Ext.Format.Itd
{
    public unsafe static class IritToClarity
    {
        public static Vector3 Convert(IrtPtType* pPoint) => new Vector3((float)pPoint->Values[0], (float)pPoint->Values[1], (float)pPoint->Values[2]);
        public static Vector3 Convert(IrtNrmlType* pPoint) => new Vector3((float)pPoint->Values[0], (float)pPoint->Values[1], (float)pPoint->Values[2]);

        public static VertexPosNormTex Convert(IPVertexStruct* pVertex)
        {
            var pTexCoords = Irit.AttrGetUVAttrib(pVertex->Attr, IritStrings.uvvals);
            return new VertexPosNormTex(
                Convert(&pVertex->Coord),
                Convert(&pVertex->Normal),
                pTexCoords != (void*)0 ? *(Vector2*)pTexCoords : Vector2.Zero);
        }

        public static Vector3 ToClarity(this IrtPtType point) => Convert(&point);

        public static VertexPosNormTex GetClarityVertex(this MCPolygonStruct mcPolygon, int index, Vector2 texCoord) =>
            new VertexPosNormTex
            {
                Position = mcPolygon.V[index].ToClarity(),
                Normal = mcPolygon.N[index].ToClarity(),
                TexCoord = texCoord
            };
    }
}