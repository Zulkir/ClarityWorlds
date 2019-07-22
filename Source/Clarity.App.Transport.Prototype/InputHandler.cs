using System.Collections.Generic;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;

namespace Clarity.App.Transport.Prototype
{
    public class InputHandler : IInputHandler
    {
        private readonly HashSet<IInputLock> inputLocks;
        private readonly List<IInputLock> locksToRelease;

        public InputHandler(IEventRoutingService eventRoutingService)
        {
            inputLocks = new HashSet<IInputLock>();
            locksToRelease = new List<IInputLock>();
            eventRoutingService.Subscribe<IInteractionEvent>(typeof(IInputHandler), nameof(OnEvent), OnEvent);
        }

        private void OnEvent(IInteractionEvent interactionEvent)
        {
            if (!(interactionEvent is IInputEvent abstractInputEvent))
                return;
            locksToRelease.Clear();
            foreach (var inputLock in inputLocks)
            {
                var result = inputLock.ProcessEvent(abstractInputEvent);
                if ((result & InputEventProcessResult.StopPropagating) != 0)
                    return;
                if ((result & InputEventProcessResult.ReleaseLock) != 0)
                    locksToRelease.Add(inputLock);
            }
            foreach (var inputLock in locksToRelease)
                inputLocks.Remove(inputLock);

            if (abstractInputEvent.Viewport?.View.TryHandleInput(abstractInputEvent) ?? false)
                return;
        }

        public void AddLock(IInputLock inputLock)
        {
            inputLocks.Add(inputLock);
        }
    }
}