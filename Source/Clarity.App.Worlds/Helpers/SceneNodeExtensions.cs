using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Helpers
{
    public static class SceneNodeExtensions
    {
        public static PresentationInfra PresentationInfra(this ISceneNode node) => 
            new PresentationInfra(node);
    }
}