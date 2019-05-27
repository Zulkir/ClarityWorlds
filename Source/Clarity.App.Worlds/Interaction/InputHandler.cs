using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.DirtyHacks;
using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.Views;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.App.Worlds.Interaction
{
    // todo: move all presentation-specific stuff to other places and return this class to Engine
    public class InputHandler : IInputHandler
    {
        private readonly IToolService toolService;
        private readonly INavigationService navigationService;
        private readonly IRayHitIndex rayHitIndex;
        private readonly IViewService viewService;

        private readonly HashSet<IInputLock> inputLocks;
        private readonly List<IInputLock> locksToRelease;

        private readonly Lazy<IDirtyHackService> dirtyHackServiceLazy;

        public InputHandler(IInputService inputService, IToolService toolService,
            INavigationService navigationService, Lazy<IDirtyHackService> dirtyHackServiceLazy, IRayHitIndex rayHitIndex, IViewService viewService)
        {
            this.toolService = toolService;
            this.navigationService = navigationService;
            this.dirtyHackServiceLazy = dirtyHackServiceLazy;
            this.rayHitIndex = rayHitIndex;
            this.viewService = viewService;
            inputLocks = new HashSet<IInputLock>();
            locksToRelease = new List<IInputLock>();
            inputService.Input += OnEvent;
        }

        private void OnEvent(IInputEventArgs abstractArgs)
        {
            locksToRelease.Clear();
            foreach (var inputLock in inputLocks)
            {
                var result = inputLock.ProcessEvent(abstractArgs);
                if ((result & InputEventProcessResult.StopPropagating) != 0)
                    return;
                if ((result & InputEventProcessResult.ReleaseLock) != 0)
                    locksToRelease.Add(inputLock);
            }
            foreach (var inputLock in locksToRelease)
                inputLocks.Remove(inputLock);

            if (toolService.CurrentTool != null)
            {
                var tool = toolService.CurrentTool;
                if (tool.TryHandleInputEvent(abstractArgs))
                    return;
            }

            if (dirtyHackServiceLazy.Value.TryHandleInput(abstractArgs))
                return;

            //if (abstractArgs is IMouseEventArgs mouseArgs)
            //    OnMouseEvent(mouseArgs);
            //else if (abstractArgs is IKeyEventArgs keyboardArgs)
            //    OnKeyEvent(keyboardArgs);
            // todo: return if handled

            if (abstractArgs.Viewport?.View.TryHandleInput(abstractArgs) ?? false)
                return;

            if (abstractArgs.Viewport != null && abstractArgs is IMouseEventArgs margs)
            {
                var hitSomething = false;
                foreach (var layer in abstractArgs.Viewport.View.Layers)
                {
                    var clickInfo = new RayHitInfo(margs.Viewport, layer, margs.State.Position);
                    var hitResults = rayHitIndex.CastRay(clickInfo);
                    // todo: introduce different levels of stopping propagation and remove '.Take(1)'
                    foreach (var hitResult in hitResults.Take(1))
                    {
                        hitSomething = true;
                        foreach (var interactionElement in hitResult.Node.SearchComponents<IInteractionComponent>())
                            if (interactionElement.TryHandleInteractionEvent(margs))
                                return;
                    }
                    if (layer.Camera is IControlledCamera controlledCamera && controlledCamera.TryHandleInput(abstractArgs))
                        return;
                }
                if (margs.IsLeftClickEvent() && margs.KeyModifyers == KeyModifyers.None && !hitSomething)
                    viewService.SelectedNode = null;
            }

            if (abstractArgs is IKeyEventArgs kargs && viewService.SelectedNode != null)
                foreach (var interactionElement in viewService.SelectedNode.Node.SearchComponents<IInteractionComponent>())
                    if (interactionElement.TryHandleInteractionEvent(kargs))
                        return;

            navigationService.TryHandleInput(abstractArgs);
        }

        public void AddLock(IInputLock inputLock)
        {
            inputLocks.Add(inputLock);
        }
    }
}