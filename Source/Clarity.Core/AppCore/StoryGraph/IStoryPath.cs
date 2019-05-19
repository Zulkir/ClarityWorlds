using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Core.AppCore.StoryGraph 
{
    public interface IStoryPath
    {
        CameraProps GetCurrentCameraProps();
        void Update(FrameTime frameTime);
        bool HasFinished { get; }
        float MaxRemainingTime { get; }
    }
}