using Clarity.Common.Infra.ActiveModel;

namespace Clarity.Engine.Objects.WorldTree
{
    public interface ISceneNodeBound : IAmObject
    {
        ISceneNode Node { get; }
    }
}