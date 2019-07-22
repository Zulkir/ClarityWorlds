using System;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;

namespace Clarity.App.Worlds.Interaction.RectangleManipulation
{
    public class ResizeRectangleInteractionElement<TMaster> : IInteractionElement
    {
        private readonly TMaster master;
        private readonly Func<TMaster, IRectangleComponent> getRectAspect;
        private readonly Func<TMaster, IPlacementSurface> getSpace;
        private readonly IInputHandler inputHandler;
        private readonly IUndoRedoService undoRedo;
        private readonly Func<TMaster, ResizeRectangleGizmoPlace> getPlace;

        public ResizeRectangleInteractionElement(TMaster master,
            Func<TMaster, IRectangleComponent> getRectAspect, Func<TMaster, IPlacementSurface> getSpace,
            Func<TMaster, ResizeRectangleGizmoPlace> getPlace, 
            IInputHandler inputHandler, IUndoRedoService undoRedo)
        {
            this.master = master;
            this.inputHandler = inputHandler;
            this.undoRedo = undoRedo;
            this.getPlace = getPlace;
            this.getRectAspect = getRectAspect;
            this.getSpace = getSpace;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            if (args is IMouseEvent mouseArgs)
                return TryHandleMouseEvent(mouseArgs);
            return false;
        }

        private bool TryHandleMouseEvent(IMouseEvent args)
        {
            if (args.IsLeftDownEvent())
            {
                inputHandler.AddLock(new ResizeRectangleInputLock(getRectAspect(master), undoRedo, getSpace(master), getPlace(master)));
                return true;
            }
            return false;
        }
    }
}