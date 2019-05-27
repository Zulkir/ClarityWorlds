using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Worlds.StoryLayouts.NestedSpheres
{
    public class NestedSpheresStorySpringModel
    {
        private class ChildPlace
        {
            public Vector3 Position;
            public int? Child;

            public bool IsOccupied => Child.HasValue;
        }

        private readonly ICoroutineService coroutineService;
        private readonly IStoryGraph sg;
        private readonly Action<NestedSpheresStorySpringModel> visualize;
        private readonly Dictionary<int, List<int>> nodeExternalConnections;

        private readonly Vector3[] positions;
        private readonly Vector3[] oldPositions;

        private readonly float[] radii;
        private readonly bool[] placed;
        private readonly float[] rotations;

        private const float LeafRadius = 4f;
        private const float SpringConstant = 0.1f;
        private const float IterationDeltaT = 0.1f;
        private const float BasicSphereZOffset = 1.8027756377319946f;

        public Vector3 GetPosition(int index) => positions[index];
        public float GetVisualRadius(int index) => placed[index] ? radii[index] : 0;
        public float GetRotation(int index) => rotations[index];

        public NestedSpheresStorySpringModel(ICoroutineService coroutineService, Action<NestedSpheresStorySpringModel> visualize, IStoryGraph sg)
        {
            Debug.Assert(SpringConstant * IterationDeltaT < 1, "SpringConstant * IterationDeltaT < 1");
            this.coroutineService = coroutineService;
            this.visualize = visualize;
            this.sg = sg;
            nodeExternalConnections = BuildNodeExternalConnections(sg);
            positions = new Vector3[sg.EnoughIntegers.Count];
            oldPositions = new Vector3[sg.EnoughIntegers.Count];
            radii = new float[sg.EnoughIntegers.Count];
            placed = new bool[sg.EnoughIntegers.Count];
            rotations = new float[sg.EnoughIntegers.Count];
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

        private static bool IsIndirectParent(IStoryGraph sg, int child, int parent) =>
            EnumerateParentChain(sg, child).Any(x => x == parent);

        private static IEnumerable<int> EnumerateParentChain(IStoryGraph sg, int node)
        {
            var parent = sg.Parents[node];
            while (parent != sg.Parents[sg.Root])
            {
                yield return parent;
                parent = sg.Parents[parent];
            }
        }

        private Vector3 GetGlobalPos(int node)
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
            var radius = CalcMinRadiusForChildren(sg.Children[node].Count, maxChildRadius);
            radii[node] = radius;
        }

        private static float CalcMinRadiusForChildren(int numChildren, float childRadius)
        {
            var positions = PackSpheres2(0, childRadius, numChildren);
            var boundingSphere = Common.Numericals.Geometry.Sphere.BoundingSphere(positions.Select(x => new Common.Numericals.Geometry.Sphere(x, childRadius)).ToArray());
            return boundingSphere.Radius + boundingSphere.Center.Length();

            switch (numChildren)
            {
                case 1: return childRadius;
                case 2: return 2 * childRadius;
                case 3: return childRadius * (1f / MathHelper.Sin(MathHelper.PiOver3) + 1);
                //case 4: return childRadius * (1 + MathHelper.Sqrt2);
                //case 5:
                //case 6:
                //case 7: return 3 * childRadius;
                //case 8:
                //case 9:
                //case 10: return 4 * childRadius;
                //case 11:
                //case 12:
                //case 13:
                //case 14:
                //case 15:
                //case 16:
                //case 17:
                //case 18:
                //case 19: return 5 * childRadius;
                default:
                    {
                        var rowLength = 4;
                        while (PackSpheres(rowLength * childRadius, childRadius, numChildren).Count() < numChildren)
                            rowLength++;
                        return rowLength * childRadius;
                    };
            }
        }

        private void RunAdjustmentLoop()
        {
            AdjustmentStep();
            //await AdjustmentStep();

            //float prevEnergy = float.MaxValue;
            //float deltaEnergy = float.MaxValue;
            //
            //while (deltaEnergy > MathHelper.Eps5)
            //{
            //    await AdjustmentStep();
            //    var currEnergy = TotalEnergy();
            //    deltaEnergy = prevEnergy - currEnergy;
            //    prevEnergy = currEnergy;
            //}
        }

        private void AdjustmentStep()
        {
            placed[sg.Root] = true;
            foreach (var node in sg.HierarchyBreadthFirst())
                PlaceNodeChildren(node);
            //for (int i = 0; i < 2; i++)
            foreach (var node in sg.HierarchyBreadthFirst().Reverse())
                CollapseCircles(node);
            CollapseCircles(sg.Root);
            for (int i = 0; i < 5; i++)
            foreach (var node in sg.HierarchyBreadthFirst())
                RotateChildren(node);
            foreach (var leaf in sg.Leaves)
                RotateLeaves(leaf);
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
                var childPlaces = PackSpheres2(nodeRadius, childRadius, children.Count)
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
                    ? (sg.Next[child].Any() ? -Vector3.UnitX : Vector3.UnitX)
                        : connections.Aggregate(Vector3.Zero,
                              (x, y) => x + (GetGlobalPos(y) - GetGlobalPos(node)).Normalize()) / connections.Count;
                externalDirection = externalDirection.LengthSquared() < MathHelper.Eps8
                    ? Vector3.UnitX
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
                        //await VisualizeAsync();
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
                        //await VisualizeAsync();
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

            var prevAngle = float.MaxValue;
            var deltaAngle = float.MaxValue;

            var rotations = new List<Quaternion>();
            while (deltaAngle > MathHelper.Eps5)
                {
                var angle = 0f;
                rotations.Clear();
                foreach (var child in children)
                {
                    if (!nodeExternalConnections.TryGetValue(child, out var externalConnections))
                        continue;
                    var childLocalDir = positions[child].Normalize();
                    var childGlobal = GetGlobalPos(child);
                    foreach (var externalConnection in externalConnections)
                    {
                        var externalConnectionLoc = externalConnection;
                        var targetGlobal = GetGlobalPos(externalConnectionLoc);
                        var dir = (targetGlobal - childGlobal).Normalize();
                        var cross = Vector3.Cross(childLocalDir, dir);
                        var crossLen = cross.Length();
                        if (crossLen > MathHelper.Eps5)
                        {
                            var crossAngle = MathHelper.AsinSafe(crossLen);
                            var crossAxis = cross / crossLen;
                            angle += Math.Abs(crossAngle);
                            rotations.Add(Quaternion.RotationAxis(crossAxis, -crossAngle * 0.1f));
                    }
                }
                }

                if (rotations.Any())
                {
                    var avg = rotations.Aggregate((x, y) => x + y) / rotations.Count;
                    foreach (var child in children)
                        positions[child] *= avg;
                }

                deltaAngle = prevAngle - angle;
                prevAngle = angle;
            }
        }

        private void RotateLeaves(int node)
        {
            var ownPos = GetGlobalPos(node);
            var dir = Vector2.Zero;
            var count = 0;
            foreach (var prev in sg.LeveledPrevious[node])
            {
                var toward = (ownPos - GetGlobalPos(prev)).Xz;
                if (toward.Length() > MathHelper.Eps5)
                    dir += toward.Normalize();
                count++;
        }
            foreach (var prev in sg.LeveledNext[node])
            {
                var toward = (GetGlobalPos(prev) - ownPos).Xz;
                if (toward.Length() > MathHelper.Eps5)
                    dir += toward.Normalize();
                count++;
            }

            if (count == 0)
        {
                rotations[node] = 0;
                return;
            }

            dir /= count;
            if (dir.Length() < MathHelper.Eps5)
            {
                rotations[node] = 0;
                return;
            }

            var rotation = MathHelper.Atan2(dir.Y, dir.X);
            rotations[node] = rotation;// + MathHelper.PiOver2;
        }

        private static Vector3[] PackSpheresBySprings(float childRadius, int numChildren)
        {
            var rnd = new Random(1);
            var points = Enumerable.Range(0, numChildren).Select(x => childRadius * new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble())).ToArray();

            float prevEnergy = float.MaxValue;
            float deltaEnergy = float.MaxValue;
            
            while (deltaEnergy > MathHelper.Eps5)
            {
                var currEnergy = 0f;
                for (int i = 0; i < points.Length; i++)
                {
                    var childEnergy = 0f;
                    //childEnergy += points[i].Length();
                    points[i] *= 0.9f;
                    var offset = Vector3.Zero;

                    for (int j = 0; j < points.Length; j++)
                    {
                        if (i == j)
                            continue;
                        var vecDist = points[i] - points[j];
                        var dist = vecDist.Length();
                        var penetration = (2 * childRadius - dist) / childRadius;
                        if (penetration <= 0)
                            continue;
                        offset += vecDist.Normalize() * penetration / 2;
                        childEnergy += (penetration + 1).Sq();
        }
                    points[i] += offset;
                    currEnergy += childEnergy;
                }

                deltaEnergy = prevEnergy - currEnergy;
                prevEnergy = currEnergy;
            }

            return points;
        }
        
        private static IEnumerable<Vector3> PackSpheres(float parentRadius, float childRadius, int numChildren)
        {
            if (numChildren == 0)
            {
                yield break;
            }

            if (numChildren == 1)
            {
                yield return new Vector3(0, 0, 0);
                yield break;
            }

            if (numChildren == 2)
            {
                yield return new Vector3(-childRadius, 0, 0);
                yield return new Vector3(childRadius, 0, 0);
                yield break;
            }

            var pointBound = parentRadius - childRadius;
            var pointBoundSq = pointBound.Sq();

            if (numChildren == 3)
            {
                var point = new Vector3(pointBound, 0, 0);
                point *= Matrix3x3.RotationZ(MathHelper.Pi / 6);
                yield return point;
                point *= Matrix3x3.RotationZ(MathHelper.TwoPi / 3);
                yield return point;
                point *= Matrix3x3.RotationZ(MathHelper.TwoPi / 3);
                yield return point;
                yield break;
            }

            //if (numChildren == 4)
            //{
            //    // todo
            //    var point = new Vector3(pointBound, 0, 0);
            //    point *= Matrix3x3.RotationZ(MathHelper.PiOver4);
            //    yield return point;
            //    point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
            //    yield return point;
            //    point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
            //    yield return point;
            //    point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
            //    yield return point;
            //    yield break;
            //}
            //
            //if (numChildren == 5)
            //{
            //    // todo
            //    var point = new Vector3(pointBound, 0, 0);
            //    point *= Matrix3x3.RotationZ(MathHelper.PiOver4);
            //    yield return point;
            //    point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
            //    yield return point;
            //    point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
            //    yield return point;
            //    point *= Matrix3x3.RotationZ(MathHelper.PiOver2);
            //    yield return point;
            //    yield break;
            //}

            var xStride = 2 * childRadius;
            var yStride = xStride * MathHelper.Sin(MathHelper.PiOver3);
            var zStride = childRadius * BasicSphereZOffset;

            var eps = MathHelper.Eps5 * childRadius;
            var comparsonPointBoundSq = (pointBound + eps).Sq();

            bool CanPlace(Vector3 p) => p.LengthSquared() <= comparsonPointBoundSq;

            //var middleFit = (int)((parentRadius + eps) / childRadius);
            //var symmetricCaseMiddleLength = MathHelper.Sqrt(pointBoundSq - (yStride / 2).Sq());
            //var symmetricFit = (int)((symmetricCaseMiddleLength / 2 + eps) / childRadius);

            //var oddRows = true;// middleFit > symmetricFit;

            var minX = -pointBound;

            var next = new Vector3(minX, 0, 0);

            //while (CanPlace(next))
            //{
            //    yield return next;
            //    next.X += xStride;
            //}
            //
            //next.X = minX;
            //next.Y = 0;

            var firstPlane = true;
            var oddPlane = false;
            next.Z = 0;
            while (Math.Abs(next.Z) <= pointBound)
            {
                var yOffset = oddPlane ? yStride / 3 : 0;
                // Yield Plane
                {
                    var firstRow = true;
                    var oddRow = false;
                    next.Y = yOffset;
                    while (Math.Abs(next.Y) <= pointBound)
                    {
                        // Yield Row
                        var xOffset = (oddPlane ^ oddRow) ? childRadius : 0;
                        next.X = minX + xOffset;
                        while (!CanPlace(next) && next.X < 0)
                            next.X += xStride;
                        var startX = next.X;

                        while (CanPlace(next))
                        {
                            yield return next;
                            next.X += xStride;
                        }

                        if (!firstRow)
                        {
                            next.X = startX;
                            next.Y = -(next.Y - yOffset) + yOffset;

                            while (CanPlace(next))
                            {
                                yield return next;
                                next.X += xStride;
                            }
                        }
                        else
                        {
                            firstRow = false;
                        }

                        next.Y = -(next.Y - yOffset) + yOffset + yStride;
                        oddRow = !oddRow;
                    }
                }

                if (!firstPlane)
                {
                    next.Z = -next.Z;

                    // Yield Plane
                    {
                        var firstRow = true;
                        var oddRow = false;
                        next.Y = yOffset;
                        while (Math.Abs(next.Y) <= pointBound)
                        {
                            // Yield Row
                            var xOffset = (oddPlane ^ oddRow) ? childRadius : 0;
                            next.X = xOffset;
                            while (!CanPlace(next) && next.X < 0)
                                next.X += xStride;
                            var startX = next.X;

                            while (CanPlace(next))
                            {
                                yield return next;
                                next.X += xStride;
                            }

                            if (!firstRow)
                            {
                                next.X = startX;
                                next.Y = -(next.Y - yOffset) + yOffset;

                                while (CanPlace(next))
                                {
                                    yield return next;
                                    next.X += xStride;
                                }
                            }
                            else
                            {
                                firstRow = false;
                            }

                            next.Y = -(next.Y - yOffset) + yOffset + yStride;
                            oddRow = !oddRow;
                        }
                    }
                }
                else
                {
                    firstPlane = false;
                }
                
                next.Z = -next.Z + zStride;
                oddPlane = !oddPlane;
            }
        }

        private static Vector3[] PackSpheres2(float parentRadius, float childRadius, int numChildren)
        {
            if (numChildren == 0)
                return EmptyArrays<Vector3>.Array;
            if (numChildren == 1)
                return new []{Vector3.Zero};

            var positions = new Vector3[numChildren];
            var directions = new Vector3[numChildren];
            for (int i = 0; i < numChildren; i++)
            {
                var amount = (float)i / (numChildren - 1);
                var x = MathHelper.Lerp(-1, 1, amount);
                var r = MathHelper.Cos(x);
                var a = MathHelper.TwoPi * amount;
                var y = r * MathHelper.Sin(a);
                var z = r * MathHelper.Cos(a);
                positions[i] = numChildren * childRadius * new Vector3(x, y, z);
                directions[i] = positions[i].Normalize();
        }

            var prevEnergy = float.MaxValue;
            var deltaEnergy = float.MaxValue;

            while (deltaEnergy > MathHelper.Eps5)
            {
                for (int i = 0; i < numChildren; i++)
                    positions[i] *= 0.9f;

                Array.Sort(positions, (p1, p2) => p1.LengthSquared().CompareTo(p2.LengthSquared()));

                for (int i = 1; i < numChildren; i++)
                {
                    var childToMove = i;
                    for (int j = 0; j < i; j++)
                    {
                        var obstacle = j;
                        var from = positions[childToMove] - positions[obstacle];
                        var dist = from.Length();
                        if (dist >= 2 * childRadius)
                        continue;
                        var offset = !(dist < MathHelper.Eps5) 
                            ? from.Normalize() * (2 * childRadius - dist) + 0.01f * directions[i] 
                            : childRadius * directions[i];
                        positions[i] += offset;
                    }
                }

                var currEnergy = positions.Select(x => x.LengthSquared()).Sum();
                deltaEnergy = prevEnergy - currEnergy;
                prevEnergy = currEnergy;
            }

            var center = positions.Aggregate((x, y) => x + y) / numChildren;
            for (int i = 0; i < numChildren; i++)
                positions[i] -= center;

            return positions;
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
            var children = sg.Children[node];
                var prevEnergy = float.MaxValue;
                var deltaEnergy = float.MaxValue;

                while (deltaEnergy > MathHelper.Eps5)
                {
                foreach (var child in children)
                {
                    if (placed[child])
                        continue;
                    var sumDeltaPos = Vector3.Zero;
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
                //await VisualizeAsync(0.1f);
                ApplySpringsForNodeChildren(node, true);
                placed[child] = true;
        }
            var childSpheres = children.Select(x => new Common.Numericals.Geometry.Sphere(positions[x], radii[x])).ToArray();
            var boundingCircle = Common.Numericals.Geometry.Sphere.BoundingSphere(childSpheres);
            foreach (var child in children)
                positions[child] -= boundingCircle.Center;
            radii[node] = boundingCircle.Radius;
        }

        private void MoveWhilePossible(int node, Vector3 offset)
        {
            var parent = sg.Parents[node];
            var children = sg.Children[parent];
            var child = node;

            // Assuming here that the child is not inside any of the spheres.
            var ownRadius = radii[child];
            var remainingDeltaPos = offset;
            var spheres = children
                //.Where(x => placed[x])
                .ExceptSingle(child)
                .Select(x => new Common.Numericals.Geometry.Sphere(positions[x], ownRadius + radii[x]))
                .ToArray();

                var pos = positions[child];
            var target = pos + offset;

            var prevDist = (pos - target).Length();
            var deltaDist = float.MaxValue;

            while (deltaDist > MathHelper.Eps8)
                {
                pos = Vector3.Lerp(pos, target, 0.1f);
                foreach (var sphere in spheres)
                {
                    var sphereDist = (pos - sphere.Center).Length() - sphere.Radius;
                    if (sphereDist < 0)
                        pos = Vector3.Lerp(pos, (pos - sphere.Center).Normalize() * sphere.Radius, 0.5f);
                }
                var dist = (pos - target).Length();
                deltaDist = prevDist - dist;
                prevDist = dist;
            }

            var prevPos = pos;
            var deltaPos = new Vector3(1, 1, 1);

            while (deltaPos.LengthSquared() > MathHelper.Eps8)
                {
                var posLength = pos.Length();
                if (posLength > MathHelper.Eps5)
                    pos -= pos / posLength * Math.Min(ownRadius * 0.1f, posLength);
                foreach (var sphere in spheres)
                {
                    var sphereDist = (pos - sphere.Center).Length() - sphere.Radius;
                    if (sphereDist < 0)
                        pos = Vector3.Lerp(pos, sphere.Center + (pos - sphere.Center).Normalize() * sphere.Radius, 0.5f);
                }
                deltaPos = pos - prevPos;
                prevPos = pos;
            }

            positions[child] = pos;
        }
    }
}