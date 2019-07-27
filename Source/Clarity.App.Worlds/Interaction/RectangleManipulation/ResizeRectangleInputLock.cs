using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction.RectangleManipulation
{
    public class ResizeRectangleInputLock : IInputLock
    {
        private readonly IRectangleComponent nodeRectAspect;
        private readonly IUndoRedoService undoRedo;
        private readonly IPlacementSurface space;
        private readonly AaRectangle2 originalRect;
        private readonly ResizeRectangleGizmoPlace place;

        public ResizeRectangleInputLock(IRectangleComponent nodeRectAspect, IUndoRedoService undoRedo, IPlacementSurface space, ResizeRectangleGizmoPlace place)
        {
            this.nodeRectAspect = nodeRectAspect;
            this.undoRedo = undoRedo;
            this.space = space;
            this.place = place;
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
            var or = originalRect;
            var asp = originalRect.Width / originalRect.Height;
            var converseAspectRatio = (args.KeyModifiers & KeyModifiers.Shift) != 0;
            switch (place)
            {
                case ResizeRectangleGizmoPlace.Left:
                    nodeRectAspect.Rectangle = NewRect(pos2D.X, or.MinY, or.MaxX, or.MaxY);
                    break;
                case ResizeRectangleGizmoPlace.Right:
                    nodeRectAspect.Rectangle = NewRect(or.MinX, or.MinY, pos2D.X, or.MaxY);
                    break;
                case ResizeRectangleGizmoPlace.Bottom:
                    nodeRectAspect.Rectangle = NewRect(or.MinX, pos2D.Y, or.MaxX, or.MaxY);
                    break;
                case ResizeRectangleGizmoPlace.Top:
                    nodeRectAspect.Rectangle = NewRect(or.MinX, or.MinY, or.MaxX, pos2D.Y);
                    break;
                case ResizeRectangleGizmoPlace.BottomLeft:
                {
                    var left = converseAspectRatio ? or.MaxX - asp * (or.MaxY - pos2D.Y) : pos2D.X;
                    nodeRectAspect.Rectangle = NewRect(left, pos2D.Y, or.MaxX, or.MaxY);
                    break;
                }
                case ResizeRectangleGizmoPlace.BottomRight:
                {
                    var right = converseAspectRatio ? or.MinX + asp * (or.MaxY - pos2D.Y) : pos2D.X;
                    nodeRectAspect.Rectangle = NewRect(or.MinX, pos2D.Y, right, or.MaxY);
                    break;
                }
                case ResizeRectangleGizmoPlace.TopLeft:
                {
                    var left = converseAspectRatio ? or.MaxX - asp * (pos2D.Y - or.MinY) : pos2D.X;
                    nodeRectAspect.Rectangle = NewRect(left, or.MinY, or.MaxX, pos2D.Y);
                    break;
                }
                case ResizeRectangleGizmoPlace.TopRight:
                {
                    var right = converseAspectRatio ? or.MinX + asp * (pos2D.Y - or.MinY) : pos2D.X;
                    nodeRectAspect.Rectangle = NewRect(or.MinX, or.MinY, right, pos2D.Y);
                    break;
                }
            }
            return InputEventProcessResult.StopPropagating;
        }

        private static AaRectangle2 NewRect(float left, float bottom, float right, float top)
        {
            return new AaRectangle2(new Vector2(left, bottom), new Vector2(right, top));
        }
    }
}