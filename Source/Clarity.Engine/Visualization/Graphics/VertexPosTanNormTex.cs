using System.Collections.Concurrent;
using System.Collections.Generic;
using Clarity.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Visualization.Graphics
{
    public struct VertexPosTanNormTex
    {
        public Vector3 Position;
        public Vector3 Tangent;
        public Vector3 Normal;
        public Vector2 TexCoord;
        public float AbstractFloat;

        public const int SizeInBytes = 48;

        public VertexPosTanNormTex(Vector3 position, Vector3 tangent, Vector3 normal, Vector2 texCoord, float abstractFloat = 0)
        {
            Position = position;
            Tangent = tangent;
            Normal = normal;
            TexCoord = texCoord;
            AbstractFloat = abstractFloat;
        }

        private static readonly ConcurrentDictionary<int, VertexElementInfo[]> PrecreatedInfoArrays = new ConcurrentDictionary<int, VertexElementInfo[]>();
        public static unsafe IReadOnlyList<IVertexElementInfo> GetElementsInfos(int arrayIndex)
        {
            return PrecreatedInfoArrays.GetOrAdd(arrayIndex, x => new []
            {
                new VertexElementInfo(CommonVertexSemantic.Position, x, CommonFormat.R32G32B32_SFLOAT, 0, sizeof(VertexPosTanNormTex)),
                new VertexElementInfo(CommonVertexSemantic.Tangent, x, CommonFormat.R32G32B32_SFLOAT, 3 * sizeof(float), sizeof(VertexPosTanNormTex)),
                new VertexElementInfo(CommonVertexSemantic.Normal, x, CommonFormat.R32G32B32_SFLOAT, 6 * sizeof(float), sizeof(VertexPosTanNormTex)),
                new VertexElementInfo(CommonVertexSemantic.TexCoord, x, CommonFormat.R32G32_SFLOAT, 9 * sizeof(float), sizeof(VertexPosTanNormTex)),
                new VertexElementInfo(CommonVertexSemantic.AbstractFloats0, x, CommonFormat.R32_SFLOAT, 11 * sizeof(float), sizeof(VertexPosTanNormTex))
            });
        }
    }
}