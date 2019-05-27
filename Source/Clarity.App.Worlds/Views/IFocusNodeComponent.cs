using Clarity.App.Worlds.Views.Cameras;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Views
{
    public interface IFocusNodeComponent : ISceneNodeComponent
    {
        IDefaultViewpointMechanism DefaultViewpointMechanism { get; }
        bool InstantTransition { get; }
    }
}