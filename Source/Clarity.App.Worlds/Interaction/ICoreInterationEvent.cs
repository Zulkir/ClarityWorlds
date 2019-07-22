using Clarity.Engine.Interaction;

namespace Clarity.App.Worlds.Interaction
{
    public interface ICoreInterationEvent : IInteractionEvent
    {
        CoreInteractionEventCategory Category { get; }
        CoreInteractionEventType Type { get; }
    }
}