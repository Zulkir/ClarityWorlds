using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Objects.WorldTree.RenderStageDistribution
{
    public interface IRenderStageDistribution
    {
        CgBasicRenderStage GetStage(ISceneNode node);
    }
}