using System;
using System.Collections.Generic;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;

namespace Assets.Scripts.Interaction
{
    public interface IVrInputDispatcher
    {
        IReadOnlyList<IVrNavigationMode> NavigationModes { get; }

        bool MinimapIsOpen { get; set; }

        void Initialize();
        void SetCapabilities(VrInputDispatcherCapabilities capabilities);
        void SetVrNavigationMode(Type type, bool showHints = false);
        void SetVrNavigationModeIndex(int? id, bool showHints = false);
        void NavigateTo(ISceneNode node, IStoryPath storyPath, CameraProps initialCameraProps, float? initialFloorHeight, float? targetFloorHeight);
    }
}