using System.Collections.Concurrent;
using Clarity.App.Worlds.External.FluidSimulation;

namespace Clarity.Ext.Simulation.Fluids
{
    public interface IFluidSimulation
    {
        int NumCells { get; }
        INavierStokesGrid CurrentNavierStokesGrid { get; }
        FluidParticle[] Particles { get; }
        ConcurrentQueue<FluidSimulationFrame> FrameQueue { get; }
        float AvgLevelSetMassFor(int ai, int aj);
        LevelSet LevelSet { get; }
        void Run(float initialTimestamp);
        void Stop();
        void Reset(FluidSimulationConfig config);
    }
}