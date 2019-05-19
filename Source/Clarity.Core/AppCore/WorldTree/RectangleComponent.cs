using System;
using System.Linq;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.AppModes;
using Clarity.Core.AppCore.Interaction;
using Clarity.Core.AppCore.Interaction.RectangleManipulation;
using Clarity.Core.AppCore.Views;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.Core.AppCore.WorldTree
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
        }

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            return appModeServiceLazy.Value.Mode == AppMode.Editing && 
                   editInteractionElems.Any(element => element.TryHandleInteractionEvent(args));
        }

        // Hittable
        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}