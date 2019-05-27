using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Functions
{
    public class MultQuery : InfraQueryBase<DPolyCubic>
    {
        public IInfraQuery<DPolyCubic> LeftQuery { get; }
        public double Factor { get; }

        public MultQuery(IInfraQuery<DPolyCubic> leftQuery, double factor)
        {
            LeftQuery = leftQuery;
            Factor = factor;
        }

        public override DPolyCubic Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            return LeftQuery.Execute(context, timestamp) * Factor;
        }

        public override IEnumerable<(double timestamp, DPolyCubic val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            return LeftQuery.ExecuteOverTime(context, startTimestamp, endTimestamp).Select(x => (x.timestamp, x.val * Factor));
        }
    }
}