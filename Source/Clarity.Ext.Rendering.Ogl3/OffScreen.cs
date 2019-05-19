using Clarity.Common.Numericals;
using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Images;
using BlitFramebufferFilter = ObjectGL.Api.Context.Actions.BlitFramebufferFilter;
using ClearBufferMask = ObjectGL.Api.Context.Actions.ClearBufferMask;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class OffScreen
    {
        private readonly IGraphicsInfra infra;
        private readonly IFramebuffer framebuffer;
        private readonly IFramebuffer resolveFramebuffer;
        private IRenderbuffer colorBuff;
        private IRenderbuffer depthBuff;
        private ITexture2D resolvedTex;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Samples { get; private set; }

        public IFramebuffer Framebuffer => framebuffer;
        public ITexture2D ResolvedTex => resolvedTex;

        public OffScreen(IGraphicsInfra infra)
        {
            this.infra = infra;
            framebuffer = infra.GlContext.Create.Framebuffer();
            resolveFramebuffer = infra.GlContext.Create.Framebuffer();
        }

        public void Prepare(int width, int height, int samples)
        {
            if (Width == width && Height == height && Samples == samples)
                return;

            Width = width;
            Height = height;
            Samples = samples;

            framebuffer.Detach(FramebufferAttachmentPoint.Color0);
            framebuffer.Detach(FramebufferAttachmentPoint.DepthStencil);

            colorBuff?.Dispose();
            depthBuff?.Dispose();
            resolvedTex?.Dispose();

            colorBuff = infra.GlContext.Create.Renderbuffer(width, height, Format.Rgba8, samples);
            depthBuff = infra.GlContext.Create.Renderbuffer(width, height, Format.Depth24Stencil8, samples);
            resolvedTex = infra.GlContext.Create.Texture2D(width, height, GraphicsHelper.TextureMipCount(width, height), Format.Rgba8);

            framebuffer.AttachRenderbuffer(FramebufferAttachmentPoint.Color0, colorBuff);
            framebuffer.AttachRenderbuffer(FramebufferAttachmentPoint.DepthStencil, depthBuff);
        }

        public void Resolve()
        {
            resolveFramebuffer.AttachTextureImage(FramebufferAttachmentPoint.Color0, resolvedTex, 0);
            infra.GlContext.Actions.BlitFramebuffer(framebuffer, resolveFramebuffer,
                0, 0, Width, Height,
                0, 0, Width, Height,
                ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            resolveFramebuffer.Detach(FramebufferAttachmentPoint.Color0);
            resolvedTex.GenerateMipmap();
        }
    }
}