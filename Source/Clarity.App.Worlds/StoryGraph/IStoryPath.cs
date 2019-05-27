using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.App.Worlds.StoryGraph 
{
    public interface IStoryPath
    {
        CameraProps GetCurrentCameraProps();
        void Update(FrameTime frameTime);
        bool HasFinished { get; }
        float MaxRemainingTime { get; }
    }
}