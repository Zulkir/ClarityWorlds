using System;

namespace Clarity.Engine.Interaction.Input
{
    public class InputLock<T> : IInputLock
    {
        private readonly T master;
        private readonly Func<T, IInputEvent, InputEventProcessResult> process;

        public InputLock(T master, Func<T, IInputEvent, InputEventProcessResult> process)
        {
            this.master = master;
            this.process = process;
        }

        public InputEventProcessResult ProcessEvent(IInputEvent args) => 
            process(master, args);
    }
}