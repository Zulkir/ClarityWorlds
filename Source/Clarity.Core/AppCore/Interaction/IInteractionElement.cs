using Clarity.Engine.Interaction;

namespace Clarity.Core.AppCore.Interaction
{
    public interface IInteractionElement
    {
        bool TryHandleInteractionEvent(IInteractionEventArgs args);
    }
}