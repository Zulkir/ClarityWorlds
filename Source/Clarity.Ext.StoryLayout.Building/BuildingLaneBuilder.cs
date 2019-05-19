using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.StoryGraph;

namespace Clarity.Ext.StoryLayout.Building
{
    public class BuildingLaneBuilder
    {
        private readonly IStoryGraph sg;
        private readonly Func<int, Transform> getLocalTransform;
        private readonly Dictionary<Pair<int>, BuildingLane> lanes;
        private readonly List<Vector3> rawPoints;
        private readonly List<BuildingRawLaneSegment> rawSegments;
        private readonly Dictionary<int, List<BuildingRawLaneSegment>> rawSegmentsByParentNodes;
        private BuildingRawLaneSegment currentLaneSegment;
        private BuildingLane currentLane;

        public IReadOnlyDictionary<Pair<int>, BuildingLane> Lanes => lanes;
        public IReadOnlyList<Vector3> RawPoints => rawPoints;
        public IReadOnlyDictionary<int, List<BuildingRawLaneSegment>> RawSegmentsByParentNodes => rawSegmentsByParentNodes;

        public BuildingLaneBuilder(IStoryGraph sg, Func<int, Transform> getLocalTransform)
        {
            this.sg = sg;
            this.getLocalTransform = getLocalTransform;
            lanes = new Dictionary<Pair<int>, BuildingLane>();
            rawPoints = new List<Vector3>();
            rawSegments = new List<BuildingRawLaneSegment>();
            rawSegmentsByParentNodes = new Dictionary<int, List<BuildingRawLaneSegment>>();
        }

        public void StartLaneSegment(BuildingStoryLayoutLanePartType type, bool startsAtView)
        {
            currentLaneSegment = new BuildingRawLaneSegment
            {
                StartPoint = rawPoints.Count,
                Type = type,
                StartsAtView = startsAtView
            };
        }

        public void EndLaneSegment(int parentNode, bool endsAtView)
        {
            currentLaneSegment.EndsAtView = endsAtView;
            currentLaneSegment.NumPoints = rawPoints.Count - currentLaneSegment.StartPoint;
            currentLaneSegment.ParentNode = parentNode;
            rawSegments.Add(currentLaneSegment);
            rawSegmentsByParentNodes.GetOrAdd(parentNode, x => new List<BuildingRawLaneSegment>()).Add(currentLaneSegment);
        }

        public void AddRawPoint(Vector3 point)
        {
            rawPoints.Add(point);
        }

        public void StartLane(Pair<int> edge)
        {
            currentLane = new BuildingLane
            {
                Edge = edge,
            };
        }

        public void EndLane(int disambiguator)
        {
            currentLane.Disambiguator = disambiguator;
            currentLane.GlobalPath = BuildGlobalPath(currentLane.Edge);
            currentLane.CurveIndexBeforeFwdFork = 1;
            currentLane.CurveIndexAfterBwdFork = currentLane.GlobalPath.Count - 2;
            lanes.Add(currentLane.Edge, currentLane);
            rawSegments.Clear();
        }

