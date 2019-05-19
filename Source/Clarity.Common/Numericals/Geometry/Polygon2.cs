using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Common.Numericals.Geometry
{
    public struct Polygon2
    {
        public ImmutableArray<Vector2> Points { get; }
        private bool? isConvex;

        public bool IsConvex => isConvex ?? (isConvex = CalcIsConvex()).Value;

        private bool CalcIsConvex()
        {
            var movedRight = false;
            var movedLeft = false;

            foreach (var offsetPair in Points.ConcatSingle(Points[0]).SequentialPairs().Select(x => x.Second - x.First).SequentialPairs())
            {
                var cross = Vector2.Cross(offsetPair.First, offsetPair.Second);
                if (cross > 0)
                    movedRight = true;
                else if (cross < 0)
                    movedLeft = true;
            }

            return !movedLeft || !movedRight;
        }
    }
}