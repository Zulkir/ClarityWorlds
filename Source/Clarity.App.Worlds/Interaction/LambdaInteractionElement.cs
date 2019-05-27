using System;
using Clarity.Engine.Interaction;

namespace Clarity.App.Worlds.Interaction
{
    public class LambdaInteractionElement : IInteractionElement
    {
        private readonly Func<IInteractionEventArgs, bool> tryHandle;

        public LambdaInteractionElement(Func<IInteractionEventArgs, bool> tryHandle)
        {
            this.tryHandle = tryHandle;
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            return tryHandle(args);
        }
    }

    public class LambdaInteractionElement<TMaster> : IInteractionElement
    {
        private readonly TMaster master;
        private readonly Func<TMaster, IInteractionEventArgs, bool> tryHandle;

        public LambdaInteractionElement(TMaster master, Func<TMaster, IInteractionEventArgs, bool> tryHandle)
        {
            this.master = master;
            this.tryHandle = tryHandle;
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            return tryHandle(master, args);
        }
    }
}