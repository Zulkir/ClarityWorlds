using Clarity.Engine.Interaction;

namespace Clarity.Core.AppCore.Interaction
{
    public interface ICoreInterationEventArgs : IInteractionEventArgs
    {
        CoreInteractionEventCategory Category { get; }
        CoreInteractionEventType Type { get; }
    }
}