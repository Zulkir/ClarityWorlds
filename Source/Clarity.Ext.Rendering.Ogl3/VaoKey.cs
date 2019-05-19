using System;
using ObjectGL.Api.Objects.Resources.Buffers;

namespace Clarity.Ext.Rendering.Ogl3
{
    public struct VaoKey : IEquatable<VaoKey>
    {
        public readonly IBuffer VertexBuffer;
        public readonly IBuffer IndexBuffer;

        public VaoKey(IBuffer vertexBuffer, IBuffer indexBuffer)
        {
            VertexBuffer = vertexBuffer;
            IndexBuffer = indexBuffer;
        }

        public bool Equals(VaoKey other) => 
            VertexBuffer == other.VertexBuffer &&
            IndexBuffer == other.IndexBuffer;

        public override bool Equals(object obj) => 
            obj is VaoKey && Equals((VaoKey)obj);

        public override int GetHashCode()
        {
            return
                (VertexBuffer?.GetHashCode() ?? 0) ^
                (IndexBuffer?.GetHashCode() ?? 0);
        }
    }
}