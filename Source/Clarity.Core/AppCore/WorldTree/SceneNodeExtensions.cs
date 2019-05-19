using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Core.Presentations;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public static class SceneNodeExtensions
    {
        public static PresentationInfra PresentationInfra(this ISceneNode node) => 
            new PresentationInfra(node);

        // todo: refactor for scene portals
        public static IEnumerable<ISceneNode> EnumerateAllNodesDeep(this ISceneNode node) => 
            node.EnumSelf().Concat(node.ChildNodes.SelectMany(EnumerateAllNodesDeep));
    }
}