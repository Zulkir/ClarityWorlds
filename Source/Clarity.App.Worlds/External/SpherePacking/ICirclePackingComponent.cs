using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.External.SpherePacking
{
    public interface ICirclePackingComponent : ISceneNodeComponent
    {
        string ShapeName { get; set; }
        float CircleRadius { get; set; }
        int MaxInitialCircles { get; set; }
        float RandomFactor { get; set; }
        int NumIterationPerBreak { get; set; }
        int BatchSize { get; set; }

        float Area { get; }
        int MaxCircles { get; }
        int CurrentNumCircles { get; }
        int CurrentIterationNumber { get; }

        void ResetPacker();
        void OptimizeStep();
        void RunOptimization();
        void StopOptimization();
        void DeleteCircle();
    }
}