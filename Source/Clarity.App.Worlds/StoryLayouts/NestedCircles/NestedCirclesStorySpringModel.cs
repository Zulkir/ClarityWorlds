using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.App.Worlds.StoryLayouts.NestedCircles
{
    public class NestedCirclesStorySpringModel
    {
        private class ChildPlace
        {
            public Vector2 Position;
            public int? Child;

            public bool IsOccupied => Child.HasValue;
        }

        private readonly ICoroutineService coroutineService;
        private readonly IStoryGraph sg;
        private readonly Action<NestedCirclesStorySpringModel> visualize;
        private readonly Dictionary<int, List<int>> nodeExternalConnections;

        private readonly Vector2[] positions;
        private readonly Vector2[] oldPositions;

        private readonly float[] radii;
        private readonly bool[] placed;

        private const float LeafRadius = 1f;
        private const float SpringConstant = 0.1f;
        private const float IterationDeltaT = 0.1f;

        public Vector2 GetPosition(int index) => positions[index];
        public float GetVisualRadius(int index) => placed[index] ? radii[index] : 0;

        public NestedCirclesStorySpringModel(ICoroutineService coroutineService, Action<NestedCirclesStorySpringModel> visualize, IStoryGraph sg)
        {
            Debug.Assert(SpringConstant * IterationDeltaT < 1, "SpringConstant * IterationDeltaT < 1");
            this.coroutineService = coroutineService;
            this.visualize = visualize;
            this.sg = sg;
            nodeExternalConnections = BuildNodeExternalConnections(sg);
            positions = new Vector2[sg.EnoughIntegers.Count];
            oldPositions = new Vector2[sg.EnoughIntegers.Count];
            radii = new float[sg.EnoughIntegers.Count];
            placed = new bool[sg.EnoughIntegers.Count];
        }

        private static Dictionary<int, List<int>> BuildNodeExternalConnections(IStoryGraph sg)
        {
            var result = new Dictionary<int, List<int>>();
            result.GetOrAdd(sg.Leaves.First(), x => new List<int>());
            result.GetOrAdd(sg.Leaves.Last(), x => new List<int>());
            foreach (var edge in sg.Edges)
            {
                var commonParent = sg.GetCommonParent(edge.First, edge.Second);

                var first = edge.First;
                while (sg.Parents[first] != commonParent)
                {
                    var list = result.GetOrAdd(first, x => new List<int>());
                    list.Add(edge.Second);
                    first = sg.Parents[first];
                }

                var second = edge.Second;
                while (sg.Parents[second] != commonParent)
                {
                    var list = result.GetOrAdd(second, x => new List<int>());
                    list.Add(edge.First);
                    second = sg.Parents[second];
                }
            }
            return result;
        }

        private Vector2 GetGlobalPos(int node)
        {
            var parent = sg.Parents[node];
            return parent != -1 ? positions[node] + GetGlobalPos(parent) : positions[node];
        }

        public void Apply()
        {
            BuildApproxRadii(sg.Root);
            RunAdjustmentLoop();
            visualize(this);
        }

        private async Task VisualizeAsync(float seconds = 0.01f)
        {
            visualize(this);
            await coroutineService.WaitSeconds(seconds);
        }

        private void BuildApproxRadii(int node)
        {
            var children = sg.Children[node];
            if (!children.Any())
            {
                radii[node] = LeafRadius;
                return;
            }

            foreach (var child in children)
                BuildApproxRadii(child);

            var maxChildRadius = children.Max(x => radii[x]);
            foreach (var child in children)
                radii[child] = maxChildRadius;
            var radius = CalcMinRadiusForChildren(children.Count, maxChildRadius);
            radii[node] = radius;
        }

        private static float CalcMinRadiusForChildren(int numChildren, float childRadius)
        {
            switch (numChildren)
            {
                case 1: return childRadius;
                case 2: return 2 * childRadius;
                case 3: return childRadius * (1f / MathHelper.Sin(MathHelper.PiOver3) + 1);
                case 4: return childRadius * (1 + MathHelper.Sqrt2);
                case 5:
                case 6:
                case 7: return 3 * childRadius;
                case 8:
                case 9:
                case 10: return 4 * childRadius;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19: return 5 * childRadius;
                default:
                {
                    var rowLength = 6;
                    while (PackCircles(rowLength * childRadius, childRadius, numChildren).Count() < numChildren)
                        rowLength++;
                    return rowLength * childRadius;
                };
            }
        }

        private void RunAdjustmentLoop()
        {
            // todo: make loop
            AdjustmentStep();
            AdjustmentStep();
        }

        private void AdjustmentStep()
        {
            placed[sg.Root] = true;
            foreach (var node in sg.HierarchyBreadthFirst())
                PlaceNodeChildren(node);
            foreach (var node in sg.HierarchyBreadthFirst().Reverse())
                CollapseCircles(node);
            foreach (var node in sg.HierarchyBreadthFirst())
                RotateChildren(node);
        }

        private float TotalEnergy()
        {
            return sg.NodeIds.Select(ChildEnergy).Sum();
        }

        private float ChildEnergy(int node)
        {
            if (!sg.Children[node].Any())
                return 0;
            return sg.Children[node]
                .SelectMany(x => sg.LeveledNeighbors[x].Select(y => new Pair<int>(x, y)))
                .Select(SpringEnergy)
                .Sum();
        }

        private float SpringEnergy(Pair<int> edge)
        {
            return (GetGlobalPos(edge.First) - GetGlobalPos(edge.Second)).LengthSquared();
        }

        private void PlaceNodeChildren(int node)
        {
            var children = sg.Children[node];

            if (!children.Any())
                return;

            foreach (var child in children)
            {
                oldPositions[child] = positions[child];
                placed[child] = false;
            }

            var childRadius = radii[children[0]];
            var sameRadii = children.All(x => radii[x] == childRadius);

            if (sameRadii)
            {
                var nodeRadius = radii[node];
                //var maxChildRadius = children.Max(x => radii[x]);

                // Generate honeycomb places
                var childPlaces = PackCircles(nodeRadius, childRadius, children.Count)
                    .Select(x => new ChildPlace { Position = x }).ToArray();
                Debug.Assert(childPlaces.Length >= children.Count, "packedCircleCenters.Length >= children.Count");
                //foreach (var child in children)
                //    positions[child] = Vector2.Zero;

                // Place externally-connected children
                foreach (var child in children)
                {
                    if (!nodeExternalConnections.TryGetValue(child, out var connections))
                        continue;
                    // todo: refactor external connections to not include initials and finals
                    var externalDirection = !connections.Any()
                        ? (sg.Next[child].Any() ? -Vector2.UnitX : Vector2.UnitX)
                        : connections.Aggregate(Vector2.Zero,
                              (x, y) => x + (GetGlobalPos(y) - GetGlobalPos(node)).Normalize()) / connections.Count;
                    externalDirection = externalDirection.LengthSquared() < MathHelper.Eps8
                        ? Vector2.UnitX
                        : externalDirection.Normalize();
                    var target = nodeRadius * externalDirection;

                    var closestPlace = childPlaces.Where(x => !x.IsOccupied)
                        .Minimal(x => (x.Position - target).LengthSquared());
                    closestPlace.Child = child;
                    positions[child] = closestPlace.Position;
                    placed[child] = true;
                }

                while (children.Any(x => !placed[x]))
                {
                    //await VisualizeAsync();
                    ApplySpringsForNodeChildren(node, false);
                    //await VisualizeAsync();

                    var mostRemotePlace = FindMostRemotePlace(node, childPlaces);
                    var child = children
                        .Where(x => !placed[x])
                        .Minimal(x => (positions[x] - mostRemotePlace.Position).LengthSquared());

                    mostRemotePlace.Child = child;
                    positions[child] = mostRemotePlace.Position;
                    placed[child] = true;
                }

                DoPlaceSwapping(node, childPlaces);
            }
            else
            {
                DoChildSwapping(node);
            }
        }

        private void DoPlaceSwapping(int node, ChildPlace[] childPlaces)
        {
            var hasImpreved = true;
            while (hasImpreved)
            {
                hasImpreved = false;
                var energy = ChildEnergy(node);
                foreach (var placePair in childPlaces.AllPairs())
                {
                    var place1 = placePair.First;
                    var place2 = placePair.Second;

                    if (place1.Child.HasValue && nodeExternalConnections.ContainsKey(place1.Child.Value) ||
                        place2.Child.HasValue && nodeExternalConnections.ContainsKey(place2.Child.Value))
                        continue;
                    SwapPlaces(place1, place2);
                    var newEnergy = ChildEnergy(node);
                    if (newEnergy < energy)
                    {
                        energy = newEnergy;
                        hasImpreved = true;
                    }
                    else
                    {
                        SwapPlaces(place1, place2);
                    }
                }
            }
        }

        private void SwapPlaces(ChildPlace place1, ChildPlace place2)
        {
            if (place1.Child.HasValue)
                positions[place1.Child.Value] = place2.Position;
            if (place2.Child.HasValue)
                positions[place2.Child.Value] = place1.Position;
            CodingHelper.Swap(ref place1.Child, ref place2.Child);
        }

        private void DoChildSwapping(int node)
        {
            var children = sg.Children[node];
            var hasImpreved = true;
            while (hasImpreved)
            {
                hasImpreved = false;
                var energy = ChildEnergy(node);
                foreach (var childPair in children.AllPairs())
                {
                    var child1 = childPair.First;
                    var child2 = childPair.Second;

                    if (radii[child1] != radii[child2] ||
                        nodeExternalConnections.ContainsKey(child1) ||
                        nodeExternalConnections.ContainsKey(child2))
                        continue;

                    CodingHelper.Swap(ref positions[child1], ref positions[child2]);
                    var newEnergy = ChildEnergy(node);
                    if (newEnergy < energy)
                    {
                        energy = newEnergy;
                        hasImpreved = true;
                    }
                    else
                    {
                        CodingHelper.Swap(ref positions[child1], ref positions[child2]);
                    }
                }
            }
        }

        private void RotateChildren(int node)
        {
            var children = sg.Children[node];
            if (children.Count == 0)
                return;

            var angleDiffs = new List<float>();
            foreach (var child in children)
            {
                if (!nodeExternalConnections.TryGetValue(child, out var externalConnections))
                    continue;
                var childLocal = positions[child];
                var childAngle = MathHelper.Atan2(childLocal.Y, childLocal.X);
                var childGlobal = GetGlobalPos(child);
                foreach (var externalConnection in externalConnections)
                {
                    var externalConnectionLoc = externalConnection;
                    var targetGlobal = GetGlobalPos(externalConnectionLoc);
                    var dir = targetGlobal - childGlobal;
                    var targetAngle = MathHelper.Atan2(dir.Y, dir.X);
                    var angleDiff = targetAngle - childAngle;
                    while (angleDiff > MathHelper.Pi)
                        angleDiff -= MathHelper.TwoPi;
                    while (angleDiff < -MathHelper.Pi)
                        angleDiff += MathHelper.TwoPi;
                    angleDiffs.Add(angleDiff);
                }
            }

            if (angleDiffs.Any())
            {
                var angleCorrection = -angleDiffs.Average();
                // todo: make Matrix2x2
                foreach (var child in children)
                    positions[child] = (new Vector3(positions[child], 0) * Matrix3x3.RotationZ(angleCorrection)).Xy;
            }
        }

        private static IEnumerable<Vector2> PackCircles(float parentRadius, float childRadius, int numChildren)
        {
            if (numChildren == 0)
            {
                yield break;
            }

            if (numChildren == 1)
            {
                yield return new Vector2(0, 0);
                yield break;
            }

            if (numChildren == 2)
            {
                yield return new Vector2(-childRadius, 0);
                yield return new Vector2(childRadius, 0);
                yield break;
            }

            var pointBound = parentRadius - childRadius;
            var pointBoundSq = pointBound.Sq();

            if (numChildren == 3)
            {
                var point = new Vector3(pointBound, 0, 0);
                point *= Matrix3x3.RotationZ(MathHelper.Pi / 6);
                yield return point.Xy;
                point *= Matrix3x3.RotationZ(MathHelper.TwoPi / 3);
                yield return point.Xy;
                point *= Matrix3x3.RotationZ(MathHelper.TwoPi / 3);
                yield return point.Xy;
                yield break;
            }

            if (numChildren == 4)
            {
                var point = new Vector3(pointBound, 0, 0);
                point *= Matrix3x3.RotationZ(MathHelper.PiOver4);
                yield return point.Xy;
                point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
                yield return point.Xy;
                point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
                yield return point.Xy;
                point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
                yield return point.Xy;
                yield break;
            }

            var xStride = 2 * childRadius;
            var yStride = xStride * MathHelper.Sin(MathHelper.PiOver3);

            var eps = MathHelper.Eps5 * childRadius;
            var comparsonPointBoundSq = (pointBound + eps).Sq();

            bool CanPlace(Vector2 p) => p.LengthSquared() <= comparsonPointBoundSq;

            var middleFit = (int)((parentRadius + eps) / childRadius);
            var symmetricCaseMiddleLength = MathHelper.Sqrt(pointBoundSq - (yStride / 2).Sq());
            var symmetricFit = (int)((symmetricCaseMiddleLength / 2 + eps) / childRadius);

            var oddRows = middleFit > symmetricFit;

            var firstY = oddRows ? 0 : yStride / 2;
            var minX = -(oddRows ? pointBound : MathHelper.Sqrt(pointBoundSq - firstY.Sq()));

            var next = new Vector2(minX, firstY);

            while (CanPlace(next))
            {
                yield return next;
                next.X += xStride;
            }

            next.X = minX;
            next.Y = firstY + yStride;

            var numRow = 1;

            while (Math.Abs(next.X) <= pointBound && Math.Abs(next.Y) <= pointBound)
            {
                if (numRow % 2 != 0)
                    next.X += childRadius;

                while (!CanPlace(next) && next.X < 0)
                    next.X += xStride;

                var xOffset = next.X;

                while (CanPlace(next))
                {
                    yield return next;
                    next.X += xStride;
                }

                next.X = xOffset;
                next.Y = -next.Y;

                while (CanPlace(next))
                {
                    yield return next;
                    next.X += xStride;
                }

                next.X = -pointBound;
                next.Y = -next.Y + yStride;

                numRow++;
            }
        }
        
        private ChildPlace FindMostRemotePlace(int parent, IEnumerable<ChildPlace> potentialPoints)
        {
            var children = sg.Children[parent];
            if (!children.Any(x => placed[x]))
                return potentialPoints.First();
            return potentialPoints.Maximal(p => children.Where(x => placed[x]).Min(x => (positions[x] - p.Position).LengthSquared()));
        }

        private void ApplySpringsForNodeChildren(int node, bool preventIntersections)
        {
            var children =  sg.Children[node];
            var prevEnergy = float.MaxValue;
            var deltaEnergy = float.MaxValue;
            while (deltaEnergy > MathHelper.Eps5)
            {
                foreach (var child in children)
                {
                    if (placed[child])
                        continue;
                    var sumDeltaPos = Vector2.Zero;
                    foreach (var neighbor in sg.LeveledNeighbors[child])
                    {
                        var toVector = positions[neighbor] - positions[child];
                        var distance = toVector.Length();
                        if (preventIntersections/* && placed[neighbor]*/)
                            distance -= (radii[child] + radii[neighbor]);
                        sumDeltaPos += toVector.Normalize() * distance * SpringConstant;
                    }
                    if (preventIntersections)
                        MoveWhilePossible(child, sumDeltaPos);
                    else
                        positions[child] += sumDeltaPos;
                }
                var currEnergy = ChildEnergy(node);
                deltaEnergy = prevEnergy - currEnergy;
                prevEnergy = currEnergy;
            }
        }

        private void CollapseCircles(int node)
        {
            var children = sg.Children[node];
            if (children.Count == 0)
            {
                radii[node] = LeafRadius;
                return;
            }
            foreach (var child in children)
                placed[child] = false;
            var orderedChildren = children.OrderBy(x => positions[x].LengthSquared()).ToArray();
            placed[orderedChildren.First()] = true;
            foreach (var child in orderedChildren.Skip(1))
            {
                ApplySpringsForNodeChildren(node, true);
                placed[child] = true;
            }
            var childCircles = children.Select(x => new Circle2(positions[x], radii[x])).ToArray();
            var boundingCircle = Circle2.BoundingCircle(childCircles);
            foreach (var child in children)
                positions[child] -= boundingCircle.Center;
            radii[node] = boundingCircle.Radius;
        }

        private void MoveWhilePossible(int node, Vector2 deltaPos)
        {
            var parent = sg.Parents[node];
            var children = sg.Children[parent];
            var child = node;

            // Assuming here that the child is not inside any of the circles.
            var ownRadius = radii[child];
            var remainingDeltaPos = deltaPos;
            var circles = children
                .ExceptSingle(child)
                .Select(x => new Circle2(positions[x], ownRadius + radii[x]))
                .ToArray();
            while (remainingDeltaPos.LengthSquared() >= MathHelper.Eps5)
            {
                var pos = positions[child];
                var newPos = pos + remainingDeltaPos;
                var segment = new LineSegment2(pos, newPos);
                var intersections = circles.SelectMany(x => x.Intersect(segment).Select(y => new Pair<Vector2, Circle2>(y, x)));
                var closestIntersection = intersections
                    .Where(x => (x.First - pos).LengthSquared() > MathHelper.Eps8)
                    .OrderBy(x => (x.First - pos).LengthSquared())
                    .FirstOrNull();
                if (closestIntersection.HasValue)
                {
                    var interPoint = closestIntersection.Value.First;
                    var interCircle = closestIntersection.Value.Second;
                    positions[child] = interPoint;
                    var normal = (interPoint - interCircle.Center).Normalize();
                    var tangent = new Vector2(normal.Y, -normal.X);
                    remainingDeltaPos = tangent * Vector2.Dot(tangent, newPos - interPoint);
                }
                else if (!circles.Any(x => x.Contains(newPos)))
                {
                    positions[child] = newPos;
                    remainingDeltaPos = Vector2.Zero;
                }
                else
                {
                    remainingDeltaPos = Vector2.Zero;
                }
            }
        }
    }
}