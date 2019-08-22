using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.External.SpherePacking
{
    public interface ICirclePackingAutoComponent : ISceneNodeComponent
    {
        string ShapeName { get; set; }
        float CircleRadius { get; set; }
        float Precision { get; set; }

        int MaxIterationsPerAttempt { get; set; }
        int CostDecreaseGracePeriod { get; set; }
        int ShakeIterations { get; set; }
        float ShakeStrength { get; set; }
        float MinCostDecrease { get; set; }

        int AttemptsPerRefresh { get; set; }

        void Reset();
        void Run();
        void Stop();
    }
}