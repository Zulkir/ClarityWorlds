using System;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Assets.Scripts.Rendering.Materials
{
    public struct StandardMaterialKey : IEquatable<StandardMaterialKey>
    {
        // todo: any materials plus fallback

        public IStandardMaterial CStandardMaterial;
        public IStandardRenderState CStandardRenderState;

        public bool Equals(StandardMaterialKey other)
        {
            return Equals(CStandardMaterial, other.CStandardMaterial) &&
                   Equals(CStandardRenderState, other.CStandardRenderState);
        }

        public override bool Equals(object obj)
        {
            return obj is StandardMaterialKey && Equals((StandardMaterialKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = CStandardMaterial?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (CStandardRenderState?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}