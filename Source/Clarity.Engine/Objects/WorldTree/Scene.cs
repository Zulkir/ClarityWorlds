using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Objects.WorldTree
{
    public abstract class Scene : AmObjectBase<Scene, IWorld>, IScene
    {
        
        public abstract string Name { get; set; }
        public abstract Color4 BackgroundColor { get; set; }
        public abstract ISkybox Skybox { get; set; }
        public abstract ISceneNode Root { get; set; }
        public IList<ISceneNode> AuxuliaryNodes { get; }

        public IWorld World => AmParent;
        IScene ISceneNodeParent.Scene => this;

        public ISceneNode AsNode => null;

        protected Scene()
        {
            BackgroundColor = Color4.Magenta;
            AuxuliaryNodes = new List<ISceneNode>();
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
            foreach (var auxuliaryNode in AuxuliaryNodes)
                auxuliaryNode.Update(frameTime);
        }

        public void OnRoutedEvent(IRoutedEvent evnt)
        {
            Root.OnRoutedEvent(evnt);
            foreach (var auxuliaryNode in AuxuliaryNodes)
                auxuliaryNode.OnRoutedEvent(evnt);
        }
    }
}