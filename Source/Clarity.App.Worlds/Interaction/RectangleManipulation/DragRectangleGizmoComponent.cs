using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction.RectangleManipulation
{
    public abstract class DragRectangleGizmoComponent : SceneNodeComponentBase<DragRectangleGizmoComponent>, 
        IRayHittableComponent, IInteractionComponent
    {
        private static readonly AaRectangle2 DefaultRect = new AaRectangle2(Vector2.Zero, 0.5f, 0.5f);

        private readonly IRayHittable hittable;
        private readonly IInteractionElement interactionElement;

        protected DragRectangleGizmoComponent(IInputHandler inputHandler, IUndoRedoService undoRedo)
        {
            hittable = new RectangleHittable<DragRectangleGizmoComponent>(
                this,
                Transform.Identity,
                x => x.GetRectComponent()?.Rectangle ?? DefaultRect,
                x => -1f / (1 << 18));
            interactionElement = new DragRectangleInteractionElement<DragRectangleGizmoComponent>(this,
                x => x.GetRectComponent(), x => x.GetChildSpace(), inputHandler, undoRedo);
        }

        private EditRectangleGizmoComponent GetGizmoRootComponent() => Node.ParentNode.GetComponent<EditRectangleGizmoComponent>();
        private ISceneNode GetRectNode() => GetGizmoRootComponent().RectangleNode;
        private IRectangleComponent GetRectComponent() => GetRectNode()?.SearchComponent<IRectangleComponent>();
        private IPlacementSurface GetChildSpace() => GetRectNode()?.ParentNode?.PresentationInfra().Placement.PlacementSurface2D;

        public RayHitResult HitWithClick(RayCastInfo clickInfo) => GetRectComponent().DragByBorders ? RayHitResult.Failure() : hittable.HitWithClick(clickInfo);
        public bool TryHandleInteractionEvent(IInteractionEvent args) => interactionElement.TryHandleInteractionEvent(args);
    }
}