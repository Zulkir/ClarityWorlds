using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Platforms;
using Clarity.Ext.Rendering.Ogl3.Caches;
using Clarity.Ext.Rendering.Ogl3.Drawers;
using Clarity.Ext.Rendering.Ogl3.Handlers;
using Clarity.Ext.Rendering.Ogl3.Helpers;
using Clarity.Ext.Rendering.Ogl3.Pipelining;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class OglRenderingExtension : IExtension
    {
        public string Name => "Rendering.Ogl3";

        public void Bind(IDiContainer di)
        {
            di.Bind<IRenderService>().To<RenderService>();
            di.Bind<IRenderingRuntime>().To<RenderingRuntime>();
            di.Bind<IShaderProgramFactory>().To<ShaderProgramFactory>();
            di.Bind<ISketchDrawer>().To<SketchDrawer>();
            di.Bind<ISkyboxDrawer>().To<SkyboxDrawer>();
            di.Bind<IVeilDrawer>().To<VeilDrawer>();
            di.Bind<IQuadDrawer>().To<QuadDrawer>();
            di.Bind<IBlurDrawer>().To<BlurDrawer>();
            di.Bind<IBleedDrawer>().To<BleedDrawer>();
            di.Bind<IHighlightDrawer>().To<HighlightDrawer>();
            di.Bind<IMainThreadDisposer>().AsLastChoice.To<MainThreadDisposer>();
            di.Bind<IGraphicsInfra>().AsLastChoice.To<GraphicsInfra>();
            di.Bind<ICommonObjects>().To<CommonObjects>();
            di.Bind<ISamplerCache>().To<SamplerCache>();
            di.Bind<IOffScreenContainer>().To<OffScreenContainer>();
            di.Bind<ISceneRendererPool>().To<SceneRendererPool>();
            di.Bind<ISceneRendererFactory>().To<SceneRendererFactory>();
            di.Bind<IVisualElementHandlerContainer>().To<VisualElementHandlerContainer>();
            di.BindMulti<IVisualElementHandler>().To<ModelVisualElementHandler>();
        }

        public void OnStartup(IDiContainer di)
        {
            di.Get<IRenderService>();
        }
    }
}