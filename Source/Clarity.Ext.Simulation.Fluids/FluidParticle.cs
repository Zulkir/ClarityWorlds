using Clarity.Common.Numericals.Algebra;

namespace Clarity.Ext.Simulation.Fluids
{
    public struct FluidParticle
    {
        public Vector3 Position;
        public float Mass;
        public float TimeToLive;

        public override string ToString()
        {
            return $"Pos: {Position.X:F3}, {Position.Y:F3}, {Position.Z:F3}, M: {Mass}";
        }
    }
}