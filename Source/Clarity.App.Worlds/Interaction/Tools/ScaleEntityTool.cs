using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction.Manipulation3D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction.Tools
{
    public class ScaleEntityTool : ITool
    {
        private readonly ISceneNode entity;
        private readonly ITransformable3DComponent cTransformable;
        private readonly Transform initialLocalTransform;
        private bool done;

        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedo;

        public ScaleEntityTool(ISceneNode entity, IToolService toolService, IUndoRedoService undoRedo)
        {
            this.entity = entity;
            this.toolService = toolService;
            this.undoRedo = undoRedo;
            cTransformable = entity.GetComponent<ITransformable3DComponent>();
            initialLocalTransform = entity.Transform;
        }

        public bool TryHandleInputEvent(IInputEvent eventArgs)
        {
            return eventArgs is IMouseEvent mouseArgs && TryHandleMouseEvent(mouseArgs);
        }

        private bool TryHandleMouseEvent(IMouseEvent eventArgs)
        {
            var viewport = eventArgs.Viewport;
            var cPlacement = entity.PresentationInfra().Placement;
            if (cPlacement == null)
                return false;

            var globalRay = viewport.GetGlobalRayForPixelPos(eventArgs.State.Position);
            if (!cPlacement.PlacementSurface3D.TryFindPlace(globalRay, out var placementTransform))
                return false;

            var delta = (placementTransform.Offset - cTransformable.LocalBoundingSphere.Center * entity.Transform).Length();
            var modelRadius = cTransformable.LocalBoundingSphere.Radius;
            var scale = delta / modelRadius;

            var newTransform = new Transform
            {
                Rotation = initialLocalTransform.Rotation,
                Offset = initialLocalTransform.Offset,
                Scale = scale
            };
            entity.Transform = newTransform;

            if (eventArgs.ComplexEventType == MouseEventType.Click && eventArgs.EventButtons == MouseButtons.Left)
            {
                entity.Transform = initialLocalTransform;
                entity.Transform = newTransform;
                undoRedo.OnChange();
                done = true;
                toolService.CurrentTool = null;
            }
            else
            {
                entity.Transform = newTransform;
            }

            return false;
        }

        public void Dispose()
        {
            if (!done)
                entity.Transform = initialLocalTransform;
        }
    }
}