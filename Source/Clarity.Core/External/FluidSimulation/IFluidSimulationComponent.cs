using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.External.FluidSimulation
{
    public interface IFluidSimulationComponent : ISceneNodeComponent
    {
        int Width { get; set; }
        int Height { get; set; }
        float CellSize { get; set; }
        int LevelSetScale { get; set; }
        FluidSurfaceType SurfaceType { get; set; }
    }
}