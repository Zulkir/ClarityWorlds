using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.StoryGraph
{
    public class StoryGraph : IStoryGraph
    {
        public int Depth { get; }
        public bool SameLayout { get; }

        public IReadOnlyList<int> EnoughIntegers { get; }
        public IReadOnlyList<bool> IsUsed { get; }

        public int Root { get; }
        public IReadOnlyList<int> NodeIds { get; }
        public IReadOnlyList<int> NodesInBfsOrder { get; }
        public IReadOnlyList<int> Leaves { get; }
        public IReadOnlyList<int> NonLeaves { get; }
        public IReadOnlyList<Pair<int>> Edges { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> Children { get; }
        public IReadOnlyDictionary<int, int> Parents { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> Next { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> Previous { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> Neighbors { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<Pair<int>>> LeveledEdges { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> SiblingNext { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> SiblingPrevious { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> SiblingNeighbors { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> LeveledNext { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> LeveledPrevious { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> LeveledNeighbors { get; }
        public IReadOnlyDictionary<int, IReadOnlyList<int>> ExternalConnections { get; }
        public IReadOnlyDictionary<int, int> Depths { get; }

        public IReadOnlyDictionary<int, ISceneNode> NodeObjects { get; }
        public IReadOnlyDictionary<int, IStoryComponent> Aspects { get; }
        public IReadOnlyDictionary<ISceneNode, int> Indices { get; }

        public StoryGraph(ISceneNode root, IReadOnlyList<Pair<int>> edges, bool sameLayout)
        {
            SameLayout = sameLayout;
            var aspectsUnsorted = root.GetComponent<IStoryComponent>().EnumerateStoryAspectsDeep(SameLayout).ToArray();
            var maxId = aspectsUnsorted.Max(x => x.Node.Id);
            EnoughIntegers = Enumerable.Range(0, maxId + 1).ToArray();
            Aspects = aspectsUnsorted.ToDictionary(x => x.Node.Id, x => x);
            NodeObjects = aspectsUnsorted.ToDictionary(x => x.Node.Id, x => x.Node);
            NodeIds = aspectsUnsorted.Select(x => x.Node.Id).ToArray();
            IsUsed = EnoughIntegers.Select(x => NodeObjects.ContainsKey(x)).ToArray();
            Indices = NodeObjects.ToDictionary(x => x.Value, x => x.Key);
            Children = Aspects.ToDictionary(x => x.Key, x => (IReadOnlyList<int>)x.Value.EnumerateImmediateStoryChildren(SameLayout).Select(y => y.Node.Id).ToArray());
            Root = root.Id;
            Parents = NodeObjects.ToDictionary(x => x.Key, x => x.Value.ParentNode?.Id ?? -1);
            Depths = NodeIds.ToDictionary(x => x, CalculateNodeDepth);
            Depth = CalculateDepth(Root);
            Leaves = EnumerateLeaves(Root).ToArray();
            NonLeaves = EnumerateNonLeaves(Root).ToArray();
            BuildPrevNext(edges, out var next, out var previous, out var goodEdges);
            Next = next;
            Previous = previous;
            Neighbors = Leaves.ToDictionary(x => x, x => (IReadOnlyList<int>)Next[x].Concat(Previous[x]).ToArray());
            Edges = goodEdges;
            LeveledEdges = BuildLeveledEdges();
            BuildLeveledPrevNext(out var leveledNext, out var leveledPrev);
            LeveledNext = leveledNext;
            LeveledPrevious = leveledPrev;
            LeveledNeighbors = NodeIds.ToDictionary(x => x, x => (IReadOnlyList<int>)LeveledNext[x].Concat(LeveledPrevious[x]).ToArray());
            ExternalConnections = BuildExternal();
            NodesInBfsOrder = BreadthFirstSearch().ToArray();
        }

        private int CalculateDepth(int node) =>
            1 + Children[node].Select(CalculateDepth).ConcatSingle(0).Max();

        private int CalculateNodeDepth(int node) =>
            node == Root ? 0 : 1 + CalculateNodeDepth(Parents[node]);

        private IEnumerable<int> EnumerateLeaves(int node)
        {
            return (Children[node].Any()
                ? Children[node].SelectMany(EnumerateLeaves)
                : node.EnumSelf());
        }

        private IEnumerable<int> EnumerateNonLeaves(int subgraphNode)
        {
            return !Children[subgraphNode].Any()
                ? Enumerable.Empty<int>()
                : subgraphNode.EnumSelf().Concat(Children[subgraphNode].SelectMany(EnumerateNonLeaves));
        }

        private void BuildPrevNext(
            IReadOnlyList<Pair<int>> edges,
            out IReadOnlyDictionary<int, IReadOnlyList<int>> next,
            out IReadOnlyDictionary<int, IReadOnlyList<int>> prev,
            out List<Pair<int>> goodEdges)
        {
            next = Leaves.ToDictionary(x => x, x => (IReadOnlyList<int>)new List<int>());
            prev = Leaves.ToDictionary(x => x, x => (IReadOnlyList<int>)new List<int>());
            goodEdges = new List<Pair<int>>();

            foreach (var edge in edges)
            {
                if (!next.TryGetValue(edge.First, out var firstList) ||
                    !prev.TryGetValue(edge.Second, out var secondList))
                    continue;
                goodEdges.Add(edge);
                ((List<int>)firstList).Add(edge.Second);
                ((List<int>)secondList).Add(edge.First);
            }
        }

        private IReadOnlyDictionary<int, IReadOnlyList<Pair<int>>> BuildLeveledEdges()
        {
            var result = Enumerable.Range(0, Depth).ToDictionary(x => x, x => (IReadOnlyList<Pair<int>>)new List<Pair<int>>());
            foreach (var edge in Edges)
            {
                var leveledEdge = ToLeveledEdge(edge.First, edge.Second, out var level);
                ((List<Pair<int>>)result[level]).Add(leveledEdge);
            }
            return result;
        }

        private void BuildLeveledPrevNext(
            out IReadOnlyDictionary<int, IReadOnlyList<int>> next,
            out IReadOnlyDictionary<int, IReadOnlyList<int>> prev)
        {
            next = NodeIds.ToDictionary(x => x, x => (IReadOnlyList<int>)new List<int>());
            prev = NodeIds.ToDictionary(x => x, x => (IReadOnlyList<int>)new List<int>());
            foreach (var edge in Edges)
            {
                var leveledEdge = ToLeveledEdge(edge.First, edge.Second, out _);
                if (!next.TryGetValue(leveledEdge.First, out var firstList) ||
                    !prev.TryGetValue(leveledEdge.Second, out var secondList))
                    throw new Exception("Fund an edge between unexisting node IDs.");
                ((List<int>) firstList).AddUnique(leveledEdge.Second);
                ((List<int>)secondList).AddUnique(leveledEdge.First);
            }
        }

        private IReadOnlyDictionary<int, IReadOnlyList<int>> BuildExternal()
        {
            var rExternal = NodeIds.ToDictionary(x => x, x => (IReadOnlyList<int>)new List<int>());
            foreach (var edge in Edges)
            {
                var commonParent = GetCommonParent(edge.First, edge.Second);
                
                var first = edge.First;
                while (Parents[first] != commonParent)
                {
                    ((List<int>)rExternal[first]).Add(edge.Second);
                    first = Parents[first];
                }

                var second = edge.Second;
                while (Parents[second] != commonParent)
                {
                    ((List<int>)rExternal[second]).Add(edge.First);
                    second = Parents[second];
                }
            }
            return rExternal;
        }

        private IEnumerable<int> BreadthFirstSearch()
        {
            var queue = new Queue<int>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                foreach (var child in Children[item])
                    queue.Enqueue(child);
                yield return item;
            }
        }

        public Pair<int> ToLeveledEdge(int from, int to, out int level)
        {
            var minDepth = Math.Min(Depths[from], Depths[to]);
            while (Depths[from] > minDepth)
                from = Parents[from];
            while (Depths[to] > minDepth)
                to = Parents[to];
            level = minDepth;
            while (Parents[from] != Parents[to])
            {
                from = Parents[from];
                to = Parents[to];
                level--;
            }
            return new Pair<int>(from, to);
        }

        public int GetCommonParent(int node1, int node2)
        {
            while (Depths[node1] > Depths[node2])
                node1 = Parents[node1];
            while (Depths[node2] > Depths[node1])
                node2 = Parents[node2];
            while (node1 != node2)
            {
                node1 = Parents[node1];
                node2 = Parents[node2];
            }
            return node1;
        }

        public IEnumerable<int> HierarchyDepthFirst()
        {
            return HierarchyDepthFirst(Root);
        }

        private IEnumerable<int> HierarchyDepthFirst(int node)
        {
            yield return node;
            foreach (var child in Children[node])
            foreach (var grandChild in HierarchyDepthFirst(child))
                yield return grandChild;
        }

        public IEnumerable<int> HierarchyBreadthFirst()
        {
            var queue = new Queue<int>();
            queue.Enqueue(Root);
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                yield return node;
                foreach (var child in Children[node])
                    queue.Enqueue(child);
            }
        }

        public bool TryFindShortestPath(int from, int to, out IReadOnlyList<int> path)
        {
            var backtracks = new Dictionary<int, int>();
            var marked = new HashSet<int>();
            var queue = new Queue<int>();
            queue.Enqueue(from);
            while (queue.Any())
            {
                var node = queue.Dequeue();
                marked.Add(node);

                foreach (var neighbor in Neighbors[node])
                {
                    if (marked.Contains(neighbor))
                        continue;

                    backtracks[neighbor] = node;
                    queue.Enqueue(neighbor);

                    if (neighbor == to)
                    {
                        var rpath = new List<int>();
                        var pathNode = neighbor;
                        rpath.Add(pathNode);
                        while (backtracks.TryGetValue(pathNode, out pathNode))
                            rpath.Add(pathNode);
                        rpath.Reverse();
                        path = rpath;
                        return true;
                    }
                }
            }
            path = null;
            return false;
        }
    }
}