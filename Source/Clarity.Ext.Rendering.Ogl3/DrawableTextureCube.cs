using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class DrawableTextureCube : IDrawableTextureCube
    {
        public ITextureCubemap GlTextureCube { get; }

        public DrawableTextureCube(ITextureCubemap glTextureCube)
        {
            GlTextureCube = glTextureCube;
        }
    }
}