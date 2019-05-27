using Clarity.Engine.Media.Images;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3.Implementations
{
    public interface IOgl3TextureImage : IImage
    {
        ITexture2D GlTexture { get; }
    }
}