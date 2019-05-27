using Clarity.Common.Numericals.Colors;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3.Drawers
{
    public interface IBleedDrawer
    {
        void Draw(ITexture2D resolvedTexture, Color4 color);
    }
}