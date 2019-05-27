using System;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Visualization.Elements.Samplers;
using ObjectGL.Api.Objects.Samplers;

namespace Clarity.Ext.Rendering.Ogl3
{
    public static class Converters
    {
        public static ObjectGL.Api.Color4 ToOgl(this Color4 color)
        {
            return new ObjectGL.Api.Color4(color.R, color.G, color.B, color.A);
        }

        public static TextureMagFilter ToOglMag(this ImageSamplerFilter c)
        {
            switch (c)
            {
                case ImageSamplerFilter.Nearest: return TextureMagFilter.Nearest;
                case ImageSamplerFilter.Linear: return TextureMagFilter.Linear;
                default: throw new ArgumentOutOfRangeException(nameof(c), c, null);
            }
        }

        public static TextureMinFilter ToOglMin(ImageSamplerFilter cMin, ImageSamplerFilter cMip)
        {
            switch (cMin)
            {
                case ImageSamplerFilter.Nearest:
                    switch (cMip)
                    {
                        case ImageSamplerFilter.Nearest: return TextureMinFilter.NearestMipmapNearest;
                        case ImageSamplerFilter.Linear: return TextureMinFilter.NearestMipmapLinear;
                        default: throw new ArgumentOutOfRangeException(nameof(cMip), cMip, null);
                    }
                case ImageSamplerFilter.Linear: 
                    switch (cMip)
                    {
                        case ImageSamplerFilter.Nearest: return TextureMinFilter.LinearMipmapNearest;
                        case ImageSamplerFilter.Linear: return TextureMinFilter.LinearMipmapLinear;
                        default: throw new ArgumentOutOfRangeException(nameof(cMip), cMip, null);
                    }
                default: throw new ArgumentOutOfRangeException(nameof(cMin), cMin, null);
            }
        }

        public static TextureWrapMode ToOgl(this ImageSamplerAddressMode c)
        {
            switch (c)
            {
                case ImageSamplerAddressMode.Repeat: return TextureWrapMode.Repeat;
                case ImageSamplerAddressMode.Mirror: return TextureWrapMode.MirroredRepeat;
                case ImageSamplerAddressMode.ClampToEdge: return TextureWrapMode.ClampToEdge;
                default: throw new ArgumentOutOfRangeException(nameof(c), c, null);
            }
        }
    }
}