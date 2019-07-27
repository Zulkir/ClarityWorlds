using System;
using Clarity.Common.Numericals.Algebra;
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
        private readonly int intTag;
        private readonly string strTag;

        private ISceneNode Node => master.Node;

        public SphereHittable(TMaster master, Func<TMaster, Sphere> getSphere, bool inverted = false,
            int intTag = 0, string strTag = null)
        {
            this.master = master;
            this.getSphere = getSphere;
            this.inverted = inverted;
            this.intTag = intTag;
            this.strTag = strTag;
        }

        public RayHitResult HitWithClick(RayCastInfo clickInfo)
        {
            var globalRay = clickInfo.GlobalRay;
            var sphere = getSphere(master);
            var segmentLength = (sphere.Center - globalRay.Point).Length() * 3;
            var segment = new LineSegment3(globalRay.Point, globalRay.Point + globalRay.Direction * segmentLength);
            sphere.Intersect(segment, out var p1, out var p2);

            if (!p1.HasValue)
                return RayHitResult.Failure();
            var d1 = (p1.Value - globalRay.Point).Length();

            Vector3 globalPoint;
            float distance;
            if (p2.HasValue)
            {
                var d2 = (p2.Value - globalRay.Point).Length();
                var use1 = (d1 <= d2) ^ inverted;
                if (use1)
                {
                    globalPoint = p1.Value;
                    distance = d1;
                }
                else
                {
                    globalPoint = p2.Value;
                    distance = d2;
                }
            }
            else
            {
                globalPoint = p1.Value;
                distance = d1;
            }

            // todo: consider rotation
            var localPoint = (globalPoint - sphere.Center) / sphere.Radius;
            return new RayHitResult
            {
                Successful = true,
                Node = Node,
                Distance = distance,
                GlobalHitPoint = globalPoint,
                LocalHitPoint = localPoint,
                IntTag = intTag,
                StrTag = strTag
            };
        }
    }
}