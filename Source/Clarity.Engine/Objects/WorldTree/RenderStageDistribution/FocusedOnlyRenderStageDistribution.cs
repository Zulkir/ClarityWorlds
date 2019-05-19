using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Objects.WorldTree.RenderStageDistribution 
{
    public class FocusedOnlyRenderStageDistribution : IRenderStageDistribution
    {
        public CgBasicRenderStage GetStage(ISceneNode node) => CgBasicRenderStage.Focused;
    }
}