using Clarity.Common.Numericals.Colors;

namespace Clarity.Ext.Rendering.Ogl3
{
    public static class Color4Extensions
    {
        public static ObjectGL.Api.Color4 ToOgl(this Color4 color)
        {
            return new ObjectGL.Api.Color4(color.R, color.G, color.B, color.A);
        }
    }
}