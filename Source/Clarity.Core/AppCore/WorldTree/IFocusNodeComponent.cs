using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface IFocusNodeComponent : ISceneNodeComponent
    {
        IDefaultViewpointMechanism DefaultViewpointMechanism { get; }
        bool InstantTransition { get; }
    }
}