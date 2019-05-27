using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.StoryLayout.Building
{
    public enum BuildingPrimitiveType
    {
        Rectangle,
        Circle
    }

    public struct BuildingPrimitive
    {
        public Vector3 Scale;
        public Transform Transform;
        public BuildingPrimitiveType Type;
        public bool IsMainCorridor;

        public BuildingPrimitive(BuildingPrimitiveType type, Vector3 scale, Transform transform, bool isMainCorridor = false)
        {
            Scale = scale;
            Transform = transform;
            IsMainCorridor = isMainCorridor;
            Type = type;
        }
    }

    public struct BuildingWallSegment
    {
        public LineSegment3 Basement;
        public float Height;

        public BuildingWallSegment(Vector3 p1, Vector3 p2, float height) 
        {
            Basement = new LineSegment3(p1, p2);
            Height = height;
        }
    }

    public struct BuildingCorridorDiggingResult
    {
        public List<BuildingWallSegment> RawWallSegments;
        public List<BuildingWallSegment> WallSegments;
        public List<AaRectangle2> Floorings;
    }

    public unsafe class BuildingCorridorDigger
    {
        private readonly HashSet<Pair<Vector3>> uniqueSegments;
        private readonly HashSet<Vector3> uniquePoints;

        public BuildingCorridorDigger()
        {
            uniqueSegments = new HashSet<Pair<Vector3>>();
            uniquePoints= new HashSet<Vector3>();
        }
        
        public BuildingCorridorDiggingResult Dig(BuildingStoryLayoutPlacementAlgorithm placementAlgorithm, int node)
        {
            var ssg = placementAlgorithm.StoryGraph;
            if (!ssg.Children[node].Any())
                return new BuildingCorridorDiggingResult
                {
                    WallSegments = new List<BuildingWallSegment>(),
                    RawWallSegments = new List<BuildingWallSegment>(),
                    Floorings = new List<AaRectangle2>()
                };
            if (node == ssg.Root)
                return BuildElevators(placementAlgorithm, node);
            FillUniqueSegments(placementAlgorithm, node);
            var primitives = CreatePrimitives(placementAlgorithm, node);
            var wallSegments = BuildWallBasementManaged(primitives);
            var rawWallSegments = BuildRawWallSegments(primitives);
            var floorings = new List<AaRectangle2>();
            if (ssg.Parents[node] == ssg.Root)
            {
                var halfSize = placementAlgorithm.HalfSizes[node];
                floorings.Add(new AaRectangle2(Vector2.Zero, halfSize.Width, halfSize.Depth));
            }
            return new BuildingCorridorDiggingResult
            {
                WallSegments = wallSegments,
                RawWallSegments = rawWallSegments,
                Floorings = floorings
            };
        }

        private BuildingCorridorDiggingResult BuildElevators(BuildingStoryLayoutPlacementAlgorithm placementAlgorithm, int node)
        {
            const float offset = BuildingConstants.ElevatorOffset;
            var negativeElevatorCenter = -(placementAlgorithm.HalfSizes[node].Width + offset) * Vector3.UnitX;
            var negPoint0 = negativeElevatorCenter + new Vector3(offset, 0, offset);
            var negPoint1 = negativeElevatorCenter + new Vector3(-offset, 0, offset);
            var negPoint2 = negativeElevatorCenter + new Vector3(-offset, 0, -offset);
            var negPoint3 = negativeElevatorCenter + new Vector3(offset, 0, -offset);
            
            var positiveElevatorCenter =  (placementAlgorithm.HalfSizes[node].Width + offset) * Vector3.UnitX;
            var posPoint0 = positiveElevatorCenter + new Vector3(-offset, 0, -offset);
            var posPoint1 = positiveElevatorCenter + new Vector3(offset, 0, -offset);
            var posPoint2 = positiveElevatorCenter + new Vector3(offset, 0, offset);
            var posPoint3 = positiveElevatorCenter + new Vector3(-offset, 0, offset);

            var fullHeight = 2 * placementAlgorithm.HalfSizes[node].Height;

            var wallSegments = new List<BuildingWallSegment>
            {
                new BuildingWallSegment(negPoint0, negPoint1, fullHeight),
                new BuildingWallSegment(negPoint1, negPoint2, fullHeight),
                new BuildingWallSegment(negPoint2, negPoint3, fullHeight),
                new BuildingWallSegment(posPoint0, posPoint1, fullHeight),
                new BuildingWallSegment(posPoint1, posPoint2, fullHeight),
                new BuildingWallSegment(posPoint2, posPoint3, fullHeight),
            };

            var children = placementAlgorithm.StoryGraph.Children[node];
            var currentHeight = BuildingConstants.CeilingHeight;
            foreach (var child in children)
            {
                var verticalOffset = Vector3.UnitY * currentHeight;
                var floorHeight = 2 * placementAlgorithm.HalfSizes[child].Height + BuildingConstants.HeightMargin;
                var separatorHeight = floorHeight - BuildingConstants.CeilingHeight;
                wallSegments.Add(new BuildingWallSegment(negPoint3 + verticalOffset, negPoint0 + verticalOffset, separatorHeight));
                wallSegments.Add(new BuildingWallSegment(posPoint3 + verticalOffset, posPoint0 + verticalOffset, separatorHeight));
                currentHeight += floorHeight;
            }

            var floorings = new List<AaRectangle2>
            {
                new AaRectangle2(negativeElevatorCenter.Xz, offset, offset),
                new AaRectangle2(positiveElevatorCenter.Xz, offset, offset),
            };

            return new BuildingCorridorDiggingResult
            {
                WallSegments = wallSegments,
                RawWallSegments = wallSegments,
                Floorings = floorings
            };
        }

        private void FillUniqueSegments(BuildingStoryLayoutPlacementAlgorithm placementAlgorithm, int node)
        {
            var lanePoints = placementAlgorithm.LanePoints;

            uniqueSegments.Clear();
            uniquePoints.Clear();

            if (placementAlgorithm.LaneSegments.TryGetValue(node, out var lanes))
            {
                foreach (var lane in lanes)
                {
                    var currPoint = lanePoints[lane.StartPoint];
                    if (currPoint.Z != 0)
                        uniquePoints.Add(currPoint);
                    for (int i = 1; i < lane.NumPoints; i++)
                    {
                        var prevPoint = currPoint;
                        currPoint = lanePoints[lane.StartPoint + i];
                        if (currPoint.Z != 0)
                            uniquePoints.Add(currPoint);
                        if (prevPoint.Z != 0 || currPoint.Z != 0)
                            uniqueSegments.Add(new Pair<Vector3>(prevPoint, currPoint));
                    }
                }
            }
        }

        private List<BuildingPrimitive> CreatePrimitives(BuildingStoryLayoutPlacementAlgorithm placementAlgorithm, int node)
        {
            var sg = placementAlgorithm.StoryGraph;

            var primitives = new List<BuildingPrimitive>();

            var nodeScale = placementAlgorithm.HalfSizes[node].ToVector();
            var mainCorridorScale = new Vector3(nodeScale.X, nodeScale.Y, BuildingConstants.CorridorHalfWidth + MathHelper.Eps5);
            var mainCorridor = new BuildingPrimitive(BuildingPrimitiveType.Rectangle, mainCorridorScale, Transform.Identity, true);
            primitives.Add(mainCorridor);

            foreach (var seg in uniqueSegments)
            {
                var forward = seg.Second - seg.First;
                var halfWidth = forward.Length() / 2;
                var center = (seg.First + seg.Second) / 2;
                var scale = new Vector3(halfWidth, 1, BuildingConstants.CorridorHalfWidth);
                var transform = new Transform(1, Quaternion.RotationToFrame(forward, Vector3.UnitY), center);
                primitives.Add(new BuildingPrimitive(BuildingPrimitiveType.Rectangle, scale, transform));
            }

            foreach (var point in uniquePoints)
            {
                primitives.Add(new BuildingPrimitive(BuildingPrimitiveType.Circle, new Vector3(BuildingConstants.CorridorHalfWidth), Transform.Translation(point)));
            }

            foreach (var child in sg.Children[node])
            {
                var scale = placementAlgorithm.HalfSizes[child].ToVector();
                var pos = placementAlgorithm.RelativePositions[child];
                if (!sg.Children[child].Any())
                {
                    scale.Z += BuildingConstants.CorridorHalfWidth / 2;
                    pos.Z -= scale.Z - BuildingConstants.CorridorHalfWidth;
                }
                primitives.Add(new BuildingPrimitive(BuildingPrimitiveType.Rectangle, scale, Transform.Translation(pos)));
            }

            return primitives;
        }

        private List<BuildingWallSegment> BuildRawWallSegments(List<BuildingPrimitive> primitives)
        {
            var primitiveSegments = new List<LineSegment3>();
            for (int i = 0; i < primitives.Count; i++)
            {
                var primitive = primitives[i];
                if (primitive.IsMainCorridor)
                {
                    var p1 = new Vector3(-primitive.Scale.X, 0, primitive.Scale.Z) * primitive.Transform;
                    var p2 = new Vector3(-primitive.Scale.X, 0, -primitive.Scale.Z) * primitive.Transform;
                    var p3 = new Vector3(primitive.Scale.X, 0, -primitive.Scale.Z) * primitive.Transform;
                    var p4 = new Vector3(primitive.Scale.X, 0, primitive.Scale.Z) * primitive.Transform;
                    primitiveSegments.Add(new LineSegment3(p2, p3));
                    primitiveSegments.Add(new LineSegment3(p4, p1));
                }
                else if (primitive.Type == BuildingPrimitiveType.Rectangle)
                {
                    var p1 = new Vector3(-primitive.Scale.X, 0, primitive.Scale.Z) * primitive.Transform;
                    var p2 = new Vector3(-primitive.Scale.X, 0, -primitive.Scale.Z) * primitive.Transform;
                    var p3 = new Vector3(primitive.Scale.X, 0, -primitive.Scale.Z) * primitive.Transform;
                    var p4 = new Vector3(primitive.Scale.X, 0, primitive.Scale.Z) * primitive.Transform;
                    primitiveSegments.Add(new LineSegment3(p1, p2));
                    primitiveSegments.Add(new LineSegment3(p2, p3));
                    primitiveSegments.Add(new LineSegment3(p3, p4));
                    primitiveSegments.Add(new LineSegment3(p4, p1));
                }
                else
                {
                    var scale = primitive.Scale.X;
                    var currPoint = scale * Vector3.UnitX * primitive.Transform;
                    for (int j = 1; j < 33; j++)
                    {
                        var prevPoint = currPoint;
                        var angle = MathHelper.TwoPi * j / (33 - 1);
                        currPoint = (scale * new Vector3(MathHelper.Cos(angle), 0, MathHelper.Sin(angle))) * primitive.Transform;
                        primitiveSegments.Add(new LineSegment3(prevPoint, currPoint));
                    }
                }
            }
            return primitiveSegments.Select(x => new BuildingWallSegment(x.Point1, x.Point2, BuildingConstants.CeilingHeight)).ToList();
        }

        private static List<BuildingWallSegment> BuildWallBasementManaged(List<BuildingPrimitive> primitives)
        {
            var wallSegments = new List<BuildingWallSegment>();
            var primitiveSegments = new List<LineSegment3>();
            var filteredSegments1 = new List<LineSegment3>();
            var filteredSegments2 = new List<LineSegment3>();
            for (int i = 0; i < primitives.Count; i++)
            {
                var primitive = primitives[i];
                primitiveSegments.Clear();
                if (primitive.IsMainCorridor)
                {
                    var p1 = new Vector3(-primitive.Scale.X, 0, primitive.Scale.Z) * primitive.Transform;
                    var p2 = new Vector3(-primitive.Scale.X, 0, -primitive.Scale.Z) * primitive.Transform;
                    var p3 = new Vector3(primitive.Scale.X, 0, -primitive.Scale.Z) * primitive.Transform;
                    var p4 = new Vector3(primitive.Scale.X, 0, primitive.Scale.Z) * primitive.Transform;
                    primitiveSegments.Add(new LineSegment3(p2, p3));
                    primitiveSegments.Add(new LineSegment3(p4, p1));
                }
                else if (primitive.Type == BuildingPrimitiveType.Rectangle)
                {
                    var p1 = new Vector3(-primitive.Scale.X, 0, primitive.Scale.Z) * primitive.Transform;
                    var p2 = new Vector3(-primitive.Scale.X, 0, -primitive.Scale.Z) * primitive.Transform;
                    var p3 = new Vector3(primitive.Scale.X, 0, -primitive.Scale.Z) * primitive.Transform;
                    var p4 = new Vector3(primitive.Scale.X, 0, primitive.Scale.Z) * primitive.Transform;
                    primitiveSegments.Add(new LineSegment3(p1, p2));
                    primitiveSegments.Add(new LineSegment3(p2, p3));
                    primitiveSegments.Add(new LineSegment3(p3, p4));
                    primitiveSegments.Add(new LineSegment3(p4, p1));
                }
                else
                {
                    var scale = primitive.Scale.X;
                    var currPoint = scale * Vector3.UnitX * primitive.Transform;
                    for (int j = 1; j < 33; j++)
                    {
                        var prevPoint = currPoint;
                        var angle = MathHelper.TwoPi * j / (33 - 1);
                        currPoint = (scale * new Vector3(MathHelper.Cos(angle), 0, MathHelper.Sin(angle))) * primitive.Transform;
                        primitiveSegments.Add(new LineSegment3(prevPoint, currPoint));
                    }
                }

                foreach (var segment in primitiveSegments)
                {
                    filteredSegments1.Clear();
                    filteredSegments2.Clear();
                    filteredSegments1.Add(segment);
                    for (int j = 0; j < primitives.Count; j++)
                    {
                        if (j == i)
                            continue;
                        var checkingPrimitive = primitives[j];
                        foreach (var subseg in filteredSegments1)
                            GetExternalPart(subseg, checkingPrimitive, filteredSegments2);
                        filteredSegments1.Clear();
                        CodingHelper.Swap(ref filteredSegments1, ref filteredSegments2);
                    }
                    wallSegments.AddRange(filteredSegments1.Select(x => new BuildingWallSegment{Basement = x, Height = BuildingConstants.CeilingHeight}));
                }
            }
            
            return wallSegments;
        }

        private static void GetExternalPart(LineSegment3 segment, BuildingPrimitive primitive, List<LineSegment3> results)
        {
            var invTransform = primitive.Transform.Invert();
            var transformedSeg = new LineSegment3(segment.Point1 * invTransform, segment.Point2 * invTransform);
            var seg = new LineSegment2(transformedSeg.Point1.Xz, transformedSeg.Point2.Xz);
            if (primitive.Type == BuildingPrimitiveType.Rectangle)
            {
                var rect = new AaRectangle2(Vector2.Zero, primitive.Scale.X, primitive.Scale.Z);
                var conains1 = rect.ContainsPoint(seg.Point1);
                var conains2 = rect.ContainsPoint(seg.Point2);
                if (conains1 && conains2)
                    return;
                if (!conains1 && !conains2)
                {
                    var p = Vector2.Zero;
                    int c = 0;
                    foreach (var rectSegment in rect.GetSegments())
                    {
                        var rsi = rectSegment.Intersect(seg);
                        if (rsi.HasValue)
                        {
                            p += rsi.Value;
                            c++;
                        }
                    }

                    if (c > 0)
                    {
                        var t = ((p / c) - seg.Point1).Length() / (seg.Point2 - seg.Point1).Length();
                        var m = segment.Point1 + (segment.Point2 - segment.Point1) * t;
                        GetExternalPart(new LineSegment3(segment.Point1, m), primitive, results);
                        GetExternalPart(new LineSegment3(m, segment.Point2), primitive, results);
                        return;
                    }
                    else
                    {
                        results.Add(segment);
                        return;
                    }
                    return;
                }

                var swap = conains1;
                if (swap)
                    CodingHelper.Swap(ref seg.Point1, ref seg.Point2);
                var inter = seg.Intersect(new LineSegment2(
                    new Vector2(-primitive.Scale.X, -primitive.Scale.Z), 
                    new Vector2(-primitive.Scale.X, primitive.Scale.Z)));
                if (inter.HasValue)
                {
                    var rs1 = To3(seg.Point1, inter.Value, primitive.Transform, swap);
                    results.Add(rs1);
                    return;
                }
                inter = seg.Intersect(new LineSegment2(
                    new Vector2(primitive.Scale.X, -primitive.Scale.Z),
                    new Vector2(primitive.Scale.X, primitive.Scale.Z)));
                if (inter.HasValue)
                {
                    var rs1 = To3(seg.Point1, inter.Value, primitive.Transform, swap);
                    results.Add(rs1);
                    return;
                }
                inter = seg.Intersect(new LineSegment2(
                    new Vector2(-primitive.Scale.X, primitive.Scale.Z),
                    new Vector2(primitive.Scale.X, primitive.Scale.Z)));
                if (inter.HasValue)
                {
                    var rs1 = To3(seg.Point1, inter.Value, primitive.Transform, swap);
                    results.Add(rs1);
                    return;
                }
                inter = seg.Intersect(new LineSegment2(
                    new Vector2(-primitive.Scale.X, -primitive.Scale.Z),
                    new Vector2(primitive.Scale.X, -primitive.Scale.Z)));
                if (inter.HasValue)
                {
                    var rs1 = To3(seg.Point1, inter.Value, primitive.Transform, swap);
                    results.Add(rs1);
                    return;
                }

                var rs2 = segment;
                results.Add(rs2);
                return;
            }
            else
            {
                var circle = new Circle2(Vector2.Zero, primitive.Scale.X);

                var conains1 = circle.Contains(seg.Point1);
                var conains2 = circle.Contains(seg.Point2);
                if (conains1 && conains2)
                    return;
                if (!conains1 && !conains2)
                {
                    var rs1 = segment;
                    results.Add(rs1);
                    return;
                }

                var swap = conains1;
                if (swap)
                    CodingHelper.Swap(ref seg.Point1, ref seg.Point2);

                var dpp = seg.Point2 - seg.Point1;
                var dpc = seg.Point1;
                var a = dpp.LengthSquared();
                var b = Vector2.Dot(dpp, dpc);
                var c = dpc.LengthSquared() - circle.Radius.Sq();
                var discr = b * b - a * c;
                if (discr < 0)
                {
                    results.Add(segment);
                    return;
                }
                if (discr < MathHelper.Eps5)
                {
                    results.Add(segment);
                    return;
                    //var l = -b / a;
                    //if (0 <= l && l <= 1)
                        
                }
                {
                    var sqrdscr = MathHelper.Sqrt(discr);
                    var l1 = (-b + sqrdscr) / a;
                    var l2 = (-b - sqrdscr) / a;
                    if (0 <= l1 && l1 <= 1)
                    {
                        var rs1 = To3(seg.Point1, Vector2.Lerp(seg.Point1, seg.Point2, l1), primitive.Transform, swap);
                        results.Add(rs1);
                    }

                    if (0 <= l2 && l2 <= 1)
                    {
                        var rs1 = To3(seg.Point1, Vector2.Lerp(seg.Point1, seg.Point2, l2), primitive.Transform, swap);
                        results.Add(rs1);
                    }
                }
            }
        }

        private static Vector3 To3(Vector2 v) => new Vector3(v.X, 0, v.Y);

        private static LineSegment3 To3(Vector2 p1, Vector2 p2, Transform transform, bool swap)
        {
            if (swap)
                CodingHelper.Swap(ref p1, ref p2);
            return new LineSegment3(To3(p1) * transform, To3(p2) * transform);
        }
    }
}