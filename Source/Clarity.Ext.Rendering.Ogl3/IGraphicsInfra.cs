using ObjectGL.Api.Context;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IGraphicsInfra
    {
        IContext GlContext { get; }
        IMainThreadDisposer MainThreadDisposer { get; }
    }
}