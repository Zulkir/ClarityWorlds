using Clarity.App.Worlds.StoryGraph;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;

namespace Assets.Scripts.Interaction
{
    public interface IVrNavigationMode
    {
        string UserFriendlyName { get; }
        bool IsEnabled { get; }
        void Initialize();
        void SetEnabled(bool enable);
        void HideHints();
        void ShowHints(float seconds);
        void Update(FrameTime frameTime);
        void FixedUpdate();
        // todo: remove
        void NavigateTo(ISceneNode node, IStoryPath storyPath, CameraProps initialCameraProps, float? initialFloorHeight, float? targetFloorHeight);
    }
}