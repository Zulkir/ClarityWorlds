using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;
using Rectangle = System.Drawing.Rectangle;

namespace Clarity.App.Worlds.Assets
{
    public class SysDrawImage : ResourceBase, IImage
    {
        // todo: load file dynamically without keeping Image instance in memory
        private readonly Image image;

        public IntSize2 Size { get; }
        public bool HasTransparency { get; }

        public SysDrawImage(Image image)
            : base(ResourceVolatility.Immutable)
        {
            this.image = image;
            Size = new IntSize2(image.Width, image.Height);
            using (var bitmap = new Bitmap(image))
                HasTransparency = GetTransparency(bitmap);
        }

        public byte[] GetRawData()
        {
            using (var bitmap = new Bitmap(image))
                return GetDataFromBitmap(bitmap);
        }

        private static byte[] GetDataFromBitmap(Bitmap bitmap)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var raw = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var resultData = new byte[raw.Stride * raw.Height];
            for (int y = 0; y < raw.Height; y++)
            {
                var bitmapRowStart = raw.Scan0 + raw.Stride * y;
                Marshal.Copy(bitmapRowStart, resultData, raw.Stride * y, raw.Stride);
            }
            bitmap.UnlockBits(raw);
            return resultData;
        }

        private static unsafe bool GetTransparency(Bitmap bitmap)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var raw = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var result = false;
            for (int y = 0; y < raw.Height; y++)
            {
                var bitmapRowStart = (byte*)raw.Scan0 + raw.Stride * y;
                for (int x = 0; x < raw.Width; x++)
                    if ((bitmapRowStart + 4 * x)[3] != 255)
                        result = true;
            }
            bitmap.UnlockBits(raw);
            return result;
        }

        public override void Dispose()
        {
            base.Dispose();
            image.Dispose();
        }
    }
}