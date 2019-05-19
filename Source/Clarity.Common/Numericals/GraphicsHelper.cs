using System;

namespace Clarity.Common.Numericals
{
    public static class GraphicsHelper
    {
        public const float MinZOffset = 3 * -1f / (1 << 18);
        public const float FrustumDistance = 2.414213562373095f;

        public static int TextureMipCount(int width, int height = 1, int depth = 1)
        {
            var mipSize = Math.Max(Math.Max(width, height), depth);
            var count = 0;
            while (mipSize > 0)
            {
                count++;
                mipSize /= 2;
            }
            return count;
        }

        public static int AlignedRowSpan(int width, int pixelSize = 4, int alignment = 16)
        {
            var rawRowSize = width * pixelSize;
            return rawRowSize % alignment != 0
                ? (rawRowSize / alignment + 1) * alignment
                : rawRowSize;
        }

        public static int AlignedSize(int width, int height, int pixelSize = 4, int alignment = 16)
        {
            return AlignedRowSpan(width, pixelSize, alignment) * height;
        }

        public static float AspectRatio(int width, int height)
        {
            return (float)Math.Max(width, 1) / Math.Max(height, 1);
        }
    }
}