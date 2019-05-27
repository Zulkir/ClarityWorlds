using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;

namespace Clarity.Engine.Objects.WorldTree
{
    public static class SceneExtensions
    {
        public static ISceneNode GetNodeById(this IScene scene, int id)
        {
            return scene.World?.GetNodeById(id) ?? scene.EnumerateAllNodes().FirstOrDefault(x => x.Id == id);
        }

        public static IEnumerable<ISceneNode> EnumerateAllNodes(this IScene scene, bool includeAuxiliary = true)
        {
            return includeAuxiliary 
                ? scene.Root.EnumSelf().Concat(scene.AuxuliaryNodes).SelectMany(x => x.EnumerateSceneNodesDeep())
                : scene.Root.EnumerateSceneNodesDeep();
        }
    }
}