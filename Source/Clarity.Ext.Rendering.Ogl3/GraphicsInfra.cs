using ObjectGL.Api.Context;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class GraphicsInfra : IGraphicsInfra
    {
        public IContext GlContext { get; }
        public ICommonObjects CommonObjects { get; }
        public IMainThreadDisposer MainThreadDisposer { get; }

        public GraphicsInfra(IContext glContext, IMainThreadDisposer mainThreadDisposer, ICommonObjects commonObjects)
        {
            GlContext = glContext;
            MainThreadDisposer = mainThreadDisposer;
            CommonObjects = commonObjects;
        }
    }
}