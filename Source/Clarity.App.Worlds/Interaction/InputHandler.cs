using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.DirtyHacks;
using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.Views;
using Clarity.Engine.EventRouting;
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

        public InputHandler(IEventRoutingService eventRoutingService, IToolService toolService,
            INavigationService navigationService, Lazy<IDirtyHackService> dirtyHackServiceLazy, IRayHitIndex rayHitIndex, IViewService viewService)
        {
            this.toolService = toolService;
            this.navigationService = navigationService;
            this.dirtyHackServiceLazy = dirtyHackServiceLazy;
            this.rayHitIndex = rayHitIndex;
            this.viewService = viewService;
            inputLocks = new HashSet<IInputLock>();
            locksToRelease = new List<IInputLock>();
            eventRoutingService.Subscribe<IInteractionEvent>(typeof(IInputHandler), nameof(OnEvent), OnEvent);
        }

        private void OnEvent(IInteractionEvent interactionEvent)
        {
            if (!(interactionEvent is IInputEvent inputEvent))
                return;
            locksToRelease.Clear();
            foreach (var inputLock in inputLocks)
            {
                var result = inputLock.ProcessEvent(inputEvent);
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
                if (tool.TryHandleInputEvent(inputEvent))
                    return;
            }

            if (dirtyHackServiceLazy.Value.TryHandleInput(inputEvent))
                return;

            //if (abstractArgs is IMouseEventArgs mouseArgs)
            //    OnMouseEvent(mouseArgs);
            //else if (abstractArgs is IKeyEventArgs keyboardArgs)
            //    OnKeyEvent(keyboardArgs);
            // todo: return if handled

            if (inputEvent.Viewport?.View.TryHandleInput(inputEvent) ?? false)
                return;

            if (inputEvent.Viewport != null && inputEvent is IMouseEvent margs)
            {
                var hitSomething = false;
                foreach (var layer in inputEvent.Viewport.View.Layers)
                {
                    var clickInfo = new RayCastInfo(margs.Viewport, layer, margs.State.Position);
                    var hitResults = rayHitIndex.CastRay(clickInfo);
                    // todo: introduce different levels of stopping propagation and remove '.Take(1)'
                    foreach (var hitResult in hitResults.Take(1))
                    {
                        hitSomething = true;
                        margs.RayHitResult = hitResult;
                        foreach (var interactionElement in hitResult.Node.SearchComponents<IInteractionComponent>())
                            if (interactionElement.TryHandleInteractionEvent(margs))
                                return;
                    }
                    margs.RayHitResult = null;
                    if (layer.Camera is IControlledCamera controlledCamera && controlledCamera.TryHandleInput(inputEvent))
                        return;
                }
                if (margs.IsLeftClickEvent() && margs.KeyModifiers == KeyModifiers.None && !hitSomething)
                    viewService.SelectedNode = null;
            }

            if (inputEvent is IKeyEvent kargs && viewService.SelectedNode != null)
                foreach (var interactionElement in viewService.SelectedNode.Node.SearchComponents<IInteractionComponent>())
                    if (interactionElement.TryHandleInteractionEvent(kargs))
                        return;

            navigationService.TryHandleInput(inputEvent);
        }

        public void AddLock(IInputLock inputLock)
        {
            inputLocks.Add(inputLock);
        }
    }
}