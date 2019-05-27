using System;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Visualization.Elements.Samplers;

namespace Clarity.Engine.Visualization.Elements.Materials
{
    public struct StandardMaterialData : IEquatable<StandardMaterialData>
    {
        public Color4 DiffuseColor;
        public IImage DiffuseMap;
        public IImage NormalMap;
        public IImageSampler Sampler;
        public bool IgnoreLighting;
        public bool NoSpecular;
        public bool HasTransparency;
        public HighlightEffect HighlightEffect;
        public RtTransparencyMode RtTransparencyMode;

        public StandardMaterialData(IStandardMaterial material)
        {
            DiffuseColor = material.DiffuseColor;
            DiffuseMap = material.DiffuseMap;
            NormalMap = material.NormalMap;
            Sampler = material.Sampler;
            IgnoreLighting = material.IgnoreLighting;
            NoSpecular = material.NoSpecular;
            HasTransparency = material.HasTransparency;
            HighlightEffect = material.HighlightEffect;
            RtTransparencyMode = material.RtTransparencyMode;
        }

        public bool Equals(StandardMaterialData other)
        {
            return DiffuseColor.Equals(other.DiffuseColor) && 
                   Equals(DiffuseMap, other.DiffuseMap) && 
                   Equals(NormalMap, other.NormalMap) && 
                   Equals(Sampler, other.Sampler) && 
                   IgnoreLighting == other.IgnoreLighting && 
                   NoSpecular == other.NoSpecular && 
                   HasTransparency == other.HasTransparency && 
                   HighlightEffect == other.HighlightEffect && 
                   RtTransparencyMode == other.RtTransparencyMode;
        }

        public override bool Equals(object obj) => obj is StandardMaterialData data && Equals(data);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = DiffuseColor.GetHashCode();
                hashCode = (hashCode * 397) ^ (DiffuseMap?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (NormalMap?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Sampler?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ IgnoreLighting.GetHashCode();
                hashCode = (hashCode * 397) ^ NoSpecular.GetHashCode();
                hashCode = (hashCode * 397) ^ HasTransparency.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)HighlightEffect;
                hashCode = (hashCode * 397) ^ (int)RtTransparencyMode;
                return hashCode;
            }
        }

        public static bool operator ==(StandardMaterialData left, StandardMaterialData right) => left.Equals(right);
        public static bool operator !=(StandardMaterialData left, StandardMaterialData right) => !left.Equals(right);
    }
}