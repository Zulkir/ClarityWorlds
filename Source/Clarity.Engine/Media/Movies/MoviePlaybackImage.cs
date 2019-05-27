using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Movies
{
    public class MoviePlaybackImage : ResourceBase, IImage
    {
        public IntSize2 Size { get; private set; }
        public byte[] RgbaData { get; private set; }

        public bool HasTransparency => false;

        public MoviePlaybackImage() : base(ResourceVolatility.Volatile)
        {
            Size = new IntSize2(1, 1);
            RgbaData = new byte[]{255, 255, 255, 255};
        }

        public byte[] GetRawData() => RgbaData;

        public void Update(IntSize2 size, byte[] data)
        {
            Size = size;
            RgbaData = data;
            OnModified(null);
        }
    }
}