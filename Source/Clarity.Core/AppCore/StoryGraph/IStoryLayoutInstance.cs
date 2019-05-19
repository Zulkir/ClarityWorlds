using Clarity.Core.AppCore.Views;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Core.AppCore.StoryGraph 
{
    public interface IStoryLayoutInstance
    {
        IStoryPath GetPath(CameraProps initialCameraProps, int endNodeId, NavigationState navigationState, bool interLevel);
        int GetClosestNodeId(CameraProps cameraProps);
        bool AllowsFreeCamera { get; }
        IControlledCamera CreateFreeCamera(CameraProps initialCameraProps);
    }
}