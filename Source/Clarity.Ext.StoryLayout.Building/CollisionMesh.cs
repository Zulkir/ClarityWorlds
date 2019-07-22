using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.StoryLayout.Building
{
    // todo: make independent from the Building and move to the Clarity.App.Worlds
    public class CollisionMesh : ICollisionMesh
    {
        private readonly IReadOnlyList<BuildingWallSegment> wallSegments;
        private readonly IReadOnlyList<AaBox> verticallyBoundFloors;

        public float ZeroGravityTeleportHeight { get; }

        public IStoryLayoutZoning Zoning { get; }

        public CollisionMesh(IReadOnlyList<BuildingWallSegment> wallSegments, IReadOnlyList<AaBox> verticallyBoundFloors, IStoryLayoutZoning zoning)
        {
            this.wallSegments = wallSegments;
            this.verticallyBoundFloors = verticallyBoundFloors;
            ZeroGravityTeleportHeight = verticallyBoundFloors
                .SequentialPairs()
                .Select(x => x.Second.Center.Y - x.First.Center.Y)
                .FirstOrNull() ?? 0f;
            Zoning = zoning;
        }

        private IEnumerable<Pair<Vector3, BuildingWallSegment>> Intersect(Ray3 ray)
        {
            return wallSegments
                .Select(x => new Pair<Vector3?, BuildingWallSegment>(Intersects(ray, x),x))
                .Where(x => x.First.HasValue)
                .Select(x => new Pair<Vector3, BuildingWallSegment>(x.First.Value,x.Second));
        }

        public Vector3 RestrictMovement(float radius, Vector3 startingPoint, Vector3 offset)
        {
            var remainingT = offset.Length();
            var horrOffset = new Vector3(offset.X, 0, offset.Z);
            var vertOffset = offset.Y;
            var currentPoint = startingPoint;

            var floor = verticallyBoundFloors.Where(x => x.Contains(startingPoint)).FirstOrNull();

            if (floor.HasValue)
            {
                var floorHeight = floor.Value.Center.Y;

                currentPoint.Y += vertOffset;
                if (currentPoint.Y < floorHeight + BuildingConstants.EyeHeight)
                {
                    currentPoint.Y = floorHeight + BuildingConstants.EyeHeight;
                }
            }
            else
            {
                currentPoint.Y += vertOffset;
                if (currentPoint.Y < BuildingConstants.EyeHeight)
                {
                    currentPoint.Y = BuildingConstants.EyeHeight;
                }
            }

            while (remainingT > 0)
            {
                var ray = new Ray3(currentPoint, horrOffset.Normalize());
                var currentPointLoc = currentPoint;
                var intersection = Intersect(ray)
                    .MinimalOrNull(x => Vector3.Distance(currentPointLoc, x.First));
                if (intersection.HasValue)
                {
                    var tangent = intersection.Value.Second.Basement.Difference.Normalize();
                    var normal = Vector3.Cross(tangent, Vector3.UnitY);
                    if (Vector3.Dot(horrOffset, normal) > 0)
                        normal = -normal;
                    var nVelocity = Vector3.Dot(horrOffset, -normal);
                    var toEye = -ray.Direction * Vector3.Distance(ray.Point, intersection.Value.First);
                    var nDist = Vector3.Dot(toEye, normal);
                    var offsetNDist = Math.Max(nDist - radius, 0);

                    if (nVelocity * remainingT < offsetNDist)
                    {
                        currentPoint += horrOffset;
                        remainingT = 0;
                    }
                    else
                    {
                        var amount = MathHelper.Clamp(offsetNDist / nDist, 0, 1);
                        var move = -toEye * amount;
                        currentPoint += move;
                        remainingT -= move.Length() / horrOffset.Length();
                        horrOffset = Vector3.Dot(horrOffset, tangent) * tangent;
                    }
                }
                else
                {
                    currentPoint += horrOffset;
                    remainingT = 0;
                }
            }

            return currentPoint;
        }

        private static Vector3? Intersects(Ray3 ray, BuildingWallSegment wallSegment)
        {
            var height = ray.Point.Y;
            if (height < wallSegment.Basement.Point1.Y ||
                height > wallSegment.Basement.Point1.Y + wallSegment.Height)
            {
                return null;
            }
            var ray2D = new Ray2(ray.Point.Xz, ray.Direction.Xz.Normalize());
            var wallSeg2D = new LineSegment2(wallSegment.Basement.Point1.Xz, wallSegment.Basement.Point2.Xz);
            var inter2D = wallSeg2D.Intersect(ray2D);
            if (!inter2D.HasValue)
            {
                return null;
            }
            return new Vector3(inter2D.Value.X, ray.Point.Y, inter2D.Value.Y);
        }

        public IReadOnlyList<AaBox> GetWalkableAreas()
        {
            // todo: actually compute the walkable areas
            return verticallyBoundFloors;
        }
    }
}
