using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Functions
{
    public class AvgOverQueryP64 : InfraQueryBase<DPolyCubic>
    {
        private struct PolySegment
        {
            public DPolyCubic Poly;
            public double T1;
            public double T2;

            public double DeltaT => T2 - T1;

            public PolySegment(in DPolyCubic poly, double t1, double t2)
            {
                Poly = poly;
                T1 = t1;
                T2 = t2;
            }

            public override string ToString() => $"({T1:N}-{T2:N}: {Poly.ToStringAt(T1)})";
        }

        public IInfraQuery<DPolyCubic> ValuesQuery { get; }
        public double TimeSpan { get; }

        public AvgOverQueryP64(IInfraQuery<DPolyCubic> valuesQuery, double timeSpan)
        {
            ValuesQuery = valuesQuery;
            TimeSpan = timeSpan;
        }

        public override DPolyCubic Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            var values = ValuesQuery.ExecuteOverTime(context, timestamp - TimeSpan, timestamp);
            var segments = ValuesToSegments(values, timestamp);
            return ForSegments(segments);
        }

        public override IEnumerable<(double timestamp, DPolyCubic val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            var values = ValuesQuery.ExecuteOverTime(context, startTimestamp - TimeSpan, endTimestamp);
            var segments = ValuesToSegments(values, endTimestamp).ToArray();
            return ToRangedSegmentSequences(segments)
                  .Select(x => (x.timestamp, ForSegments(x.segments)));
        }

        private static IEnumerable<PolySegment> ValuesToSegments(IEnumerable<(double timestamp, DPolyCubic val)> values, double endTimestamp)
        {
            return values.ConcatSingle((timestamp: endTimestamp, val: new DPolyCubic(0, 0, 0, 0)))
                         .SequentialPairs()
                         .Select(x => new PolySegment(x.First.val, x.First.timestamp, x.Second.timestamp));
        }

        private IEnumerable<double> CalcDiscontinuityPoints(PolySegment[] allSegments)
        {
            var t1 = allSegments[0].T1;
            var i1 = 0;
            var t2 = t1 + TimeSpan;
            var i2 = allSegments.Select((s, i) => (s, i)).First(x => x.s.T2 >= t2).i;

            yield return t2;

            while (i2 < allSegments.Length)
            {
                var toLowerBorder = allSegments[i1].T2 - t1;
                var toHigherBorder = allSegments[i2].T2 - t2;
                if (toLowerBorder < toHigherBorder)
                {
                    i1++;
                    t1 = allSegments[i1].T1;
                    t2 = t1 + TimeSpan;
                    yield return t2; // (t2 - toLowerBorder, EnumerateSubsegments(allSegments, i1, t1, i2, t2));
                }
                else
                {
                    t2 = allSegments[i2].T2;
                    t1 = t2 - TimeSpan;
                    yield return t2; // (t2 - toHigherBorder, EnumerateSubsegments(allSegments, i1, t1, i2, t2));
                    i2++;
                }
            }
        }

        private IEnumerable<(double timestamp, IEnumerable<PolySegment> segments)> ToRangedSegmentSequences(PolySegment[] allSegments)
        {
            var t1 = allSegments[0].T1;
            var i1 = 0;
            var t2 = t1 + TimeSpan;
            var i2 = allSegments.Select((s, i) => (s, i)).First(x => x.s.T2 >= t2).i;

            //yield return (t2, EnumerateSubsegments(allSegments, i1, t1, i2, t2));

            while (i2 < allSegments.Length)
            {
                var toLowerBorder = allSegments[i1].T2 - t1;
                var toHigherBorder = allSegments[i2].T2 - t2;
                var offset = Math.Min(toLowerBorder, toHigherBorder);
                if (offset > MathHelper.DEps8)
                {
                    var halfOffset = offset / 2;
                    yield return (t2, EnumerateSubsegments(allSegments, i1, t1 + halfOffset, i2, t2 + halfOffset));
                }

                if (toLowerBorder < toHigherBorder)
                {
                    i1++;
                    t1 = allSegments[i1].T1;
                    t2 = t1 + TimeSpan;
                }
                else
                {
                    t2 = allSegments[i2].T2;
                    t1 = t2 - TimeSpan;
                    i2++;
                }
            }
        }

        private IEnumerable<PolySegment> EnumerateSubsegments(PolySegment[] allSegments, int i1, double t1, int i2, double t2)
        {
            for (var i = i1; i <= i2; i++)
            {
                var segment = allSegments[i];
                var localT1 = i == i1 ? t1 : segment.T1;
                var localT2 = i == i2 ? t2 : segment.T2;
                yield return new PolySegment(segment.Poly, localT1, localT2);
            }
        }

        private static DPolyCubic ForSegments(IEnumerable<PolySegment> segments)
        {
            var firstSegment = default(PolySegment);
            var lastSegment = default(PolySegment);
            var isFirst = true;
            var d0sum = 0.0;
            foreach (var segment in segments)
            {
                if (isFirst)
                {
                    firstSegment = segment;
                    isFirst = false;
                }
                d0sum += segment.Poly.Integrate(segment.T1, segment.T2);
                lastSegment = segment;
            }
            var f1 = firstSegment.Poly;
            var f2 = lastSegment.Poly;
            var t1 = firstSegment.T1;
            var t2 = lastSegment.T2;
            var deltaT = t2 - t1;
            if (deltaT < MathHelper.DEps8)
            {
                var midT = (t1 + t2) / 2;
                var avgVal = (f1.ValueAt(t1) + f2.ValueAt(t2)) / 2;
                return new DPolyCubic(midT, avgVal, 0, 0, 0);
            }
            var invDeltaT = 1.0 / deltaT;
            var d0 = d0sum * invDeltaT;
            var d1 = (f2.ValueAt(t2) - f1.ValueAt(t1)) * invDeltaT;
            var d1f1 = f1.Derive();
            var d1f2 = f2.Derive();
            var d2 = (d1f2.ValueAt(t2) - d1f1.ValueAt(t1)) * invDeltaT;
            var d2f1 = d1f1.Derive();
            var d2f2 = d1f2.Derive();
            var d3 = (d2f2.ValueAt(t2) - d2f1.ValueAt(t1)) * invDeltaT;
            return new DPolyCubic(t2, d0, d1, d2, d3);
        }
    }
}