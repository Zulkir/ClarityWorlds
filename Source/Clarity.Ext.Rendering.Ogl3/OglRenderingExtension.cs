using Clarity.Common.Infra.Di;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class OglRenderingExtension : IExtension
    {
        public string Name => "Rendering.Ogl3";

        public void Bind(IDiContainer di)
        {
            di.Bind<IRenderService>().To<RenderService>();
            di.Bind<IRenderingRuntime>().To<RenderingRuntime>();
            di.Bind<IDrawableTextureCubeFactory>().To<DrawableTextureCubeFactory>();
            di.Bind<IShaderProgramFactory>().To<ShaderProgramFactory>();
            di.Bind<ISketchDrawer>().To<SketchDrawer>();
            di.Bind<ISceneRenderingContextFactory>().To<SceneRenderingContextFactory>();
            di.Bind<ICommonObjects>().To<CommonObjects>();
            di.Bind<IVisualElementHandlerChoices>().To<VisualElementHandlerChoices>();
            di.BindMulti<IVisualElementHandler>().To<CgModelVisualElementHandler>();
        }

        public void OnStartup(IDiContainer di)
        {
            di.Get<IRenderService>();
        }
    }
}