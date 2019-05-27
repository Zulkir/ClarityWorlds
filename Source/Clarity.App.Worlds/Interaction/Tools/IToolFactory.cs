using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Interaction.Tools
{
    public interface IToolFactory
    {
        ITool MoveEntity(ISceneNode entity, bool isNew);
        ITool RotateEntity(ISceneNode entity);
        ITool ScaleEntity(ISceneNode entity);
        //ITool AddCube();
        ITool AddRectangle();
        ITool AddImage(IImage image);
        ITool AddMovie(IMovie movie);
        ITool AddText();
        ITool DrawBorderCurve(ISceneNode entity);
        ITool AddExplicitStoryGraphEdge(int node);
        ITool StoryBranchInto(int from);
    }
}