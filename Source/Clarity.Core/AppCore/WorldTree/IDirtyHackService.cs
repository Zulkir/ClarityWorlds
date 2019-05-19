using Clarity.Engine.Interaction;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface IDirtyHackService
    {
        bool TryHandleInput(IInteractionEventArgs args);
    }
}