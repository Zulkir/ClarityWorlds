using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface ICommonNodeFactory
    {
        ISceneNode WorldRoot(bool withStoryComponent);
        ISceneNode PlacementPlane2D();
        ISceneNode PlacementPlane3D();

        ISceneNode ColorRectangleNode(Color4 color);
        ISceneNode ImageRectangleNode(IImage image);
        ISceneNode MovieRectangleNode(IMovie movie);
        ISceneNode RichTextRectangle(IRichText text);

        ISceneNode StoryNode();
        ISceneNode RectangleEditGizmo();
    }
}