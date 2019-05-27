using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction.Manipulation3D
{
    public interface ITransformable3DComponent : ISceneNodeComponent
    {
        Sphere LocalBoundingSphere { get; }
    }
}