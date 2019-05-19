using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Platforms;
using JitsuGen.Core;

namespace Clarity.Engine.Objects.WorldTree
{
    [JitsuGenIgnore]
    public abstract class SceneNodeComponentBase<TSelf> : AmObjectBase<TSelf, ISceneNodeBound>, ISceneNodeComponent
        where TSelf : ISceneNodeComponent
    {
        public ISceneNode Node => AmParent?.Node;
        
        public virtual void Update(FrameTime frameTime) { }
        public virtual void OnNodeEvent(IAmEventMessage message) { }
    }
}