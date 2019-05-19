using System;
using System.Collections.Generic;
using Clarity.Core.AppCore.Tools;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.Input;

namespace Clarity.Core.AppCore.Input
{
    // todo: move all presentation-specific stuff to other places and return this class to Engine
    public class InputHandler : IInputHandler
    {
        private readonly IToolService toolService;
        private readonly INavigationService navigationService;

        private readonly HashSet<IInputLock> inputLocks;
        private readonly List<IInputLock> locksToRelease;

        private readonly Lazy<IDirtyHackService> dirtyHackServiceLazy;

        public InputHandler(IInputService inputService, IToolService toolService,
            INavigationService navigationService, Lazy<IDirtyHackService> dirtyHackServiceLazy)
        {
            this.toolService = toolService;
            this.navigationService = navigationService;
            this.dirtyHackServiceLazy = dirtyHackServiceLazy;
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

            navigationService.TryHandleInput(abstractArgs);
        }

        public void AddLock(IInputLock inputLock)
        {
            inputLocks.Add(inputLock);
        }
    }
}