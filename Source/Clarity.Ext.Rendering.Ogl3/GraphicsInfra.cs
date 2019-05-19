using ObjectGL.Api.Context;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class GraphicsInfra : IGraphicsInfra
    {
        public IContext GlContext { get; }
        public IMainThreadDisposer MainThreadDisposer { get; }

        public GraphicsInfra(IContext glContext, IMainThreadDisposer mainThreadDisposer)
        {
            GlContext = glContext;
            MainThreadDisposer = mainThreadDisposer;
        }
    }
}