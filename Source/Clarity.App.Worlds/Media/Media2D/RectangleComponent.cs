using System;
using System.Linq;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.Interaction.RectangleManipulation;
using Clarity.App.Worlds.Views;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.App.Worlds.Media.Media2D
{
    public abstract class RectangleComponent : SceneNodeComponentBase<RectangleComponent>, IRectangleComponent,
        IInteractionComponent, IRayHittableComponent
    {
        public abstract AaRectangle2 Rectangle { get; set; }
        public abstract bool DragByBorders { get; set; }

        private readonly Lazy<IAppModeService> appModeServiceLazy;

        private readonly IRayHittable hittable;
        private readonly IInteractionElement[] editInteractionElems;

        protected RectangleComponent(ICommonNodeFactory commonNodeFactory, Lazy<IAppModeService> appModeServiceLazy, IViewService viewService)
        {
            this.appModeServiceLazy = appModeServiceLazy;
            hittable = new RectangleHittable<RectangleComponent>(this, Transform.Identity,
                c => c.Rectangle, c => 0);

            editInteractionElems = new IInteractionElement[]
            {
                new SelectOnClickInteractionElement(this, viewService),
                new EditRectangleInteractionElement(this, commonNodeFactory),
            };
        }
        
        public override void Update(FrameTime frameTime)
        {
            var placementSurface = AmParent?.Node.PresentationInfra().Placement.PlacementSurface2D;
            if (placementSurface == null)
                return;
            var transform = placementSurface.Point2DToPlace(Vector2.Zero);
            if (Node.Transform != transform)
                Node.Transform = transform;
        }

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return appModeServiceLazy.Value.Mode == AppMode.Editing && 
                   editInteractionElems.Any(element => element.TryHandleInteractionEvent(args));
        }

        // Hittable
        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}