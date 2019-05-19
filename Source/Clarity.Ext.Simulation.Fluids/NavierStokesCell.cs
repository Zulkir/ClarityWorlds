using Clarity.Common.Numericals.Algebra;

namespace Clarity.Ext.Simulation.Fluids
{
    public struct NavierStokesCell
    {
        public Vector3 Velocity;
        public float Pressure;
        public float Mass;
        public NavierStokesCellState State;
        public bool IsSurface;

        public override string ToString()
        {
            return $"{State}, P: {Pressure:F3}, V: {Velocity.X:F3}, {Velocity.Y:F3}, {Velocity.Z:F3}, M: {Mass}";
        }
    }
}