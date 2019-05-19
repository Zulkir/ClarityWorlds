using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.Interaction.RectangleManipulation
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

        private ISceneNode GetGizmoRootNode() => Node?.ParentNode;
        private ISceneNode GetRectNode() => GetGizmoRootNode()?.ParentNode;
        private IRectangleComponent GetRectComponent() => GetRectNode()?.SearchComponent<IRectangleComponent>();
        private IPlacementPlane GetChildSpace()
        {
            var rectAspectNode = GetRectNode();
            return rectAspectNode?.ParentNode?.GetComponent<IPlacementPlaneComponent>().PlacementPlane;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo) => GetRectComponent().DragByBorders ? RayHitResult.Failure() : hittable.HitWithClick(clickInfo);
        public bool TryHandleInteractionEvent(IInteractionEventArgs args) => interactionElement.TryHandleInteractionEvent(args);
    }
}