using System;
using System.Collections.Generic;
using Clarity.Core.AppCore.Interaction;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Components;

namespace Clarity.Core.AppCore.StoryGraph
{
    public class StoryNodeDynamicParts
    {
        public IDefaultViewpointMechanism DefaultViewpointMechanism { get; set; }
        public IRayHittable Hittable { get; set; }
        public IReadOnlyList<IVisualElement> VisualElements { get; set; }
        public IReadOnlyList<IInteractionElement> InteractionElements { get; set; }
        public Action<ISceneNode, object, FrameTime> OnUpdate { get; set; }
        public object OnUpdateClosure { get; set; }
    }
}