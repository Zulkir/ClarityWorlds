using Clarity.Common.Numericals;
using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Images;
using BlitFramebufferFilter = ObjectGL.Api.Context.Actions.BlitFramebufferFilter;
using ClearBufferMask = ObjectGL.Api.Context.Actions.ClearBufferMask;

namespace Clarity.Ext.Rendering.Ogl3.Helpers
{
    public class OffScreen : IOffScreen
    {
        private readonly IGraphicsInfra infra;
        private readonly IFramebuffer resolveFramebuffer;

        public IFramebuffer Framebuffer { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Samples { get; private set; }
        public IRenderbuffer ColorBuffer { get; private set; }
        public IRenderbuffer DepthBuffer { get; private set; }
        public ITexture2D ResolvedTex { get; private set; }

        public OffScreen(IGraphicsInfra infra)
        {
            this.infra = infra;
            Framebuffer = infra.GlContext.Create.Framebuffer();
            resolveFramebuffer = infra.GlContext.Create.Framebuffer();
        }

        public void Dispose()
        {
            resolveFramebuffer?.Dispose();
            Framebuffer?.Dispose();
            ColorBuffer?.Dispose();
            DepthBuffer?.Dispose();
            ResolvedTex?.Dispose();
        }

        public void Prepare(int width, int height, int samples)
        {
            if (Width == width && Height == height && Samples == samples)
                return;

            Width = width;
            Height = height;
            Samples = samples;

            Framebuffer.Detach(FramebufferAttachmentPoint.Color0);
            Framebuffer.Detach(FramebufferAttachmentPoint.DepthStencil);

            ColorBuffer?.Dispose();
            DepthBuffer?.Dispose();
            ResolvedTex?.Dispose();

            ColorBuffer = infra.GlContext.Create.Renderbuffer(width, height, Format.Srgb8Alpha8, samples);
            DepthBuffer = infra.GlContext.Create.Renderbuffer(width, height, Format.Depth24Stencil8, samples);
            ResolvedTex = infra.GlContext.Create.Texture2D(width, height, GraphicsHelper.TextureMipCount(width, height), Format.Srgb8Alpha8);

            Framebuffer.AttachRenderbuffer(FramebufferAttachmentPoint.Color0, ColorBuffer);
            Framebuffer.AttachRenderbuffer(FramebufferAttachmentPoint.DepthStencil, DepthBuffer);
        }

        public void Resolve()
        {
            resolveFramebuffer.AttachTextureImage(FramebufferAttachmentPoint.Color0, ResolvedTex, 0);
            infra.GlContext.Actions.BlitFramebuffer(Framebuffer, resolveFramebuffer,
                0, 0, Width, Height,
                0, 0, Width, Height,
                ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            resolveFramebuffer.Detach(FramebufferAttachmentPoint.Color0);
            ResolvedTex.GenerateMipmap();
        }
    }
}