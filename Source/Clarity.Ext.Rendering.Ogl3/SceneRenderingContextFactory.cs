using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class SceneRenderingContextFactory : ISceneRenderingContextFactory
    {
        private readonly IGraphicsInfra graphicsInfra;

        public SceneRenderingContextFactory(IGraphicsInfra graphicsInfra)
        {
            this.graphicsInfra = graphicsInfra;
        }

        public ISceneRenderingContext Create(ISceneRenderingContext parentContext, ICamera camera, float aspectRatio)
        {
            // todo: create config for extensions
            var stages = new IRenderStage[]
            {
                new RenderStage(StandardRenderStageNames.Opaque, new StandardOpaqueRenderQueue()),
                new RenderStage(StandardRenderStageNames.Transparent, new StandardTransparentRenderQueue()),
            };
            return new SceneRenderingContext(parentContext, graphicsInfra, stages, camera, aspectRatio);
        }
    }
}