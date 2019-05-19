using Clarity.Common.Infra.ActiveModel;
using JetBrains.Annotations;

namespace Clarity.Engine.Objects.WorldTree
{
    public interface ISceneNodeParent : IAmObject
    {
        IScene Scene { get; }
        [CanBeNull]
        ISceneNode AsNode { get; }
    }
}