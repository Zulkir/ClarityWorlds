using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.Interaction.Manipulation3D;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Worlds.Hacks.SpherePackingLoad
{
    public abstract class SpherePackingComponent : SceneNodeComponentBase<SpherePackingComponent>,
        IVisualComponent, ITransformable3DComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly IEmbeddedResources embeddedResources;
        private readonly Lazy<IAppModeService> appModeServiceLazy;

        public abstract SpherePackingResult SpherePackingResult { get; set; }
        public abstract float Radius { get; set; }
        public abstract Color4 Color { get; set; }

        public Sphere LocalBoundingSphere { get; private set; }

        private IVisualElement[] visualElems;
        private readonly IInteractionElement selectInteractionELement;
        private readonly IRayHittable hittable;

        protected SpherePackingComponent(IEmbeddedResources embeddedResources, IViewService viewService, Lazy<IAppModeService> appModeServiceLazy)
        {
            this.embeddedResources = embeddedResources;
            this.appModeServiceLazy = appModeServiceLazy;
            Radius = 0.05f;
            Color = Color4.Red;
            selectInteractionELement = new SelectOnClickInteractionElement(this, viewService);
            hittable = new SphereHittable<SpherePackingComponent>(this, x => x.LocalBoundingSphere * x.Node.GlobalTransform);
        }

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            if (message.Obj(this).ValueChanged(x => x.SpherePackingResult, out _))
                Rebuild();
        }

        private void Rebuild()
        {
            if (SpherePackingResult == null)
                return;
            var material = StandardMaterial.New(this)
                .SetDiffuseColor(x => x.Color);
            visualElems = SpherePackingResult.Points.Select(p =>
                    ModelVisualElement.New(this)
                        .SetModel(embeddedResources.SphereModel(64))
                        .SetTransform(x => new Transform(x.Radius, Quaternion.Identity, p))
                        .SetMaterial(material))
                .Cast<IVisualElement>()
                .ToArray();
            LocalBoundingSphere = Sphere.BoundingSphere(SpherePackingResult.Points);
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements() => visualElems;
        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return appModeServiceLazy.Value.Mode == AppMode.Editing &&
                   selectInteractionELement.TryHandleInteractionEvent(args);
        }

        // Hittable
        public RayHitResult HitWithClick(RayCastInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}