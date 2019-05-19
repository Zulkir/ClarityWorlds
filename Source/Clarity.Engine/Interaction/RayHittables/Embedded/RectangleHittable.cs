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

        private ISceneNode Node => master.Node;

        public RectangleHittable(TComponent master, Transform planeTransform, 
            Func<TComponent, AaRectangle2> getLocalRectangle, Func<TComponent, float> getHitDistanceOffset) 
        {
            this.master = master;
            this.getLocalRectangle = getLocalRectangle;
            this.getHitDistanceOffset = getHitDistanceOffset;
            this.planeTransform = planeTransform;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo)
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
            var globalPoint = globalRay.Point + globalRay.Direction * globalT;
            var offset = getHitDistanceOffset(master);
            return RayHitResult.Success(Node, globalPoint, globalT + offset);
        }
    }
}