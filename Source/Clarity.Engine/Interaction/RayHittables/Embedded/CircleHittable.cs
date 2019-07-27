using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables.Embedded
{
    public class CircleHittable<TComponent> : IRayHittable
        where TComponent : ISceneNodeBound
    {
        private readonly TComponent master;
        private readonly Transform planeTransform;
        private readonly Func<TComponent, Circle2> getLocalCirlce;
        private readonly Func<TComponent, float> getHitDistanceOffset;
        private readonly int intTag;
        private readonly string strTag;

        private ISceneNode Node => master.Node;

        public CircleHittable(TComponent master, Transform planeTransform,
            Func<TComponent, Circle2> getLocalCirlce, Func<TComponent, float> getHitDistanceOffset, int intTag = 0, string strTag = null)
        {
            this.master = master;
            this.getLocalCirlce = getLocalCirlce;
            this.getHitDistanceOffset = getHitDistanceOffset;
            this.planeTransform = planeTransform;
            this.intTag = intTag;
            this.strTag = strTag;
        }

        public RayHitResult HitWithClick(RayCastInfo clickInfo)
        {
            var globalRay = clickInfo.GlobalRay;
            var globalTransformInverse = (planeTransform * Node.GlobalTransform).Invert();
            var localRay = globalRay * globalTransformInverse;

            var t = -localRay.Point.Z / localRay.Direction.Z;
            if (t <= 0f)
                return RayHitResult.Failure();

            var point = localRay.Point + localRay.Direction * t;
            var cirlce = getLocalCirlce(master);
            if (!cirlce.Contains(point.Xy))
                return RayHitResult.Failure();

            var globalT = t / globalTransformInverse.Scale;
            var offset = getHitDistanceOffset(master);
            var globalPoint = globalRay.Point + globalRay.Direction * globalT;
            // todo: consider rotation
            var localPoint = new Vector3(point.Xy - cirlce.Center, 0) / cirlce.Radius;
            return new RayHitResult
            {
                Successful = true,
                Node = Node,
                Distance = globalT + offset,
                GlobalHitPoint = globalPoint,
                LocalHitPoint = localPoint,
                IntTag = intTag,
                StrTag = strTag
            };
        }
    }
}