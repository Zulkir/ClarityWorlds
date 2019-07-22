using System;
using Clarity.Engine.Interaction;

namespace Clarity.App.Worlds.Interaction
{
    public class ActionOnEventInteractionElement : IInteractionElement
    {
        private readonly Action action;
        private readonly Func<IInteractionEvent, bool> condition;

        public ActionOnEventInteractionElement(Func<IInteractionEvent, bool> condition, Action action)
        {
            this.action = action;
            this.condition = condition;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
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