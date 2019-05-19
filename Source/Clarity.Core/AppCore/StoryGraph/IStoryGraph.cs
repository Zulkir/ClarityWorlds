using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.StoryGraph
{
    public interface IStoryGraph
    {
        int Depth { get; }
        bool SameLayout { get; }

        IReadOnlyList<int> EnoughIntegers { get; }
        IReadOnlyList<bool> IsUsed { get; }

        int Root { get; }
        IReadOnlyList<int> NodeIds { get; }
        IReadOnlyList<int> NodesInBfsOrder { get; }
        IReadOnlyList<int> Leaves { get; }
        IReadOnlyList<int> NonLeaves { get; }
        IReadOnlyList<Pair<int>> Edges { get; }
        IReadOnlyDictionary<int, IReadOnlyList<int>> Children { get; }
        IReadOnlyDictionary<int, int> Parents { get; }
        IReadOnlyDictionary<int, IReadOnlyList<int>> Next { get; }
        IReadOnlyDictionary<int, IReadOnlyList<int>> Previous { get; }
        IReadOnlyDictionary<int, IReadOnlyList<int>> Neighbors { get; }
        IReadOnlyDictionary<int, IReadOnlyList<Pair<int>>> LeveledEdges { get; }
        IReadOnlyDictionary<int, IReadOnlyList<int>> LeveledNext { get; }
        IReadOnlyDictionary<int, IReadOnlyList<int>> LeveledPrevious { get; }
        IReadOnlyDictionary<int, IReadOnlyList<int>> LeveledNeighbors { get; }
        IReadOnlyDictionary<int, IReadOnlyList<int>> ExternalConnections { get; }
        IReadOnlyDictionary<int, int> Depths { get; }

        IReadOnlyDictionary<int, ISceneNode> NodeObjects { get; }
        IReadOnlyDictionary<int, IStoryComponent> Aspects { get; }

        Pair<int> ToLeveledEdge(int from, int to, out int level);
        int GetCommonParent(int node1, int node2);

        IEnumerable<int> HierarchyDepthFirst();
        IEnumerable<int> HierarchyBreadthFirst();
        bool TryFindShortestPath(int from, int to, out IReadOnlyList<int> path);
    }
}