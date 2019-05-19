using System;
using Clarity.Engine.Interaction;

namespace Clarity.Core.AppCore.Interaction
{
    public class ActionOnEventInteractionElement : IInteractionElement
    {
        private readonly Action action;
        private readonly Func<IInteractionEventArgs, bool> condition;

        public ActionOnEventInteractionElement(Func<IInteractionEventArgs, bool> condition, Action action)
        {
            this.action = action;
            this.condition = condition;
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (condition(args))
            {
                action();
                return true;
            }
            return false;
        }
    }
}