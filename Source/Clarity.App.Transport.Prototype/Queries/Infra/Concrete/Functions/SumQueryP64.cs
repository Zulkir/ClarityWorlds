using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Functions 
{
    public class SumQueryP64 : InfraQueryBase<DPolyCubic>
    {
        public IInfraQuery<IEnumerable<DPolyCubic>> ValuesQuery { get; }

        public SumQueryP64(IInfraQuery<IEnumerable<DPolyCubic>> valuesQuery)
        {
            ValuesQuery = valuesQuery;
        }

        public override DPolyCubic Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            var values = ValuesQuery.Execute(context, timestamp);
            return values.Aggregate((x, y) => x + y);
        }

        public override IEnumerable<(double timestamp, DPolyCubic val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            return ValuesQuery.ExecuteOverTime(context, startTimestamp, endTimestamp)
                .Select(v => (v.timestamp, val: v.val.Aggregate(DPolyCubic.Zero, (x, y) => x + y)))
                .WhereChanged((x, y) => x.val == y.val);
        }
    }
}