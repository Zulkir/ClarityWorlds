using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Media.Media2D
{
    public interface IRectangleComponent : ISceneNodeComponent
    {
        AaRectangle2 Rectangle { get; set; }
        bool DragByBorders { get; set; }
    }
}