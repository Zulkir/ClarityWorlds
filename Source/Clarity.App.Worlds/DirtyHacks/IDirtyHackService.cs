using Clarity.Engine.Interaction;

namespace Clarity.App.Worlds.DirtyHacks
{
    public interface IDirtyHackService
    {
        bool TryHandleInput(IInteractionEvent args);
    }
}