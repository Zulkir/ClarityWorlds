using Clarity.Engine.Interaction;

namespace Clarity.App.Worlds.Interaction
{
    public interface IInteractionElement
    {
        bool TryHandleInteractionEvent(IInteractionEvent args);
    }
}