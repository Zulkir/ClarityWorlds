using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.StoryLayout.Building 
{
    public class BuildingStoryPath : IStoryPath
    {
        private struct PathSegment
        {
            public Vector3 StartPoint;
            public Vector3 Direction;
            public float Length;

            public PathSegment(Vector3 startPoint, Vector3 nextPoint)
            {
                StartPoint = startPoint;
                var rawDirection = nextPoint - startPoint;
                Length = rawDirection.Length();
                Direction = rawDirection / Length;
            }
        }

        public bool HasFinished { get; private set; }
        
        private readonly BuildingStoryLayoutPlacementAlgorithm pa;
        private readonly IStoryGraph sg;
        private readonly List<BezierQuadratic3> beziers;
        private readonly PathSegment[] path;
        private readonly DirectStoryPath directPath;
        private CameraProps currentCameraProps;
        private float curentTimestamp;
        private float currentDist;
        private int currentSegment;
        private const float MinSpeed = 5f;
        private const float MaxSpeed = 20f;
        private const float StraightDistanceBias = 0.2f;
        private const float StraightDistanceForMaxSpeed = 15f;
        private const float BezierTolerance = 0.0001f;

        public float MaxRemainingTime => path.Skip(currentSegment).Sum(x => x.Length / MinSpeed) - 
                                                                         currentDist / MinSpeed;

        public BuildingStoryPath(BuildingStoryLayoutPlacementAlgorithm pa, IStoryGraph sg, CameraProps initialCameraProps, int endNodeId, NavigationState navigationState, bool interLevel)
        {
            this.pa = pa;
            this.sg = sg;
            beziers = new List<BezierQuadratic3>();
            var closestNode = sg.Leaves.Minimal(x =>
                (BuildingConstants.LeafHalfSize.Depth * Vector3.UnitZ * GetGlobalTransform(x) -
                 initialCameraProps.Frame.Eye).LengthSquared());
            //var closestNodeGlobalTransform = GetGlobalTransform(endNodeId);
            //var closestNodeCenter = Vector3.Zero * closestNodeGlobalTransform;
            //var closestNodeConnector = BuildingConstants.LeafHalfSize.Depth * Vector3.UnitZ * closestNodeGlobalTransform;
            if (closestNode != endNodeId || interLevel)
            {
                if (!interLevel && sg.TryFindShortestPath(closestNode, endNodeId, out var nodePath))
                {
                    foreach (var edge in nodePath.SequentialPairs())
                    {
                        IEnumerable<BezierQuadratic3> pathToAdd;
                        if (pa.Lanes.TryGetValue(edge, out var lane))
                            pathToAdd = lane.GlobalPath;
                        else if (pa.Lanes.TryGetValue(edge.Reverse(), out lane))
                            pathToAdd = lane.GlobalPath.Reverse().Select(x => x.Reverse());
                        else
                            throw new Exception("Path search returned an unexisting path.");
                        beziers.AddRange(pathToAdd.Skip(1).ExceptLast());
                    }
                }
                else
                {
                    var endNode = sg.NodeObjects[endNodeId];
                    var aEndFocus = endNode.GetComponent<IFocusNodeComponent>();
                    var endCameraInfo = aEndFocus.DefaultViewpointMechanism.FixedCamera.GetProps();
                    directPath = new DirectStoryPath(initialCameraProps, endCameraInfo, 1f);
                }
            }

            switch (navigationState)
            {
                case NavigationState.AtNode:
                    break;
                case NavigationState.AtForwardFork:
                {
                    var nextEdge = new Pair<int>(endNodeId, sg.Next[endNodeId].First());
                    var nextLane = pa.Lanes[nextEdge];
                    var lastBezier = nextLane.GlobalPath.First();
                    var point = lastBezier.At(1);
                    var tangent = lastBezier.TangentAt(1);
                    beziers.Add(BezierQuadratic3.Straight(point - tangent, point));
                    break;
                }
                case NavigationState.AtBackwardFork:
                {
                    var nextEdge = new Pair<int>(sg.Previous[endNodeId].First(), endNodeId);
                    var nextLane = pa.Lanes[nextEdge];
                    var lastBezier = nextLane.GlobalPath.Last().Reverse();
                    var point = lastBezier.At(1);
                    var tangent = lastBezier.TangentAt(1);
                    beziers.Add(BezierQuadratic3.Straight(point - tangent, point));
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(navigationState), navigationState, null);
            }

            foreach (var indexPair in Enumerable.Range(0, beziers.Count).SequentialPairs())
            {
                if (beziers[indexPair.First].P2 != beziers[indexPair.Second].P0)
                {
                    var avg = (beziers[indexPair.First].P2 + beziers[indexPair.Second].P0) / 2;
                    beziers[indexPair.First] = beziers[indexPair.First].With((ref BezierQuadratic3 b) => b.P2 = avg);
                    beziers[indexPair.Second] = beziers[indexPair.Second].With((ref BezierQuadratic3 b) => b.P0 = avg);
                }
            }

            if (!beziers.Any())
                beziers.Add(BezierQuadratic3.Straight(initialCameraProps.Frame.Eye,
                    initialCameraProps.Frame.Eye + initialCameraProps.Frame.Forward));

            path = beziers
                .SelectMany(x => x.ToEnumPolyline(BezierTolerance))
                .SequentialPairs()
                .Select(x => new PathSegment(x.First, x.Second))
                .Where(x => x.Length > 0)
                .ToArray();

            currentCameraProps = initialCameraProps;
        }

        private Transform GetGlobalTransform(int node)
        {
            var local = pa.RelativeTransforms[node];
            return sg.Parents[node] == -1 ? local : local * GetGlobalTransform(sg.Parents[node]);
        }

        public CameraProps GetCurrentCameraProps()
        {
            return currentCameraProps;
        }

        public void Update(FrameTime frameTime)
        {
            if (HasFinished)
                return;

            if (path.Length == 0)
            {
                HasFinished = true;
                return;
            }

            curentTimestamp += frameTime.DeltaSeconds;
            var speed = GetSpeed(currentSegment, currentDist);
            var dist = currentDist + frameTime.DeltaSeconds * speed;

            while (dist > 0)
            {
                if (dist > path[currentSegment].Length)
                {
                    if (currentSegment == path.Length - 1)
                    {
                        HasFinished = true;
                        currentDist = path[currentSegment].Length;
                        dist = 0;
                    }
                    else
                    {
                        dist -= path[currentSegment].Length;
                        currentSegment++;
                    }
                }
                else
                {
                    currentDist = dist;
                    dist = 0;
                }
            }

            var segment = path[currentSegment];

            var eye = segment.StartPoint + segment.Direction * currentDist +
                      Vector3.UnitY * BuildingConstants.LeafHalfSize.Height;

            Vector3 forward;
            if (segment.Direction.Xz.LengthSquared() > MathHelper.Eps8)
            {
                forward = segment.Direction.Y == 0
                    ? segment.Direction
                    : new Vector3(segment.Direction.X, 0, segment.Direction.Z).Normalize();
            }
            else
            {
                var amount = currentDist / segment.Length;
                forward = segment.StartPoint.X > 0
                    ? Vector3.UnitX * Quaternion.RotationY(MathHelper.Pi * amount)
                    : -Vector3.UnitX * Quaternion.RotationY(MathHelper.Pi * amount);
            }

            //var forward = new Vector3(segment.Direction.X, 0, segment.Direction.Z).Normalize();
            var target = eye + forward * speed / 2; // todo: to constants
            //var up = Math.Abs(Vector3.Cross(forward, Vector3.UnitY).LengthSquared()) < MathHelper.Eps8
            //    ? eye.X < 0
            //        ? -Vector3.UnitX
            //        : Vector3.UnitX
            //    : Vector3.UnitY;
            var up = Vector3.UnitY;

            currentCameraProps = new CameraProps(target, new CameraFrame(eye, forward, up),
                new CameraProjection(0.1f, 100f, MathHelper.PiOver4));
        }

        private float GetSpeed(int segment, float localDistance)
        {
            var distToPrev = localDistance;
            var i = segment;
            while (i > 0 && Math.Abs(Vector3.Cross(path[i - 1].Direction, path[i].Direction).Length()) < 0.01f)
            {
                distToPrev += path[i - 1].Length;
                i--;
            }

            i = segment;
            var distToNext = path[i].Length - localDistance;
            while (i < path.Length - 1 &&
                   Math.Abs(Vector3.Cross(path[i].Direction, path[i + 1].Direction).Length()) < 0.01f)
            {
                distToNext += path[i + 1].Length;
                i++;
            }

            var dist = MathHelper.Clamp(Math.Min(distToPrev, distToNext) - StraightDistanceBias, 0,
                StraightDistanceForMaxSpeed);
            return MathHelper.Lerp(MinSpeed, MaxSpeed, dist / StraightDistanceForMaxSpeed);
        }
    }
}