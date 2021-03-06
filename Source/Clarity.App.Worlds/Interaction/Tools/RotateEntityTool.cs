﻿using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction.Tools
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

        public bool TryHandleInputEvent(IInputEvent eventArgs)
        {
            return eventArgs is IMouseEvent mouseArgs && TryHandleMouseEvent(mouseArgs);
        }

        private bool TryHandleMouseEvent(IMouseEvent eventArgs)
        {
            var pixelPos = eventArgs.Viewport.GetPixelPos(entity);
            var pixelDelta = eventArgs.State.Position - pixelPos;
            var radianDelta = (Vector2)pixelDelta / 50f;
            var rotation = (eventArgs.KeyModifiers & KeyModifiers.Shift) == 0  
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