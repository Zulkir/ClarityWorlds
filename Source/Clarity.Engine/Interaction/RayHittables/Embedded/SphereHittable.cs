using System;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables.Embedded
{
    public class SphereHittable<TMaster> : IRayHittable
        where TMaster : ISceneNodeBound
    {
        private readonly TMaster master;
        private readonly Func<TMaster, Sphere> getSphere;
        private readonly bool inverted;

        private ISceneNode Node => master.Node;

        public SphereHittable(TMaster master, Func<TMaster, Sphere> getSphere, bool inverted = false)
        {
            this.master = master;
            this.getSphere = getSphere;
            this.inverted = inverted;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo)
        {
            var globalRay = clickInfo.GlobalRay;
            var sphere = getSphere(master);
            var segmentLength = (sphere.Center - globalRay.Point).Length() * 3;
            var segment = new LineSegment3(globalRay.Point, globalRay.Point + globalRay.Direction * segmentLength);
            sphere.Intersect(segment, out var p1, out var p2);

            if (p1.HasValue && p2.HasValue)
            {
                var d1 = (p1.Value - globalRay.Point).Length();
                var d2 = (p2.Value - globalRay.Point).Length();
                var use1 = (d1 <= d2) ^ inverted;
                return use1 
                    ? RayHitResult.Success(Node, p1.Value, d1)
                    : RayHitResult.Success(Node, p2.Value, d2);
            }

            if (p1.HasValue)
            {
                var d1 = (p1.Value - globalRay.Point).Length();
                return RayHitResult.Success(Node, p1.Value, d1);
            }

                return RayHitResult.Failure();
        }
    }
}