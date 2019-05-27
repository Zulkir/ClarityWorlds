using Clarity.Engine.Interaction;

namespace Clarity.App.Worlds.Interaction
{
    public interface ICoreInterationEventArgs : IInteractionEventArgs
    {
        CoreInteractionEventCategory Category { get; }
        CoreInteractionEventType Type { get; }
    }
}