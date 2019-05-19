using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Clarity.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Visualization.Graphics
{
    [TrwSerialize]
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = SizeInBytes)]
    public struct CgVertexPosNormTex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoord;

        public const int SizeInBytes = 32;

        public CgVertexPosNormTex(Vector3 position, Vector3 normal, Vector2 texCoord)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
        }

        public CgVertexPosNormTex(float px, float py, float pz, float nx, float ny, float nz, float tx, float ty)
        {
            Position = new Vector3(px, py, pz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(tx, ty);
        }

        private static readonly ConcurrentDictionary<int, VertexElementInfo[]> precreatedInfoArrays = new ConcurrentDictionary<int, VertexElementInfo[]>();
        public static unsafe IReadOnlyList<IVertexElementInfo> GetElementsInfos(int arrayIndex)
        {
            return precreatedInfoArrays.GetOrAdd(arrayIndex, x => new []
            {
                new VertexElementInfo(CommonVertexSemantic.Position, x, CommonFormat.R32G32B32_SFLOAT, 0, sizeof(CgVertexPosNormTex)),
                new VertexElementInfo(CommonVertexSemantic.Normal, x, CommonFormat.R32G32B32_SFLOAT, sizeof(Vector3), sizeof(CgVertexPosNormTex)),
                new VertexElementInfo(CommonVertexSemantic.TexCoord, x, CommonFormat.R32G32_SFLOAT, 2 * sizeof(Vector3), sizeof(CgVertexPosNormTex))
            });
        }
    }
}