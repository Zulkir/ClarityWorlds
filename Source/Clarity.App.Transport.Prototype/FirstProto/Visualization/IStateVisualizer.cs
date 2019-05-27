using Clarity.App.Transport.Prototype.FirstProto.Simulation;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Transport.Prototype.FirstProto.Visualization
{
    public interface IStateVisualizer
    {
        ISceneNode RootNode { get; }
        void UpdateToState(ISimState simState);
    }
}