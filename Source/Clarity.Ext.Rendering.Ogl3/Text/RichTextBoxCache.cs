using System;
using Clarity.Common.CodingUtilities.Unmanaged;
using Clarity.Common.Numericals;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.Caching;
using ObjectGL.Api.Objects.Resources.Images;
using OpenTK.Graphics.OpenGL4;

namespace Clarity.Ext.Rendering.Ogl3.Text
{
    public class RichTextBoxCache : AutoDisposableBase, ICache
    {
        private readonly IGraphicsInfra infra;
        private readonly IRtImageBuilder rtImageBuilder;
        private readonly IRichTextBox textBox;
        private ITexture2D glTexture;
        private bool isDirty;


        public RichTextBoxCache(IGraphicsInfra infra, IRtImageBuilder rtImageBuilder, IRichTextBox textBox)
        {
            this.infra = infra;
            this.rtImageBuilder = rtImageBuilder;
            this.textBox = textBox;
            isDirty = true;
        }

        protected override void Dispose(bool explicitly)
        {
            infra.MainThreadDisposer.Add(glTexture);
        }

        public void OnMasterEvent(object eventArgs)
        {
            isDirty = true;
        }

        public unsafe ITexture2D GetGlTexture2D()
        {
            if (!isDirty)
                return glTexture;
            
            var size = textBox.Size;
            var width = size.Width;
            var height = size.Height;
            var mipCount = GraphicsHelper.TextureMipCount(width, height);

            if (glTexture == null || glTexture.Width != width || glTexture.Height != height)
            {
                glTexture?.Dispose();
                glTexture = infra.GlContext.Create.Texture2D(width, height, mipCount, Format.Rgba8);
            }

            if (width == 0 || height == 0)
            {
                isDirty = false;
                return glTexture;
            }

            var textImage = rtImageBuilder.BuildImage(textBox);
            fixed (byte* data = textImage.Data)
                glTexture.SetData(0, (IntPtr)data, FormatColor.Bgra, FormatType.UnsignedByte);
            // todo: to ObjectGL
            infra.GlContext.GL.GenerateMipmap((int)All.Texture2D);

            isDirty = false;
            return glTexture;
        }
    }
}