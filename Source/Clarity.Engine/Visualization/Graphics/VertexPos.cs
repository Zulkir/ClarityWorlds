using System.Collections.Concurrent;
using System.Collections.Generic;
using Clarity.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Visualization.Graphics
{
    public struct VertexPos
    {
        public Vector3 Position;
        public float AbstractFloat;

        public const int SizeInBytes = 16;

        public VertexPos(Vector3 position, float a = 0)
        {
            Position = position;
            AbstractFloat = a;
        }

        public VertexPos(float px, float py, float pz, float a = 0)
        {
            Position = new Vector3(px, py, pz);
            AbstractFloat = a;
        }

        private static readonly ConcurrentDictionary<int, VertexElementInfo[]> PrecreatedInfoArrays = new ConcurrentDictionary<int, VertexElementInfo[]>();
        public static unsafe IReadOnlyList<IVertexElementInfo> GetElementsInfos(int arrayIndex)
        {
            return PrecreatedInfoArrays.GetOrAdd(arrayIndex, x => new []
            {
                new VertexElementInfo(CommonVertexSemantic.Position, x, CommonFormat.R32G32B32_SFLOAT, 0, sizeof(VertexPos)),
                new VertexElementInfo(CommonVertexSemantic.AbstractFloats0, x, CommonFormat.R32_SFLOAT, sizeof(Vector3), sizeof(VertexPos)),
            });
        }
    }
}