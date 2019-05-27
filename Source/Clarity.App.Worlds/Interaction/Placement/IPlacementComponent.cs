using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction.Placement
{
    public interface IPlacementComponent : ISceneNodeComponent
    {
        IPlacementSurface PlacementSurface2D { get; }
        IPlacementSurface PlacementSurface3D { get; }
    }
}