using Clarity.App.Transport.Prototype.Simulation;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Transport.Prototype.Visualization
{
    public interface IStateVisualizer
    {
        ISceneNode RootNode { get; }
        void UpdateToState(ISimState simState);
    }
}