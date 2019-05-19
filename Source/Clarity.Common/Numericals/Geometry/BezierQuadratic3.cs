using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    public struct BezierQuadratic3
    {
        public Vector3 P0;
        public Vector3 P1;
        public Vector3 P2;

        public BezierQuadratic3(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
        }

        public Vector3 At(float t)
        {
            var it = 1f - t;
            return it * it * P0 + 2 * it * t * P1 + t * t * P2;
        }

        public Vector3 DerivFirstAt(float t)
        {
            var it = 1f - t;
            return 2 * it * (P1 - P0) + 2 * t * (P2 - P1);
        }

        public Vector3 DerivSecond()
        {
            return 2 * (P2 - 2 * P1 + P0);
        }

        public Vector3 TangentAt(float t) => DerivFirstAt(t).Normalize();

        public IEnumerable<Vector3> ToEnumPolyline(float tolerance)
        {
            yield return P0;
            foreach (var p in EnumIntermediatePoints(P0, 0, P2, 1, tolerance.Sq()))
                yield return p;
            yield return P2;
        }

        private IEnumerable<Vector3> EnumIntermediatePoints(Vector3 p1, float t1, Vector3 p2, float t2, float toleranceSq)
        {
            var middleT = (t1 + t2) / 2;
            var middleApprox = (p1 + p2) / 2;
            var moddleReal = At(middleT);
            if ((middleApprox - moddleReal).LengthSquared() < toleranceSq)
                return Enumerable.Empty<Vector3>();
            return 
                EnumIntermediatePoints(p1, t1, moddleReal, middleT, toleranceSq)
                .ConcatSingle(moddleReal)
                .Concat(EnumIntermediatePoints(moddleReal, middleT, p2, t2, toleranceSq));
        }

        public BezierQuadratic3 Reverse() => new BezierQuadratic3(P2, P1, P0);

        public static BezierQuadratic3 Straight(Vector3 p0, Vector3 p2) => new BezierQuadratic3(p0, (p0 + p2)/ 2, p2);
    }
}