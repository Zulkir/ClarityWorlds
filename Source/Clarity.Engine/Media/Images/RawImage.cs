using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Images
{
    public class RawImage : ResourceBase, IImage
    {
        public IntSize2 Size { get; }
        public bool HasTransparency { get; }
        public byte[] RawData { get; }
        public byte[] GetRawData() => RawData;

        public RawImage(ResourceVolatility volatility, IntSize2 size, bool hasTransparency, byte[] rawData) 
            : base(volatility)
        {
            Size = size;
            HasTransparency = hasTransparency;
            RawData = rawData;
        }
    }
}