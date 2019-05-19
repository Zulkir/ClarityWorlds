using Clarity.Common.Numericals.Colors;
using ObjectGL.Api.Context;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IDrawCall
    {
        void Issue(IContext glContext);
        Color4? OverwriteColor { get; }
    }
}