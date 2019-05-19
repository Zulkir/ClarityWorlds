using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Objects.Caching;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Objects.WorldTree
{
    public abstract class SceneNode : AmObjectBase<SceneNode, ISceneNodeParent>, ISceneNode
    {
        public abstract int Id { get; set; }
        public abstract string Name { get; set; }
        public abstract Transform Transform { get; set; }
        public abstract IList<ISceneNodeComponent> Components { get; }
        public abstract IList<ISceneNode> ChildNodes { get; }

        public ICacheContainer CacheContainer { get; }

        public T GetComponent<T>() where T : class, ISceneNodeComponent  => Components.OfType<T>().Last();
        public T SearchComponent<T>() where T : class, ISceneNodeComponent => Components.OfType<T>().LastOrDefault();
        public IEnumerable<T> SearchComponents<T>() where T : class, ISceneNodeComponent => Components.OfType<T>();
        public bool HasComponent<T>() where T : class, ISceneNodeComponent => Components.OfType<T>().Any();

        public bool IsDescendantOf(ISceneNode node) => ParentNode != null && (ParentNode == node || ParentNode.IsDescendantOf(node));

        public Transform GlobalTransform => Transform * (ParentNode?.GlobalTransform ?? Transform.Identity);
        public ISceneNode Node => this;
        public IScene Scene => AmParent?.Scene;
        public ISceneNode AsNode => this;
        public ISceneNode ParentNode => AmParent?.AsNode;

        protected SceneNode()
        {
            CacheContainer = new CacheContainer();
            Transform = Transform.Identity;
        }
        
        public void Update(FrameTime frameTime)
        {
            foreach (var component in Components)
                component.Update(frameTime);
            foreach (var childNode in ChildNodes)
                childNode.Update(frameTime);
        }

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            foreach (var component in Components)
                component.OnNodeEvent(message);
            foreach (var cache in CacheContainer.GetAll())
                cache.OnMasterEvent(message);
        }

        public override string ToString() => Name ?? $"World Node {Id}";
    }
}