        private List<BezierQuadratic3> BuildGlobalPath(Pair<int> edge)
        {
            var globalPath = new List<BezierQuadratic3>();
            var pairsOfGlobalSegments = EnumerageGlobalPointsForLane()
                .SequentialPairs()
                //.SelectMany(GenerateElevators)
                .Select(x => new LineSegment3(x.First, x.Second))
                .SequentialPairs()
                .ToArray();
            var firstPoint = pairsOfGlobalSegments.First().First.Point1;
            var firstGlobalTransform = GetGlobalTransform(edge.First);
            globalPath.Add(new BezierQuadratic3(
                (BuildingConstants.LeafHalfSize.Depth - BuildingConstants.TerminalLaneDepth) * Vector3.UnitZ * firstGlobalTransform,
                (BuildingConstants.LeafHalfSize.Depth) * Vector3.UnitZ * firstGlobalTransform,
                firstPoint));
            var prevSegmentEndPairEnd = firstPoint;
            foreach (var segmentPair in pairsOfGlobalSegments)
            {
                var seg1 = segmentPair.First;
                var seg2 = segmentPair.Second;
                var bezierPoint0 = seg1.LengthSq / 4 > BuildingConstants.BezierSegmentMaxLengthSq
                    ? seg1.Point2 + (seg1.Point1 - seg1.Point2).Normalize() * BuildingConstants.BezierSegmentMaxLength
                    : (seg1.Point1 + seg1.Point2) / 2;
                var bezierPoint2 = seg2.LengthSq / 4 > BuildingConstants.BezierSegmentMaxLengthSq
                    ? seg2.Point1 + (seg2.Point2 - seg2.Point1).Normalize() * BuildingConstants.BezierSegmentMaxLength
                    : (seg2.Point1 + seg2.Point2) / 2;
                globalPath.Add(new BezierQuadratic3(prevSegmentEndPairEnd, (prevSegmentEndPairEnd + bezierPoint0) / 2, bezierPoint0));
                globalPath.Add(new BezierQuadratic3(bezierPoint0, seg1.Point2, bezierPoint2));
                prevSegmentEndPairEnd = bezierPoint2;
            }
            var lastPoint = pairsOfGlobalSegments.Last().Second.Point2;
            globalPath.Add(new BezierQuadratic3(prevSegmentEndPairEnd, (prevSegmentEndPairEnd + lastPoint) / 2, lastPoint));
            var lastGlobalTransform = GetGlobalTransform(edge.Second);
            globalPath.Add(new BezierQuadratic3(
                lastPoint,
                (BuildingConstants.LeafHalfSize.Depth) * Vector3.UnitZ * lastGlobalTransform,
                (BuildingConstants.LeafHalfSize.Depth - BuildingConstants.TerminalLaneDepth) * Vector3.UnitZ * lastGlobalTransform));
            return globalPath;
        }

        private IEnumerable<Vector3> EnumerageGlobalPointsForLane()
        {
            foreach (var rawSegment in rawSegments)
            {
                var transform = GetGlobalTransform(rawSegment.ParentNode);
                for (int i = 0; i < rawSegment.NumPoints; i++)
                    yield return rawPoints[rawSegment.StartPoint + i] * transform;
            }
        }

        private Transform GetGlobalTransform(int node)
        {
            var localTransform = getLocalTransform(node);
            return node == sg.Root 
                ? localTransform 
                : localTransform * GetGlobalTransform(sg.Parents[node]);
        }

        private IEnumerable<Pair<Vector3>> GenerateElevators(Pair<Vector3> pair)
        {
            if (pair.First.Y == pair.Second.Y)
            {
                yield return pair;
                yield break;
            }

            if (pair.First.X > 0)
            {
                var p0 = pair.First;
                var p1 = p0 + new Vector3(0.5f * BuildingConstants.ElevatorOffset, 0, 0);
                var p2 = p0 + new Vector3(1.5f * BuildingConstants.ElevatorOffset, 0, 0.5f * BuildingConstants.ElevatorOffset);
                var p3 = p0 + new Vector3(1.5f * BuildingConstants.ElevatorOffset, 0, 0);

                var p6 = pair.Second;
                var p5 = p6 + new Vector3(0.5f * BuildingConstants.ElevatorOffset, 0, 0);

                yield return new Pair<Vector3>(p0, p1);
                yield return new Pair<Vector3>(p1, p2);
                yield return new Pair<Vector3>(p2, p3);
                yield return new Pair<Vector3>(p3, p5);
                yield return new Pair<Vector3>(p5, p6);
                yield break;
            }

            if (pair.First.X > 0)
            {
                var p0 = pair.First;
                var p1 = p0 + new Vector3(-0.5f * BuildingConstants.ElevatorOffset, 0, 0);
                var p2 = p0 + new Vector3(-1.5f * BuildingConstants.ElevatorOffset, 0, -0.5f * BuildingConstants.ElevatorOffset);
                var p3 = p0 + new Vector3(-1.5f * BuildingConstants.ElevatorOffset, 0, 0);

                var p6 = pair.Second;
                var p5 = p6 + new Vector3(-0.5f * BuildingConstants.ElevatorOffset, 0, 0);

                yield return new Pair<Vector3>(p0, p1);
                yield return new Pair<Vector3>(p1, p2);
                yield return new Pair<Vector3>(p2, p3);
                yield return new Pair<Vector3>(p3, p5);
                yield return new Pair<Vector3>(p5, p6);
                yield break;
            }
        }
    }
}