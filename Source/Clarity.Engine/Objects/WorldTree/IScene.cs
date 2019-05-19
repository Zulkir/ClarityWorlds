using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Objects.WorldTree.RenderStageDistribution;
using Clarity.Engine.Platforms;
using JetBrains.Annotations;

namespace Clarity.Engine.Objects.WorldTree 
{
    public interface IScene : IAmObject, ISceneNodeParent
    {
        string Name { get; set; }
        Color4 BackgroundColor { get; set; }
        ISkybox Skybox { get; set; }
        [NotNull] IRenderStageDistribution RenderStageDistribution { get; set; }
        ISceneNode Root { get; set; }

        void Update(FrameTime frameTime);
    }
}