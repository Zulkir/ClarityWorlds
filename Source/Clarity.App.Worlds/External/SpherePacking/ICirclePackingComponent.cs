using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.External.SpherePacking
{
    public interface ICirclePackingComponent : ISceneNodeComponent
    {
        int Width { get; set; }
        int Height { get; set; }
        float CircleRadius { get; set; }
        float RandomFactor { get; set; }

        float Area { get; }
        int MaxCircles { get; }
        int CurrentNumCircles { get; }

        void ResetPacker();
        void OptimizeStep();
        void RunOptimization();
    }
}