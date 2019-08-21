namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public class CirclePackingSolver
    {
        public void Solve(CirclePackingSolverSettings settings) => 
            new CirclePackingSolvingProcess(settings).Run();
    }
}