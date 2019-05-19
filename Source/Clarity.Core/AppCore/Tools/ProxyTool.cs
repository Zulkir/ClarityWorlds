using System;
using Clarity.Engine.Interaction.Input;

namespace Clarity.Core.AppCore.Tools
{
    public class ProxyTool<TMaster> : ITool
    {
        private readonly IToolService toolService;
        private readonly TMaster master;
        private readonly Func<IToolService, TMaster, IInputEventArgs, bool> tryHandleInputEvent;
        private readonly Action<TMaster> dispose;

        public ProxyTool(IToolService toolService, TMaster master, 
            Func<IToolService, TMaster, IInputEventArgs, bool> tryHandleInputEvent, 
            Action<TMaster> dispose)
        {
            this.toolService = toolService;
            this.master = master;
            this.tryHandleInputEvent = tryHandleInputEvent;
            this.dispose = dispose;
        }

        public bool TryHandleInputEvent(IInputEventArgs eventArgs) => tryHandleInputEvent(toolService, master, eventArgs);
        public void Dispose() => dispose(master);
    }
}