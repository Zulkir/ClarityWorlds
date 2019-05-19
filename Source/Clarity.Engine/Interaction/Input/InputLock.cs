using System;

namespace Clarity.Engine.Interaction.Input
{
    public class InputLock<T> : IInputLock
    {
        private readonly T master;
        private readonly Func<T, IInputEventArgs, InputEventProcessResult> process;

        public InputLock(T master, Func<T, IInputEventArgs, InputEventProcessResult> process)
        {
            this.master = master;
            this.process = process;
        }

        public InputEventProcessResult ProcessEvent(IInputEventArgs args) => 
            process(master, args);
    }
}