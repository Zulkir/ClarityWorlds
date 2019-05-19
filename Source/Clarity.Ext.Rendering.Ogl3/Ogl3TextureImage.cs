using System;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Resources;
using ObjectGL.Api.Objects.Resources.Images;
using OpenTK.Graphics.OpenGL4;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class Ogl3TextureImage : ResourceBase, IOgl3TextureImage
    {
        private readonly IGraphicsInfra infra;

        public IntSize2 Size { get; }
        public bool HasTransparency { get; }
        public ITexture2D GlTexture { get; }

        public Ogl3TextureImage(ResourceVolatility volatility, IGraphicsInfra infra, IntSize2 size, bool hasTransparency) 
            : base(volatility)
        {
            this.infra = infra;
            Size = size;
            HasTransparency = hasTransparency;
            // todo: to sRGB format
            GlTexture = infra.GlContext.Create.Texture2D(size.Width, size.Height,
                GraphicsHelper.TextureMipCount(size.Width, size.Height), Format.Rgba8);
        }

        public unsafe byte[] GetRawData()
        {
            infra.GlContext.Bindings.Textures.Units[infra.GlContext.Bindings.Textures.EditingIndex].Set(GlTexture);
            var result = new byte[GraphicsHelper.AlignedSize(Size.Width, Size.Height)];
            fixed (byte* pResult = result)
                infra.GlContext.GL.GetTexImage((int)GlTexture.Target, 0, (int)All.Rgba, (int)All.UnsignedByte, (IntPtr)pResult);
            return result;
        }

        public override void Dispose()
        {
            base.Dispose();
            infra.MainThreadDisposer.Add(GlTexture);
        }
    }
}