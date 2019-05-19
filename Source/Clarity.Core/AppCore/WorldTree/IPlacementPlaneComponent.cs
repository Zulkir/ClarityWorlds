using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface IPlacementPlaneComponent : ISceneNodeComponent
    {
        // todo: either change to rnum or remove completely
        bool Is2D { get; }
        IPlacementPlane PlacementPlane { get; }
    }
}