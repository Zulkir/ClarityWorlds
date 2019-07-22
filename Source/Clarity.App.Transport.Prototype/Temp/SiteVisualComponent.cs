using System.Collections.Generic;
using Clarity.App.Transport.Prototype.FirstProto.Visualization;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Transport.Prototype.Temp
{
    public abstract class SiteVisualComponent : SceneNodeComponentBase<SiteComponent>, IVisualComponent, IRayHittableComponent, IInteractionComponent
    {
        private readonly IVisualElement[] visualElements;
        private readonly IRayHittable hittable;

        public string Site { get; set; }

        protected SiteVisualComponent(IEmbeddedResources embeddedResources)
        {
            var model = embeddedResources.CubeModel();
            var cubeElem = ModelVisualElement.New()
                .SetModel(model)
                .SetMaterial(StandardMaterial.New().SetDiffuseColor(Color4.White).FromGlobalCache());
            visualElements = new IVisualElement[] {cubeElem};
            hittable = new SphereHittable<SiteVisualComponent>(this, x => new Sphere(Node.GlobalTransform.Offset, 1));
        }

        public IEnumerable<IVisualElement> GetVisualElements()
        {
            return visualElements;
        }

        public IEnumerable<IVisualEffect> GetVisualEffects()
        {
            yield break;
        }

        public static ISceneNode CreateNode()
        {
            var node = AmFactory.Create<SceneNode>();
            var component = AmFactory.Create<SiteComponent>();
            node.Components.Add(component);
            return node;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo)
        {
            return hittable.HitWithClick(clickInfo);
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            if (args is IMouseEvent margs && margs.IsLeftClickEvent())
            {
                //playbackService.SelectedSite = Site;
                return true;
            }
            return false;
        }
    }
}