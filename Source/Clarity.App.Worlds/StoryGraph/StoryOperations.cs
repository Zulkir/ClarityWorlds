using System.Linq;
using Clarity.App.Worlds.Helpers;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.StoryGraph
{
    // todo: move to StoryService itself?
    public static class StoryOperations
    {
        public static ISceneNode AddChild(IStoryService storyService, ICommonNodeFactory commonNodeFactory, int parent, object transaction)
        {
            storyService.OnBeginTransaction(transaction);
            var newNode = commonNodeFactory.StoryNode();
            if (storyService.GlobalGraph.Leaves.Contains(parent))
            {
                var nexts = storyService.GlobalGraph.Next[parent].ToArray();
                var prevs = storyService.GlobalGraph.Previous[parent].ToArray();
                foreach (var prev in prevs)
                    storyService.RemoveEdge(prev, parent);
                foreach (var next in nexts)
                    storyService.RemoveEdge(parent, next);
                storyService.GlobalGraph.NodeObjects[parent].ChildNodes.Add(newNode);
                foreach (var prev in prevs)
                    storyService.AddEdge(prev, newNode.Id);
                foreach (var next in nexts)
                    storyService.AddEdge(newNode.Id, next);
            }
            else
            {
                storyService.GlobalGraph.NodeObjects[parent].ChildNodes.Add(newNode);
            }
            storyService.OnEndTransaction(transaction);
            return newNode;
        }
    }
}