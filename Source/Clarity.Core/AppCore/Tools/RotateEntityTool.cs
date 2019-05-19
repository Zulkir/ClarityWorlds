using Clarity.Common.Numericals.Algebra;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Core.AppCore.Tools
{
    public class RotateEntityTool : ITool
    {
        private readonly ISceneNode entity;
        private readonly Transform initialLocalTransform;
        private bool done;

        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedo;

        public RotateEntityTool(ISceneNode entity, IToolService toolService, IUndoRedoService undoRedo)
        {
            this.entity = entity;
            this.toolService = toolService;
            this.undoRedo = undoRedo;
            initialLocalTransform = entity.Transform;
        }

        public bool TryHandleInputEvent(IInputEventArgs eventArgs)
        {
            return eventArgs is IMouseEventArgs mouseArgs && TryHandleMouseEvent(mouseArgs);
        }

        private bool TryHandleMouseEvent(IMouseEventArgs eventArgs)
        {
            var pixelPos = eventArgs.Viewport.GetPixelPos(entity);
            var pixelDelta = eventArgs.State.Position - pixelPos;
            var radianDelta = (Vector2)pixelDelta / 50f;
            var rotation = (eventArgs.KeyModifyers & KeyModifyers.Shift) == 0  
                ? Quaternion.RotationX(-radianDelta.Y) * Quaternion.RotationY(-radianDelta.X)
                : Quaternion.RotationZ(radianDelta.X) * Quaternion.RotationY(-radianDelta.Y);
            var newTransform = new Transform
            {
                Scale = initialLocalTransform.Scale,
                Offset = initialLocalTransform.Offset,
                Rotation = initialLocalTransform.Rotation * rotation
            };

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