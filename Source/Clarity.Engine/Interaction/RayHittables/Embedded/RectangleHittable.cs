using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables.Embedded
{
    public class RectangleHittable<TComponent> : IRayHittable
        where TComponent : ISceneNodeBound
    {
        private readonly TComponent master;
        private readonly Transform planeTransform;
        private readonly Func<TComponent, AaRectangle2> getLocalRectangle;
        private readonly Func<TComponent, float> getHitDistanceOffset;
        private readonly int intTag;
        private readonly string strTag;

        private ISceneNode Node => master.Node;

        public RectangleHittable(TComponent master, Transform planeTransform, 
            Func<TComponent, AaRectangle2> getLocalRectangle, Func<TComponent, float> getHitDistanceOffset, 
            int intTag = 0, string strTag = null) 
        {
            this.master = master;
            this.getLocalRectangle = getLocalRectangle;
            this.getHitDistanceOffset = getHitDistanceOffset;
            this.intTag = intTag;
            this.strTag = strTag;
            this.planeTransform = planeTransform;
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
            var rectangle = getLocalRectangle(master);
            if (!rectangle.ContainsPoint(point.Xy))
                return RayHitResult.Failure();

            var globalT = t / globalTransformInverse.Scale;
            var offset = getHitDistanceOffset(master);
            var globalPoint = globalRay.Point + globalRay.Direction * globalT;
            var localPoint = new Vector3(
                (point.X - rectangle.Center.X) / (rectangle.HalfWidth * 2),
                (point.Y - rectangle.Center.Y) / (rectangle.HalfHeight * 2),
                0);
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