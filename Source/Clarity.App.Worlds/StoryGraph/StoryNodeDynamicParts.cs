using System;
using System.Collections.Generic;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;

namespace Clarity.App.Worlds.StoryGraph
{
    public class StoryNodeDynamicParts
    {
        public IDefaultViewpointMechanism DefaultViewpointMechanism { get; set; }
        public IRayHittable Hittable { get; set; }
        public IReadOnlyList<IVisualElement> VisualElements { get; set; }
        public Func<ISceneNode, IEnumerable<IVisualEffect>> GetVisualEffects { get; set; }
        public Action<ISceneNode, object, FrameTime> OnUpdate { get; set; }
        public object OnUpdateClosure { get; set; }
        public IPlacementSurface PlacementSurface2D { get; set; }
        public IPlacementSurface PlacementSurface3D { get; set; }
    }
}