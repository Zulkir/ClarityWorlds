using System;

namespace Clarity.Engine.Visualization.Elements.RenderStates
{
    public struct StandardRenderStateData : IEquatable<StandardRenderStateData>
    {
        public CullFace CullFace;
        public PolygonMode PolygonMode;
        public float ZOffset;
        public float PointSize;
        public float LineWidth;

        public StandardRenderStateData(IStandardRenderState renderState)
        {
            CullFace = renderState.CullFace;
            PolygonMode = renderState.PolygonMode;
            ZOffset = renderState.ZOffset;
            PointSize = renderState.PointSize;
            LineWidth = renderState.LineWidth;
        }

        public bool Equals(StandardRenderStateData other)
        {
            return CullFace == other.CullFace && 
                   PolygonMode == other.PolygonMode && 
                   ZOffset.Equals(other.ZOffset) && 
                   PointSize.Equals(other.PointSize) && 
                   LineWidth.Equals(other.LineWidth);
        }

        public override bool Equals(object obj) => 
            obj is StandardRenderStateData data && Equals(data);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)CullFace;
                hashCode = (hashCode * 397) ^ (int)PolygonMode;
                hashCode = (hashCode * 397) ^ ZOffset.GetHashCode();
                hashCode = (hashCode * 397) ^ PointSize.GetHashCode();
                hashCode = (hashCode * 397) ^ LineWidth.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(StandardRenderStateData left, StandardRenderStateData right) => left.Equals(right);
        public static bool operator !=(StandardRenderStateData left, StandardRenderStateData right) => !left.Equals(right);
    }
}