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

        private ISceneNode Node => master.Node;

        public LineHittable(TMaster master, Func<TMaster, Line3> getLine, float lineWidth)
        {
            this.master = master;
            this.getLine = getLine;
            this.lineWidth = lineWidth;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo)
        {
            var line = getLine(master);
            var mouseLine = clickInfo.GlobalRay.ToLine();
            Line3.GetClosestPoints(line, mouseLine, out var p1, out var p2);
            var grad = clickInfo.GetAbsPixelGradientAt((p1 + p2) / 2);
            var diff = (p2 - p1).Abs();
            var pixelDiff = Vector3.Dot(diff, grad);
            return pixelDiff <= lineWidth 
                ? RayHitResult.Success(Node, p1, (clickInfo.GlobalRay.Point - p1).Length()) 
                : RayHitResult.Failure();
        }
    }
}