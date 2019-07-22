using System;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction.RectangleManipulation
{
    public class DragRectangleInteractionElement<TMaster> : IInteractionElement
    {
        private readonly TMaster master;
        private readonly Func<TMaster, IRectangleComponent> getRectAspect;
        private readonly Func<TMaster, IPlacementSurface> getSpace;
        private readonly IInputHandler inputHandler;
        private readonly IUndoRedoService undoRedo;

        public DragRectangleInteractionElement(TMaster master, 
            Func<TMaster, IRectangleComponent> getRectAspect, Func<TMaster, IPlacementSurface> getSpace, 
            IInputHandler inputHandler, IUndoRedoService undoRedo)
        {
            this.master = master;
            this.inputHandler = inputHandler;
            this.undoRedo = undoRedo;
            this.getRectAspect = getRectAspect;
            this.getSpace = getSpace;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            if (args is IMouseEvent mouseArgs)
                return TryHandleMouseEvent(mouseArgs);
            return true;
        }

        private bool TryHandleMouseEvent(IMouseEvent args)
        {
            if (args.IsLeftDownEvent())
            {
                var rectAspect = getRectAspect(master);
                var space = getSpace(master);
                var globalRay = args.Viewport.GetGlobalRayForPixelPos(args.State.Position);
                if (!space.TryFindPoint2D(globalRay, out var initialPoint))
                    return false;
                //if (rectAspect.Rectangle.DistanceToBorderFrom(initialPoint) > 0.05f)
                //    return false;
                var anchor = initialPoint - rectAspect.Rectangle.MinMin;
                inputHandler.AddLock(new DragRectangleInputLock(undoRedo, rectAspect, space, anchor));
                return true;
            }
            return false;
        }
    }
}