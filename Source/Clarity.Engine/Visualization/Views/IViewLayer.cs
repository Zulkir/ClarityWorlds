using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using JetBrains.Annotations;

namespace Clarity.Engine.Visualization.Views 
{
    public interface IViewLayer
    {
        [NotNull]
        IScene VisibleScene { get; }
        ICamera Camera { get; }
    }
}