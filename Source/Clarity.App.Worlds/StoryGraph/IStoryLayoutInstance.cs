using Clarity.App.Worlds.Navigation;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.App.Worlds.StoryGraph 
{
    public interface IStoryLayoutInstance
    {
        IStoryPath GetPath(CameraProps initialCameraProps, int endNodeId, NavigationState navigationState, bool interLevel);
        int GetClosestNodeId(CameraProps cameraProps);
        bool AllowsFreeCamera { get; }
        IControlledCamera CreateFreeCamera(CameraProps initialCameraProps);
    }
}