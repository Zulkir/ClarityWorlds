using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction
{
    public interface IInteractionComponent : ISceneNodeComponent
    {
        bool TryHandleInteractionEvent(IInteractionEvent args);
    }
}