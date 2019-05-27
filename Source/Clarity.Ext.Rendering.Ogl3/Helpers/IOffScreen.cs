using System;
using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3.Helpers 
{
    public interface IOffScreen : IDisposable
    {
        IFramebuffer Framebuffer { get; }
        int Width { get; }
        int Height { get; }
        int Samples { get; }
        IRenderbuffer ColorBuffer { get; }
        IRenderbuffer DepthBuffer { get; }
        ITexture2D ResolvedTex { get; }

        void Prepare(int width, int height, int samples);
        void Resolve();
    }
}