﻿using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction.RectangleManipulation
{
    public class DragRectangleInputLock : IInputLock
    {
        private readonly IUndoRedoService undoRedo;
        private readonly IRectangleComponent nodeRectAspect;
        private readonly IPlacementSurface space;
        private readonly AaRectangle2 originalRect;
        private readonly Vector2 anchorPointOnRect;

        public DragRectangleInputLock(IUndoRedoService undoRedo, IRectangleComponent nodeRectAspect, IPlacementSurface space, Vector2 anchorPointOnRect)
        {
            this.undoRedo = undoRedo;
            this.nodeRectAspect = nodeRectAspect;
            this.space = space;
            this.anchorPointOnRect = anchorPointOnRect;
            originalRect = nodeRectAspect.Rectangle;
        }

        public InputEventProcessResult ProcessEvent(IInputEvent args)
        {
            if (args is IMouseEvent mouseArgs)
                return ProcessMouseEvent(mouseArgs);
            return InputEventProcessResult.DontCare;
        }

        private InputEventProcessResult ProcessMouseEvent(IMouseEvent args)
        {
            if ((args.State.Buttons & MouseButtons.Left) == 0)
            {
                nodeRectAspect.Node.GetComponent<IRectangleComponent>().Rectangle = nodeRectAspect.Rectangle;
                undoRedo.OnChange();
                return InputEventProcessResult.ReleaseLock;
            }
            if (!args.IsOfType(MouseEventType.Move))
                return InputEventProcessResult.DontCare;
            var globalRay = args.Viewport.GetGlobalRayForPixelPos(args.State.Position);
            if (!space.TryFindPoint2D(globalRay, out var pos2D))
                return InputEventProcessResult.StopPropagating;
            var minCorner = pos2D - anchorPointOnRect;
            var maxCorner = minCorner + new Vector2(originalRect.Width, originalRect.Height);
            nodeRectAspect.Rectangle = new AaRectangle2(minCorner, maxCorner);
            return InputEventProcessResult.StopPropagating;
        }
    }
}