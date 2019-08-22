namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public class CirclePackingSolver
    {
        public CirclePackingSolvingProcess Solve(CirclePackingSolverSettings settings) => 
            new CirclePackingSolvingProcess(settings);
    }
}