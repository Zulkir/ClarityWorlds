using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Special
{
    public interface IWarpScrollComponent : ISceneNodeComponent
    {
        float VisibleScrollAmount { get; set; }
    }
}