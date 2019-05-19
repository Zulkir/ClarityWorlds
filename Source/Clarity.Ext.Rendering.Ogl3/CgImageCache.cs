using System;
using Clarity.Common.CodingUtilities.Unmanaged;
using Clarity.Common.Numericals;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.Caching;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class CgImageCache : AutoDisposableBase, ICache
    {
        private readonly IGraphicsInfra infra;
        private readonly IImage image;
        private ITexture2D glTexture2D;
        private bool isDirty;

        public CgImageCache(IGraphicsInfra infra, IImage image)
        {
            this.image = image;
            this.infra = infra;
            isDirty = true;
        }

        public void OnMasterEvent(object eventArgs)
        {
            isDirty = true;
        }

        public unsafe ITexture2D GetGlTexture2D()
        {
            if (image is Ogl3TextureImage oglCgImage)
                return oglCgImage.GlTexture;

            if (!isDirty)
                return glTexture2D;

            var size = image.Size;
            if (glTexture2D == null || glTexture2D.Width != size.Width || glTexture2D.Height != size.Height)
            {
                glTexture2D?.Dispose();
                var mipCount = GraphicsHelper.TextureMipCount(size.Width, size.Height);
                // todo: choose more appropriate format (sRGB?)
                glTexture2D = infra.GlContext.Create.Texture2D(size.Width, size.Height, mipCount, Format.Rgba8);
            }

            fixed (byte* pRawData = image.GetRawData())
                glTexture2D.SetData(0, (IntPtr)pRawData, FormatColor.Bgra, FormatType.UnsignedByte);
            glTexture2D.GenerateMipmap();
            isDirty = false;

            return glTexture2D;
        }

        protected override void Dispose(bool explicitly)
        {
            infra.MainThreadDisposer.Add(glTexture2D);
        }
    }
}