namespace Clarity.Ext.Simulation.SpherePacking.CirclePacking
{
    public class CirclePackingSolverSettings
    {
        public float CircleRadius { get; set; }
        public float Precision { get; set; }
        public ICirclePackingBorder Border { get; set; }
        public int MaxIterationsPerAttempt { get; set; }
        public int CostDecreaseGracePeriod { get; set; }
        public int ShakeIterations { get; set; }
        public float ShakeStrength { get; set; }
        public float MinCostDecrease { get; set; }
        public int AttemptsPerCallback { get; set; }
    }
}