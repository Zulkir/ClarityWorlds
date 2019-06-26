using System;
using Clarity.Engine.Interaction;

namespace Clarity.App.Worlds.Interaction
{
    public class LambdaInteractionElement : IInteractionElement
    {
        private readonly Func<IInteractionEvent, bool> tryHandle;

        public LambdaInteractionElement(Func<IInteractionEvent, bool> tryHandle)
        {
            this.tryHandle = tryHandle;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return tryHandle(args);
        }
    }

    public class LambdaInteractionElement<TMaster> : IInteractionElement
    {
        private readonly TMaster master;
        private readonly Func<TMaster, IInteractionEvent, bool> tryHandle;

        public LambdaInteractionElement(TMaster master, Func<TMaster, IInteractionEvent, bool> tryHandle)
        {
            this.master = master;
            this.tryHandle = tryHandle;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return tryHandle(master, args);
        }
    }
}