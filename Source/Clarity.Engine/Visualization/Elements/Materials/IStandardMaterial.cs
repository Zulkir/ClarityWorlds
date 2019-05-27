using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Visualization.Elements.Samplers;

namespace Clarity.Engine.Visualization.Elements.Materials
{
    public interface IStandardMaterial : IMaterial
    {
        Color4 DiffuseColor { get; }
        IImage DiffuseMap { get; }
        IImage NormalMap { get; }
        IImageSampler Sampler { get; }
        bool IgnoreLighting { get; }
        bool NoSpecular { get; }

        // todo: remove or replace
        HighlightEffect HighlightEffect { get; }
        RtTransparencyMode RtTransparencyMode { get; }

        StandardMaterialData GetData();
        StandardMaterialImmutabilityFlags GetImmutability();
    }
}