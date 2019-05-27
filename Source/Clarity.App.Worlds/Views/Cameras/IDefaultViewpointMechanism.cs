using Clarity.Engine.Visualization.Cameras;

namespace Clarity.App.Worlds.Views.Cameras
{
    public interface IDefaultViewpointMechanism
    {
        ICamera FixedCamera { get; }
        ICamera CreateControlledViewpoint();
    }
}