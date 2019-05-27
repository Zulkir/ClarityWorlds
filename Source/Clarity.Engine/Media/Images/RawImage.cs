using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Images
{
    public class RawImage : ResourceBase, IImage
    {
        public IntSize2 Size { get; }
        public bool HasTransparency { get; private set; }
        public byte[] RawData { get; private set; }
        public byte[] GetRawData() => RawData;

        public RawImage(ResourceVolatility volatility, IntSize2 size, bool hasTransparency, byte[] rawData) 
            : base(volatility)
        {
            Size = size;
            HasTransparency = hasTransparency;
            RawData = rawData;
        }

        public RawImage(ResourceVolatility volatility, IntSize2 size, byte[] rawData)
            : base(volatility)
        {
            Size = size;
            RawData = rawData;
            HasTransparency = GraphicsHelper.HasTransparency(Size, RawData);
        }

        public void SetData(byte[] rawData, bool hasTransparency)
        {
            RawData = rawData;
            HasTransparency = hasTransparency;
            OnModified(null);
        }
    }
}