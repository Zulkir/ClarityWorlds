using System.Runtime.InteropServices;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;
using Eto.Drawing;

namespace Clarity.Ext.Gui.EtoForms
{
    public class FromEtoImage : ResourceBase, IImage
    {
        public Image EtoImage { get; }

        public IntSize2 Size { get; }
        public bool HasTransparency { get; }

        public FromEtoImage(Image eImage, ResourceVolatility volatility) : base(volatility)
        {
            EtoImage = eImage;
            Size = EtoImage.Size.ToClarity();
            HasTransparency = GraphicsHelper.HasTransparency(Size, GetRawData());
        }

        public FromEtoImage(IntSize2 size, byte[] srgbaData, ResourceVolatility volatility) : 
            this(EtoBitmapFromRaw(size, srgbaData), volatility)
        {
        }

        public byte[] GetRawData()
        {
            return GetRawData(EtoImage);
        }

        public static unsafe byte[] GetRawData(Image eImage)
        {
            var alignedRowSpan = GraphicsHelper.AlignedRowSpan(eImage.Size.Width, 4, 1);
            var data = new byte[alignedRowSpan * eImage.Size.Height];

            var bitmap = new Bitmap(eImage);
            using (var bitmapData = bitmap.Lock())
            {
                if (!bitmapData.Flipped & bitmapData.BytesPerPixel == 4 && bitmapData.ScanWidth == alignedRowSpan)
                {
                    Marshal.Copy(bitmapData.Data, data, 0, data.Length);
                }
                else
                {
                    fixed (byte* pData = data)
                    {
                        for (var y = 0; y < eImage.Size.Height; y++)
                        {
                            var yf = bitmapData.Flipped ? eImage.Size.Height - 1 - y : y;
                            var dstRowOffset = y * alignedRowSpan;
                            for (var x = 0; x < eImage.Size.Width; x++)
                            {
                                var color = bitmapData.GetPixel(x, yf);
                                var pDst = pData + dstRowOffset + x * 4;
                                pDst[0] = (byte)color.Rb;
                                pDst[1] = (byte)color.Gb;
                                pDst[2] = (byte)color.Bb;
                                pDst[3] = (byte)color.Ab;
                            }
                        }
                    }
                }
            }
            return data;
        }

        public static Bitmap EtoBitmapFromRaw(IntSize2 size, byte[] srgbaData)
        {
            var bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgba);
            var alignedRowSpan = GraphicsHelper.AlignedRowSpan(size.Width, 4, 1);
            using (var bitmapData = bitmap.Lock())
            {
                for (var y = 0; y < size.Height; y++)
                {
                    var dataOffset = y * alignedRowSpan;
                    var yf = bitmapData.Flipped ? size.Height - 1 - y : y;
                    var bitmapOffset = yf * bitmapData.ScanWidth;
                    Marshal.Copy(srgbaData, dataOffset, bitmapData.Data + bitmapOffset, 4 * size.Width);
                }
            }
            return bitmap;
        }
    }
}