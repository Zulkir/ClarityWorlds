using Clarity.Common.Numericals.Algebra;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Core.AppCore.Tools
{
    public class ScaleEntityTool : ITool
    {
        private readonly ISceneNode entity;
        private readonly ITransformable3DComponent transformable3DComponent;
        private readonly Transform initialLocalTransform;
        private bool done;

        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedo;

        public ScaleEntityTool(ISceneNode entity, IToolService toolService, IUndoRedoService undoRedo)
        {
            this.entity = entity;
            this.toolService = toolService;
            this.undoRedo = undoRedo;
            transformable3DComponent = entity.GetComponent<ITransformable3DComponent>();
            initialLocalTransform = entity.Transform;
        }

        public bool TryHandleInputEvent(IInputEventArgs eventArgs)
        {
            return eventArgs is IMouseEventArgs mouseArgs && TryHandleMouseEvent(mouseArgs);
        }

        private bool TryHandleMouseEvent(IMouseEventArgs eventArgs)
        {
            var viewport = eventArgs.Viewport;
            var aPlacement = entity.PresentationInfra().PlacementNodeAspect;
            if (aPlacement == null)
                return false;

            var globalRay = viewport.GetGlobalRayForPixelPos(eventArgs.State.Position);
            var placementPlane = aPlacement.PlacementPlane;
            if (!placementPlane.TryFindPlace(globalRay, out var placementTransform))
                return false;

            var delta = (placementTransform.Offset - entity.Transform.Offset).Length();
            var modelRadius = transformable3DComponent.OwnRadius;
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
                undoRedo.Common.ChangeProperty(entity, x => x.Transform, newTransform);
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