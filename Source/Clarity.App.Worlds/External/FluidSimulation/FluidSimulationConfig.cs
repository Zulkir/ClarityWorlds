using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.App.Worlds.External.FluidSimulation
{
    [TrwSerialize]
    public class FluidSimulationConfig
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float CellSize { get; set; }
        public int LevelSetScale { get; set; }
        public FluidSurfaceType SurfaceType { get; set; }
    }
}