using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface IDefaultViewpointMechanism
    {
        ICamera FixedCamera { get; }
        ICamera CreateControlledViewpoint();
    }
}