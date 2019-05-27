using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3.Drawers 
{
    public interface IHighlightDrawer
    {
        void Draw(IFramebuffer targetFramebuffer, IRenderbuffer depthStencil);
    }
}