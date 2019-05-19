using Clarity.Common.Numericals.Colors;

namespace Clarity.Engine.Media.Images
{
    public interface ISingleColorPixelSource : IPixelSource
    {
        Color4 Color { get; }
    }
}