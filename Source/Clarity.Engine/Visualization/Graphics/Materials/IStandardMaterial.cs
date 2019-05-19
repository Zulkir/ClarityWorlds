using Clarity.Engine.Media.Images;
using JetBrains.Annotations;

namespace Clarity.Engine.Visualization.Graphics.Materials
{
    public interface IStandardMaterial : IMaterial
    {
        [NotNull]
        IPixelSource DiffuseTextureSource { get; }
        bool IgnoreLighting { get; }
        bool NoSpecular { get; }
        float PointSize { get; }
        float LineWidth { get; }
        bool Hide { get; }
    }
}