using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Objects.Caching;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Objects.WorldTree
{
    public interface ISceneNode : IAmObject<ISceneNodeParent>, ISceneNodeBound, ISceneNodeParent
    {
        int Id { get; set; }
        string Name { get; set; }

        IList<ISceneNodeComponent> Components { get; }
        bool HasComponent<T>() where T : class, ISceneNodeComponent;
        T GetComponent<T>() where T : class, ISceneNodeComponent;
        T SearchComponent<T>() where T : class, ISceneNodeComponent;
        IEnumerable<T> SearchComponents<T>() where T : class, ISceneNodeComponent;

        IList<ISceneNode> ChildNodes { get; }
        ISceneNode ParentNode { get; }
        bool IsDescendantOf(ISceneNode node);

        Transform Transform { get; set; }
        Transform GlobalTransform { get; }

        ICacheContainer CacheContainer { get; }
        void Update(FrameTime frameTime);
        void OnRoutedEvent(IRoutedEvent evnt);
    }
}