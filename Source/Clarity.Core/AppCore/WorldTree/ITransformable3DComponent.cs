using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface ITransformable3DComponent : ISceneNodeComponent
    {
        float OwnRadius { get; }
    }
}