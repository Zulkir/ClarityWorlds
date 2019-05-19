using Clarity.Engine.Media.Skyboxes;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IDrawableTextureCubeFactory
    {
        IDrawableTextureCube Create(ISkybox clTexture);
    }
}