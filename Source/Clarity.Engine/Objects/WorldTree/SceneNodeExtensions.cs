using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;

namespace Clarity.Engine.Objects.WorldTree
{
    public static class SceneNodeExtensions
    {
        public static IEnumerable<ISceneNode> EnumerateSceneNodesDeep(this ISceneNode subtreeRoot) => 
            subtreeRoot.EnumSelf().Concat(subtreeRoot.ChildNodes.SelectMany(EnumerateSceneNodesDeep));

        public static IEnumerable<ISceneNode> EnumerateParents(this ISceneNode node)
        {
            var parent = node.ParentNode;
            return parent?.EnumSelf().Concat(parent.EnumerateParents()) ?? Enumerable.Empty<ISceneNode>();
        }
    }
}