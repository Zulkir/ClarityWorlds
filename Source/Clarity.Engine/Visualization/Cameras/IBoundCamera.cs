using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Visualization.Cameras
{
    public interface IBoundCamera : ICamera
    {
        ISceneNode Node { get; }
        CameraFrame GetLocalFrame();
    }
}