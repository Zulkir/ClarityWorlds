using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.StoryLayout.Building
{
    public class BuildingStoryLayoutPlacementAlgorithm
    {
        private readonly ICoroutineService coroutineService;
        private readonly Action<BuildingStoryLayoutPlacementAlgorithm> visualize;
        private readonly IStoryGraph sg;
        private readonly BuildingLaneBuilder laneBuilder;
        private readonly int[] nodeRows;
        private readonly Dictionary<int, BuildingStoryLayoutAbstractGrid> grids;
        private readonly Vector3[] relPositions;
        private readonly Size3[] halfSizes;
        private readonly bool[] isReversed;
        private readonly Dictionary<int, Transform> relativeTransforms;

        public IStoryGraph StoryGraph => sg;
        public IReadOnlyList<Vector3> RelativePositions => relPositions;
        public IReadOnlyList<Size3> HalfSizes => halfSizes;
        public IReadOnlyList<bool> IsReversed => isReversed;
        public IReadOnlyList<Vector3> LanePoints => laneBuilder.RawPoints;
        public IReadOnlyDictionary<int, List<BuildingRawLaneSegment>> LaneSegments => laneBuilder.RawSegmentsByParentNodes;
        public IReadOnlyDictionary<int, Transform> RelativeTransforms => relativeTransforms;

        public IReadOnlyDictionary<Pair<int>, BuildingLane> Lanes => laneBuilder.Lanes;

        public BuildingStoryLayoutPlacementAlgorithm(ICoroutineService coroutineService, Action<BuildingStoryLayoutPlacementAlgorithm> visualize, IStoryGraph sg)
        {
            this.coroutineService = coroutineService;
            this.visualize = visualize;
            this.sg = sg;
            laneBuilder = new BuildingLaneBuilder(sg, x => relativeTransforms[x]);
            nodeRows = sg.EnoughIntegers.Select(x => -1).ToArray();
            grids = sg.EnoughIntegers.ToDictionary(x => x, x => new BuildingStoryLayoutAbstractGrid());
            relPositions = new Vector3[sg.EnoughIntegers.Count];
            halfSizes = new Size3[sg.EnoughIntegers.Count];
            isReversed = new bool[sg.EnoughIntegers.Count];
            relativeTransforms = new Dictionary<int, Transform>();
        }

        private bool IsMultiFloorNode(int node) => node == sg.Root;

        public void Run()
        {
            FillGrids(sg.Root, nodeRows);
            PlaceChildren(sg.Root);
            CalcIsReversed();
            CalcRelTransforms(sg.Root);
            PlaceCorridors();
        }

        private void FillGrids(int subtreeRoot, int[] nodeRowCache)
        {
            var grid = grids[subtreeRoot];
            foreach (var child in sg.Children[subtreeRoot])
                grid.GetRow(GetNodeRow(child, nodeRowCache)).Add(child);
            foreach (var child in sg.Children[subtreeRoot].Where(x => sg.Children[x].Any()))
                FillGrids(child, nodeRowCache);
        }

        private int GetNodeRow(int node, int[] nodeRowCache)
        {
            if (nodeRowCache[node] != -1)
                return nodeRowCache[node];
            var siblingPrevs = sg.Children[sg.Parents[node]].Where(x => sg.LeveledNext[x].Contains(node));
            var result = siblingPrevs.Select(x => GetNodeRow(x, nodeRowCache)).ConcatSingle(-1).Max() + 1;
            nodeRowCache[node] = result;
            return nodeRowCache[node];
        }

        private void PlaceChildren(int subtreeRoute)
        {
            var children = sg.Children[subtreeRoute];

            var hasChildren = children.Any();
            if (!hasChildren)
            {
                halfSizes[subtreeRoute] = BuildingConstants.LeafHalfSize;
                return;
            }

            if (IsMultiFloorNode(subtreeRoute))
                ReversedFloors(subtreeRoute);

            foreach (var child in children)
                PlaceChildren(child);

            if (IsMultiFloorNode(subtreeRoute))
                PlaceFloors(subtreeRoute);
            else
                PlaceLeaves(subtreeRoute);

            //else if (!children.SelectMany(x => sg.Children[x]).Any())
            //    await PlaceLeaves(subtreeRoute);
            //else
            //    await PlaceIntermediateChildren(subtreeRoute);
        }

        private void CalcIsReversed()
        {
            var floorNodes = sg.Children[sg.Root];
            foreach (var floorNode in floorNodes)
            {
                if (sg.LeveledPrevious[floorNode].Count(x => !isReversed[x]) >
                    sg.LeveledPrevious[floorNode].Count(x => isReversed[x]))
                    isReversed[floorNode] = true;
            }
        }

        private void CalcRelTransforms(int subtreeRoute)
        {
            var children = sg.Children[subtreeRoute];
            var hasChildren = children.Any();
            relativeTransforms[subtreeRoute] = hasChildren
                ? new Transform(1,
                    isReversed[subtreeRoute] ? Quaternion.RotationY(MathHelper.Pi) : Quaternion.Identity,
                    relPositions[subtreeRoute])
                : Transform.Translation(relPositions[subtreeRoute] - Vector3.UnitZ * halfSizes[subtreeRoute].Depth);
            foreach (var child in children)
                CalcRelTransforms(child);
        }

        private void PlaceLeaves(int parentNode)
        {
            var children = sg.Children[parentNode];
            var grid = grids[parentNode];

            float GetIsolatedRowUpperDepth(List<int> row, int highwayIndex)
            {
                return row.Skip(highwayIndex).Sum(x => 2 * halfSizes[x].Depth) + BuildingConstants.DepthMargin * (row.Count - highwayIndex);
            }

            float GetIsolatedRowLowerDepth(List<int> row, int highwayIndex)
            {
                return row.Take(highwayIndex).Sum(x => 2 * halfSizes[x].Depth) + BuildingConstants.DepthMargin * (highwayIndex + 1);
            }

            float GetIsolatedRowDepth(List<int> row, out int highwayIndex)
            {
                var result = BuildingConstants.DepthMargin;
                highwayIndex = 0;
                for (int i = 0; i < row.Count; i++)
                {
                    var higher = GetIsolatedRowUpperDepth(row, i);
                    var lower = GetIsolatedRowLowerDepth(row, i);
                    if (lower > higher)
                    {
                        highwayIndex = Math.Max(i - 1, 0);
                        break;
                    }
                    result = 2 * higher;
                }
                return result;
            }

            float GetRowWidth(int childRow)
            {
                return grid.GetRow(childRow).Max(x => halfSizes[x].Width * 2);
            }

            var totalWidth = Enumerable.Range(0, grid.RowCount).Sum(x => GetRowWidth(x)) + BuildingConstants.WidthMargin * (grid.RowCount + 1);
            var totalHeight = children.Max(x => halfSizes[x].Height * 2) + BuildingConstants.HeightMargin;
            var totalDepth = Enumerable.Range(0, grid.RowCount).Max(x => GetIsolatedRowDepth(grid.GetRow(x), out _));

            var pos = new Vector3(-totalWidth / 2, 0, totalDepth / 2);
            pos.X += BuildingConstants.WidthMargin;

            foreach (var rowIndex in Enumerable.Range(0, grid.RowCount))
            {
                var row = grid.GetRow(rowIndex);
                GetIsolatedRowDepth(row, out var highwayIndex);

                pos.Z = 0f;
                var higherActualDepthMargin = (totalDepth / 2 - row.Skip(highwayIndex).Sum(x => halfSizes[x].Depth * 2)) / (row.Count - highwayIndex);
                for (int i = highwayIndex; i < row.Count; i++)
                {
                    var child = row[i];
                    relPositions[child] = pos + Vector3.UnitX * halfSizes[child].Width;
                    pos.Z -= (2 * halfSizes[child].Depth + higherActualDepthMargin);
                }

                pos.Z = 0f;
                var lowerActualDepthMargin = (totalDepth / 2 - row.Take(highwayIndex).Sum(x => halfSizes[x].Depth * 2)) / (highwayIndex + 1);
                for (int i = highwayIndex - 1; i >= 0; i--)
                {
                    var child = row[i];
                    pos.Z += (2 * halfSizes[child].Depth + lowerActualDepthMargin);
                    relPositions[child] = pos + Vector3.UnitX * halfSizes[child].Width;
                }
                
                pos.X += GetRowWidth(rowIndex);
                pos.X += BuildingConstants.WidthMargin;
            }

            halfSizes[parentNode] = new Size3(totalWidth / 2, totalHeight / 2, totalDepth / 2);
        }

        private void PlaceIntermediateChildren(int subtreeRoute)
        {
            var children = sg.Children[subtreeRoute];
            var grid = grids[subtreeRoute];

            float GetRowWidth(int childRow)
            {
                return grid.GetRow(childRow).Max(x => halfSizes[x].Width * 2);
            }

            float GetRowDepth(int childRow)
            {
                var row = grid.GetRow(childRow);
                return row.Sum(x => halfSizes[x].Depth * 2) + BuildingConstants.DepthMargin * (row.Count + 1);
            }

            var totalWidth = Enumerable.Range(0, grid.RowCount).Sum(x => GetRowWidth(x)) + BuildingConstants.WidthMargin * (grid.RowCount + 1);
            var totalHeight = children.Max(x => halfSizes[x].Height * 2) + BuildingConstants.HeightMargin;
            var totalDepth = Enumerable.Range(0, grid.RowCount).Max(x => GetRowDepth(x));

            var pos = new Vector3(-totalWidth / 2, 0, totalDepth / 2);
            pos.X += BuildingConstants.WidthMargin;

            foreach (var rowIndex in Enumerable.Range(0, grid.RowCount))
            {
                var row = grid.GetRow(rowIndex);
                pos.Z = totalDepth / 2;
                var actualDepthMargin = (totalDepth - row.Sum(x => halfSizes[x].Depth * 2)) / (row.Count + 1);
                pos.Z -= actualDepthMargin;
                foreach (var child in row)
                {
                    relPositions[child] = pos + new Vector3(halfSizes[child].Width, 0, -halfSizes[child].Depth);
                    pos.Z -= (2 * halfSizes[child].Depth + actualDepthMargin);
                }
                pos.X += GetRowWidth(rowIndex);
                pos.X += BuildingConstants.WidthMargin;
            }

            halfSizes[subtreeRoute] = new Size3(totalWidth / 2, totalHeight / 2, totalDepth / 2);
        }

        private void PlaceFloors(int subtreeRoute)
        {
            var children = sg.Children[subtreeRoute];
            var grid = grids[subtreeRoute];

            var totalWidth = children.Max(x => 2 * halfSizes[x].Width);
            var totalHeight = children.Sum(x => 2 * halfSizes[x].Height + BuildingConstants.HeightMargin);
            var totalDepth = children.Max(x => 2 * halfSizes[x].Depth);

            var y = 0f;
            foreach (var child in grid.EnumerateRows().SelectMany(x => x))
            {
                halfSizes[child].Width = totalWidth / 2;
                //halfSizes[child].Depth = totalDepth / 2;
                relPositions[child] = new Vector3(0, y, 0);
                y += 2 * halfSizes[child].Height + BuildingConstants.HeightMargin;
            }

            halfSizes[subtreeRoute] = new Size3(totalWidth / 2, totalHeight / 2, totalDepth / 2);
        }

        private void ReversedFloors(int subtreeRoute)
        {
            var children = sg.Children[subtreeRoute];
            foreach (var child in children)
            {
                if (sg.LeveledPrevious[child].Count(x => !isReversed[x]) > sg.LeveledPrevious[child].Count(x => isReversed[x]))
                    isReversed[child] = true;
            }
        }

        private void PlaceCorridors()
        {
            var occupiedLanes = new IntSet32[sg.NodeIds.Max() + 1];

            foreach (var edge in sg.Edges)
            {
                laneBuilder.StartLane(edge);
                var disambiguator = 0;
                var commonOccupiedLanes = new IntSet32();
                if (sg.Parents[edge.First] == sg.Parents[edge.Second])
                {
                    var commonParent = sg.Parents[edge.First];
                    var startPoint = relPositions[edge.First] + Vector3.UnitX * halfSizes[edge.First].Width;
                    var endPoint = relPositions[edge.Second] - Vector3.UnitX * halfSizes[edge.Second].Width;

                    if (!IsMultiFloorNode(commonParent))
                    {
                        if (nodeRows[edge.Second] - nodeRows[edge.First] == 1)
                        {
                            laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.Immediate, true);
                            laneBuilder.AddRawPoint(startPoint);
                            laneBuilder.AddRawPoint(startPoint + Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                            laneBuilder.AddRawPoint(endPoint - Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                            laneBuilder.AddRawPoint(endPoint);
                            laneBuilder.EndLaneSegment(commonParent, true);
                        }
                        else
                        {
                            disambiguator = 1;
                            while (occupiedLanes[commonParent].Contains(disambiguator))
                                disambiguator++;

                            var leafChildrenOnly = !sg.Children[commonParent].SelectMany(x => sg.Children[x]).Any();
                            if (leafChildrenOnly)
                            {
                                laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.LongRange, true);
                                laneBuilder.AddRawPoint(startPoint);
                                laneBuilder.AddRawPoint(startPoint + Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                                laneBuilder.AddRawPoint(new Vector3(startPoint.X + BuildingConstants.WidthMargin - BuildingConstants.CorridorBranchingOffset, startPoint.Y, 0));
                                laneBuilder.AddRawPoint(new Vector3(endPoint.X - BuildingConstants.WidthMargin + BuildingConstants.CorridorBranchingOffset, startPoint.Y, 0));
                                laneBuilder.AddRawPoint(endPoint - Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                                laneBuilder.AddRawPoint(endPoint);
                                laneBuilder.EndLaneSegment(commonParent, true);
                                occupiedLanes[commonParent] = occupiedLanes[commonParent].With(disambiguator);
                            }
                            else
                            {
                                laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.LongRange, true);
                                laneBuilder.AddRawPoint(startPoint);
                                laneBuilder.AddRawPoint(startPoint + Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                                laneBuilder.AddRawPoint(new Vector3(startPoint.X + BuildingConstants.WidthMargin - BuildingConstants.CorridorBranchingOffset, startPoint.Y, 0));
                                laneBuilder.AddRawPoint(new Vector3(startPoint.X + BuildingConstants.WidthMargin, startPoint.Y, 0));
                                laneBuilder.EndLaneSegment(commonParent, false);

                                laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.LongRange, false);
                                laneBuilder.AddRawPoint(new Vector3(endPoint.X - BuildingConstants.WidthMargin, startPoint.Y, 0));
                                laneBuilder.AddRawPoint(new Vector3(endPoint.X - BuildingConstants.WidthMargin + BuildingConstants.CorridorBranchingOffset, startPoint.Y, 0));
                                laneBuilder.AddRawPoint(endPoint - Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                                laneBuilder.AddRawPoint(endPoint);
                                laneBuilder.EndLaneSegment(commonParent, true);

                                occupiedLanes[commonParent] = occupiedLanes[commonParent].With(disambiguator);
                            }
                        }
                    }
                    else
                    {
                        disambiguator = 1;
                        while (occupiedLanes[commonParent].Contains(disambiguator))
                            disambiguator++;

                        var floorDistance = nodeRows[edge.Second] - nodeRows[edge.First];
                        laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.LongRange, true);
                        laneBuilder.AddRawPoint(startPoint);
                        laneBuilder.AddRawPoint(new Vector3(halfSizes[commonParent].Width, startPoint.Y, startPoint.Z));
                        laneBuilder.AddRawPoint(new Vector3(halfSizes[commonParent].Width, startPoint.Y, -halfSizes[commonParent].Depth - BuildingConstants.StairsDistance * (floorDistance + 1)));
                        laneBuilder.AddRawPoint(new Vector3(-halfSizes[commonParent].Width, endPoint.Y, -halfSizes[commonParent].Depth - BuildingConstants.StairsDistance * (floorDistance + 1)));
                        laneBuilder.AddRawPoint(new Vector3(-halfSizes[commonParent].Width, endPoint.Y, endPoint.Z));
                        laneBuilder.AddRawPoint(endPoint);
                        laneBuilder.EndLaneSegment(commonParent, true);
                    }
                }
                else
                {
                    var commonParent = sg.GetCommonParent(edge.First, edge.Second);
                    var laneContainerNodes = new List<int>();

                    var descendingNodes = new Stack<int>();
                    {
                        var descNode = edge.Second;
                        while (descNode != commonParent)
                        {
                            descendingNodes.Push(descNode);
                            descNode = sg.Parents[descNode];
                        }
                    }

                    var node = edge.First;
                    while (sg.Parents[node] != commonParent)
                    {
                        var parent = sg.Parents[node];
                        commonOccupiedLanes = commonOccupiedLanes.Union(occupiedLanes[parent]);
                        laneContainerNodes.Add(parent);
                        var nodeLoc = node;
                        var transitSiblings = sg.Children[parent].Where(x => relPositions[x].Z == 0f && nodeRows[nodeLoc] < nodeRows[x]);
                        foreach (var transitSibling in transitSiblings)
                        {
                            commonOccupiedLanes = commonOccupiedLanes.Union(occupiedLanes[transitSibling]);
                            laneContainerNodes.Add(transitSibling);
                        }
                        var startPoint = relPositions[node] + Vector3.UnitX * halfSizes[node].Width;
                        laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.InterNode, node == edge.First);
                        laneBuilder.AddRawPoint(startPoint);
                        laneBuilder.AddRawPoint(startPoint + Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                        laneBuilder.AddRawPoint(new Vector3(startPoint.X + BuildingConstants.WidthMargin - BuildingConstants.CorridorBranchingOffset, startPoint.Y, 0));
                        laneBuilder.AddRawPoint(new Vector3(halfSizes[parent].Width, startPoint.Y, 0));
                        laneBuilder.EndLaneSegment(parent, false);
                        node = sg.Parents[node];
                    }

                    {
                        var topLevelChildFirst = node;
                        var topLevelChildSecond = descendingNodes.Pop();
                        commonOccupiedLanes = commonOccupiedLanes.Union(occupiedLanes[commonParent]);
                        laneContainerNodes.Add(commonParent);

                        var startPoint = relPositions[topLevelChildFirst] + Vector3.UnitX * halfSizes[topLevelChildFirst].Width;
                        var endPoint = relPositions[topLevelChildSecond] - Vector3.UnitX * halfSizes[topLevelChildSecond].Width;

                        if (!IsMultiFloorNode(commonParent))
                        {
                            if (nodeRows[topLevelChildSecond] - nodeRows[topLevelChildFirst] == 1)
                            {
                                laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.InterNode, false);
                                laneBuilder.AddRawPoint(startPoint);
                                laneBuilder.AddRawPoint(startPoint + Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                                laneBuilder.AddRawPoint(endPoint - Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                                laneBuilder.AddRawPoint(endPoint);
                                laneBuilder.EndLaneSegment(commonParent, false);
                            }
                            else
                            {
                                var transitSiblings = sg.Children[commonParent].Where(x => relPositions[x].Z == 0f && nodeRows[topLevelChildFirst] < nodeRows[x] && nodeRows[x] < nodeRows[topLevelChildSecond]);
                                foreach (var transitSibling in transitSiblings)
                                {
                                    commonOccupiedLanes = commonOccupiedLanes.Union(occupiedLanes[transitSibling]);
                                    laneContainerNodes.Add(transitSibling);
                                }

                                laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.InterNode, false);
                                laneBuilder.AddRawPoint(startPoint);
                                laneBuilder.AddRawPoint(startPoint + Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                                laneBuilder.AddRawPoint(new Vector3(startPoint.X + BuildingConstants.WidthMargin - BuildingConstants.CorridorBranchingOffset, startPoint.Y, 0));
                                laneBuilder.AddRawPoint(new Vector3(endPoint.X - BuildingConstants.WidthMargin + BuildingConstants.CorridorBranchingOffset, startPoint.Y, 0));
                                laneBuilder.AddRawPoint(endPoint - Vector3.UnitX * BuildingConstants.CorridorBranchingOffset);
                                laneBuilder.AddRawPoint(endPoint);
                                laneBuilder.EndLaneSegment(commonParent, false);
                            }
                        }
                        else
                        {
                            var revStartPoint = new Vector3(-startPoint.X, startPoint.Y, startPoint.Z);
                            var revEndPoint = new Vector3(-endPoint.X, endPoint.Y, endPoint.Z);

                            var floorDistance = nodeRows[topLevelChildSecond] - nodeRows[topLevelChildFirst];
                            laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.Elevator, false);
                            if (!isReversed[topLevelChildFirst])
                            {
                                laneBuilder.AddRawPoint(startPoint);
                                laneBuilder.AddRawPoint(startPoint + Vector3.UnitX * BuildingConstants.ElevatorOffset);
                            }
                            else
                            {
                                laneBuilder.AddRawPoint(revStartPoint);
                                laneBuilder.AddRawPoint(startPoint - Vector3.UnitX * BuildingConstants.ElevatorOffset);
                            }

                            if (!isReversed[topLevelChildSecond])
                            {
                                if (!isReversed[topLevelChildFirst])
                                {
                                    laneBuilder.AddRawPoint(revEndPoint + Vector3.UnitX * BuildingConstants.ElevatorOffset);
                                    laneBuilder.AddRawPoint(revEndPoint);
                                    laneBuilder.AddRawPoint(endPoint);
                                }
                                else
                                {
                                    laneBuilder.AddRawPoint(endPoint - Vector3.UnitX * BuildingConstants.ElevatorOffset);
                                    laneBuilder.AddRawPoint(endPoint);
                                }
                            }
                            else
                            {
                                if (!isReversed[topLevelChildFirst])
                                {
                                    laneBuilder.AddRawPoint(revEndPoint + Vector3.UnitX * BuildingConstants.ElevatorOffset);
                                    laneBuilder.AddRawPoint(revEndPoint);
                                }
                                else
                                {
                                    laneBuilder.AddRawPoint(endPoint - Vector3.UnitX * BuildingConstants.ElevatorOffset);
                                    laneBuilder.AddRawPoint(endPoint);
                                    laneBuilder.AddRawPoint(revEndPoint);
                                }
                            }
                            laneBuilder.EndLaneSegment(commonParent, false);
                        }
                    }

                    while (descendingNodes.Any())
                    {
                        node = descendingNodes.Pop();
                        var parent = sg.Parents[node];
                        commonOccupiedLanes = commonOccupiedLanes.Union(occupiedLanes[parent]);
                        laneContainerNodes.Add(parent);
                        var nodeLoc = node;
                        var transitSiblings = sg.Children[parent].Where(x => relPositions[x].Z == 0f && nodeRows[x] < nodeRows[nodeLoc]);
                        foreach (var transitSibling in transitSiblings)
                        {
                            commonOccupiedLanes = commonOccupiedLanes.Union(occupiedLanes[transitSibling]);
                            laneContainerNodes.Add(transitSibling);
                        }

                        var endPoint = relPositions[node] - Vector3.UnitX * halfSizes[node].Width;
                        laneBuilder.StartLaneSegment(BuildingStoryLayoutLanePartType.InterNode, false);
                        laneBuilder.AddRawPoint(new Vector3(-halfSizes[parent].Width, endPoint.Y, 0));
                        laneBuilder.AddRawPoint(new Vector3(endPoint.X - BuildingConstants.WidthMargin + BuildingConstants.CorridorBranchingOffset, endPoint.Y, 0));
                        laneBuilder.AddRawPoint(new Vector3(endPoint.X - BuildingConstants.CorridorBranchingOffset, endPoint.Y, endPoint.Z));
                        laneBuilder.AddRawPoint(endPoint);
                        laneBuilder.EndLaneSegment(parent, node == edge.Second);
                    }

                    disambiguator = 1;
                    while (commonOccupiedLanes.Contains(disambiguator))
                        disambiguator++;

                    foreach (var laneContainerNode in laneContainerNodes)
                        occupiedLanes[laneContainerNode] = occupiedLanes[laneContainerNode].With(disambiguator);
                }
                laneBuilder.EndLane(disambiguator);
            }
        }

        public Transform GetGlobalTransform(int node)
        {
            return node == sg.Root
                ? RelativeTransforms[node]
                : RelativeTransforms[node] * GetGlobalTransform(sg.Parents[node]);
        }
    }
}