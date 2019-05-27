using System.Collections.Generic;
using Clarity.App.Transport.Prototype.FirstProto.Simulation;
using Clarity.App.Transport.Prototype.Visualization;
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

namespace Clarity.App.Transport.Prototype.FirstProto.Visualization
{
    public abstract class PackageComponent : SceneNodeComponentBase<PackageComponent>, IVisualComponent, IRayHittableComponent, IInteractionComponent
    {
        private readonly IOldPlayback playback;
        private readonly ICommonMaterials commonMaterials;
        private readonly IVisualElement[] visualElements;
        private readonly IRayHittable hittable;

        public SimPackage Package { get; set; }

        protected PackageComponent(IEmbeddedResources embeddedResources, ICommonMaterials commonMaterials, IOldPlayback playback)
        {
            this.commonMaterials = commonMaterials;
            this.playback = playback;
            var model = embeddedResources.SphereModel(32);
            var cubeElem = new ModelVisualElement<PackageComponent>(this)
                .SetModel(model)
                .SetMaterial(x => x.commonMaterials.GetPackageMaterial(x.Package.Entry.Header.Code));
            visualElements = new IVisualElement[] { cubeElem };
            hittable = new SphereHittable<PackageComponent>(this, x => new Sphere(Node.GlobalTransform.Offset, 1));
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
            var component = AmFactory.Create<PackageComponent>();
            node.Components.Add(component);
            return node;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo)
        {
            return hittable.HitWithClick(clickInfo);
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (args is IMouseEventArgs margs && margs.IsLeftClickEvent())
            {
                playback.SelectedPackage = Package;
                return true;
            }
            return false;
        }
    }
}