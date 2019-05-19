using System.Collections.Generic;
using Clarity.Engine.Interaction.Input;

namespace Clarity.App.Transport.Prototype
{
    public class InputHandler : IInputHandler
    {
        private readonly HashSet<IInputLock> inputLocks;
        private readonly List<IInputLock> locksToRelease;

        public InputHandler(IInputService inputService)
        {
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

            if (abstractArgs.Viewport?.View.TryHandleInput(abstractArgs) ?? false)
                return;
        }

        public void AddLock(IInputLock inputLock)
        {
            inputLocks.Add(inputLock);
        }
    }
}