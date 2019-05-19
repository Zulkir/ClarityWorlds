using System.Linq;
using Clarity.Core.AppCore.Views;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Objects.WorldTree.RenderStageDistribution;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Core.AppFeatures.StoryLayouts.NestedSpheres
{
    public class NestedSpheresRenderStageDistribution : IRenderStageDistribution
    {
        private readonly INavigationService navigationService;

        public NestedSpheresRenderStageDistribution(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public CgBasicRenderStage GetStage(ISceneNode node)
        {
            return node.EnumerateParents().Any(x => x == navigationService.Current) 
                ? CgBasicRenderStage.Focused 
                : CgBasicRenderStage.Blurred;
        }
    }
}