using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction.Tools
{
    public class MoveEntityTool : ITool
    {
        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedo;

        private readonly ISceneNode entity;
        private readonly Transform initialLocalTransform;
        private readonly bool isNew;
        
        private bool done;

        public MoveEntityTool(ISceneNode entity, bool isNew, IToolService toolService, IUndoRedoService undoRedo)
        {
            this.entity = entity;
            this.isNew = isNew;
            initialLocalTransform = entity.Transform;
            this.toolService = toolService;
            this.undoRedo = undoRedo;
            done = false;
        }

        public bool TryHandleInputEvent(IInputEventArgs eventArgs)
        {
            return eventArgs is IMouseEventArgs mouseArgs && TryHandleMouseEvent(mouseArgs);
        }

        private bool TryHandleMouseEvent(IMouseEventArgs eventArgs)
        {
            var viewport = eventArgs.Viewport;
            var placementNode = (viewport.View as IFocusableView)?.FocusNode;
            var cPlacement = placementNode?.SearchComponent<IPlacementComponent>();
            if (cPlacement == null)
            {
                entity.Deparent();
                return false;
            }
            var globalRay = viewport.GetGlobalRayForPixelPos(eventArgs.State.Position);
            if (!cPlacement.PlacementSurface3D.TryFindPlace(globalRay, out var placementTransform))
            {
                entity.Deparent();
                return false;
            }

            var newTransform = new Transform
            {
                Scale = isNew ? placementTransform.Scale : initialLocalTransform.Scale,
                Rotation = isNew ? placementTransform.Rotation : initialLocalTransform.Rotation,
                Offset = placementTransform.Offset
            };

            entity.Transform = newTransform;
            if (!entity.IsDescendantOf(placementNode))
            {
                entity.Deparent();
                placementNode.ChildNodes.Add(entity);
            }

            if (eventArgs.ComplexEventType == MouseEventType.Click && eventArgs.EventButtons == MouseButtons.Left)
            {
                undoRedo.OnChange();
                done = true;
                toolService.CurrentTool = null;
            }

            return false;
        }

        public void Dispose()
        {
            if (done)
                return;
            if (isNew)
                entity.Deparent();
            else
                entity.Transform = initialLocalTransform;
        }
    }
}