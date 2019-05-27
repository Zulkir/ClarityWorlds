using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3.Drawers 
{
    public interface IBlurDrawer
    {
        void Draw(ITexture2D resolvedTexture);
    }
}