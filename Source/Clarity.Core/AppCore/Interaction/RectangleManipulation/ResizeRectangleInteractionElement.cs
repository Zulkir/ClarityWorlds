using System;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;

namespace Clarity.Core.AppCore.Interaction.RectangleManipulation
{
    public class ResizeRectangleInteractionElement<TMaster> : IInteractionElement
    {
        private readonly TMaster master;
        private readonly Func<TMaster, IRectangleComponent> getRectAspect;
        private readonly Func<TMaster, IPlacementPlane> getSpace;
        private readonly IInputHandler inputHandler;
        private readonly IUndoRedoService undoRedo;
        private readonly Func<TMaster, ResizeRectangleGizmoPlace> getPlace;

        public ResizeRectangleInteractionElement(TMaster master,
            Func<TMaster, IRectangleComponent> getRectAspect, Func<TMaster, IPlacementPlane> getSpace,
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

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (args is IMouseEventArgs mouseArgs)
                return TryHandleMouseEvent(mouseArgs);
            return false;
        }

        private bool TryHandleMouseEvent(IMouseEventArgs args)
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