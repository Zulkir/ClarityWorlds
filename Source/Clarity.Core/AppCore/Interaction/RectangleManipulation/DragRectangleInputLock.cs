using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Core.AppCore.Interaction.RectangleManipulation
{
    public class DragRectangleInputLock : IInputLock
    {
        private readonly IUndoRedoService undoRedo;
        private readonly IRectangleComponent nodeRectAspect;
        private readonly IPlacementPlane space;
        private readonly AaRectangle2 originalRect;
        private readonly Vector2 anchorPointOnRect;

        public DragRectangleInputLock(IUndoRedoService undoRedo, IRectangleComponent nodeRectAspect, IPlacementPlane space, Vector2 anchorPointOnRect)
        {
            this.undoRedo = undoRedo;
            this.nodeRectAspect = nodeRectAspect;
            this.space = space;
            this.anchorPointOnRect = anchorPointOnRect;
            originalRect = nodeRectAspect.Rectangle;
        }

        public InputEventProcessResult ProcessEvent(IInputEventArgs args)
        {
            if (args is IMouseEventArgs mouseArgs)
                return ProcessMouseEvent(mouseArgs);
            return InputEventProcessResult.DontCare;
        }

        private InputEventProcessResult ProcessMouseEvent(IMouseEventArgs args)
        {
            if ((args.State.Buttons & MouseButtons.Left) == 0)
            {
                undoRedo.Common.ChangeProperty(nodeRectAspect.Node, x => x.GetComponent<IRectangleComponent>(), x => x.Rectangle, originalRect, nodeRectAspect.Rectangle);
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