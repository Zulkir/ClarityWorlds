using Clarity.Common.Numericals.Colors;

namespace Clarity.Engine.Media.Images
{
    public class SingleColorPixelSource : ISingleColorPixelSource
    {
        public Color4 Color { get; }
        public bool HasTransparency => Color.A < 1;

        public SingleColorPixelSource(Color4 color)
        {
            Color = color;
        }
    }
}