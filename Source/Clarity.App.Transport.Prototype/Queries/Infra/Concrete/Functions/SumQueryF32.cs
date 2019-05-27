using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Functions
{
    public class SumQueryF32 : InfraQueryBase<float>
    {
        public IInfraQuery<IEnumerable<float>> ValuesQuery { get; }

        public SumQueryF32(IInfraQuery<IEnumerable<float>> valuesQuery)
        {
            ValuesQuery = valuesQuery;
        }

        public override float Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            var values = ValuesQuery.Execute(context, timestamp);
            return values.Sum();
        }

        public override IEnumerable<(double timestamp, float val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            return ValuesQuery.ExecuteOverTime(context, startTimestamp, endTimestamp)
                .Select(x => (x.timestamp, val: x.val.Sum()))
                .WhereChanged((x, y) => x.val == y.val);
        }
    }
}