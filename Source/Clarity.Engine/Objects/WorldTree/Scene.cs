using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Objects.WorldTree.RenderStageDistribution;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Objects.WorldTree
{
    public abstract class Scene : AmObjectBase<Scene>, IScene
    {
        public abstract string Name { get; set; }
        public abstract Color4 BackgroundColor { get; set; }
        public abstract ISkybox Skybox { get; set; }
        public abstract ISceneNode Root { get; set; }
        public IRenderStageDistribution RenderStageDistribution { get; set; }

        IScene ISceneNodeParent.Scene => this;

        public ISceneNode AsNode => null;

        protected Scene()
        {
            BackgroundColor = Color4.Magenta;
            RenderStageDistribution = new FocusedOnlyRenderStageDistribution();
        }

        public static Scene Create(ISceneNode root)
        {
            var self = AmFactory.Create<Scene>();
            self.Root = root;
            return self;
        }

        public void Update(FrameTime frameTime)
        {
            Root.Update(frameTime);
        }
    }
}