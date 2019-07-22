using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.Simulation.Fluids 
{
    public class FluidSimulationFrame
    {
        public float Timestamp { get; set; }
        public IntSize3 LeveSetSizeStokesSize { get; }
        public Vector3[] Particles { get; }
        public double[] Phi { get; }
        public bool[] ParticleMask { get; }
        public INavierStokesGrid NavierStokesGrid { get; }

        public FluidSimulationFrame(IntSize3 leveSetSizeStokesSize, int numParticles, IntSize3 navierStokesSize, float navierStokesCellSize)
        {
            LeveSetSizeStokesSize = leveSetSizeStokesSize;
            Particles = new Vector3[numParticles];
            Phi = new double[leveSetSizeStokesSize.Volume()];
            ParticleMask = new bool[leveSetSizeStokesSize.Volume()];
            NavierStokesGrid = new NavierStokesGrid(navierStokesSize, navierStokesCellSize);
        }
    }
}