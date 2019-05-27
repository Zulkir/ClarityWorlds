using Clarity.Common.Numericals.Colors;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3.Drawers
{
    public interface IQuadDrawer
    {
        void Draw(Color4 color);
        void Draw(ITexture2D glTexture);
    }
}