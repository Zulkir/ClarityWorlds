using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables.Embedded
{
    public class LineHittable<TMaster> : IRayHittable
        where TMaster : ISceneNodeBound
    {
        private readonly TMaster master;
        private readonly Func<TMaster, Line3> getLine;
        private readonly float lineWidth;
        private readonly int intTag;
        private readonly string strTag;

        private ISceneNode Node => master.Node;

        public LineHittable(TMaster master, Func<TMaster, Line3> getLine, float lineWidth,
            int intTag = 0, string strTag = null)
        {
            this.master = master;
            this.getLine = getLine;
            this.lineWidth = lineWidth;
            this.intTag = intTag;
            this.strTag = strTag;
        }

        public RayHitResult HitWithClick(RayCastInfo clickInfo)
        {
            // todo: calculate in screen space?
            var line = getLine(master);
            var mouseLine = clickInfo.GlobalRay.ToLine();
            Line3.GetClosestPoints(line, mouseLine, out var p1, out var p2);
            var grad = clickInfo.GetAbsPixelGradientAt((p1 + p2) / 2);
            var diff = (p2 - p1).Abs();
            var pixelDiff = Vector3.Dot(diff, grad);
            return pixelDiff <= lineWidth 
                ? new RayHitResult
                    {
                        Successful = true,
                        Node = Node,
                        Distance = (clickInfo.GlobalRay.Point - p1).Length(),
                        GlobalHitPoint = p1,
                        LocalHitPoint = p1,
                        IntTag = intTag,
                        StrTag = strTag
                    }
                : RayHitResult.Failure();
        }
    }
}