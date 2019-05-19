using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class DrawableTexture : IDrawableTexture
    {
        public ITexture2D GlTexture { get; }

        public DrawableTexture(ITexture2D glTexture)
        {
            GlTexture = glTexture;
        }
    }
}