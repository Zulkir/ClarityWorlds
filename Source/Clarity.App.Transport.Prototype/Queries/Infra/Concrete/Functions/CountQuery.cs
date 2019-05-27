using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Functions
{
    // todo: to <int>
    public class CountQuery : InfraQueryBase<DPolyCubic>
    {
        public IInfraQuery<TableRowEnumeration> RowEnumerationQuery { get; }

        public CountQuery(IInfraQuery<TableRowEnumeration> rowEnumerationQuery)
        {
            RowEnumerationQuery = rowEnumerationQuery;
        }

        public override DPolyCubic Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            var count = RowEnumerationQuery.Execute(context, timestamp).Keys.Count();
            return new DPolyCubic(0, 0, 0, count);
        }

        public override IEnumerable<(double timestamp, DPolyCubic val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            return RowEnumerationQuery.ExecuteOverTime(context, startTimestamp, endTimestamp)
                .Select(x => (x.timestamp, val: x.val.Keys.Count()))
                .Select(x => (x.timestamp, val: new DPolyCubic(0, 0, 0, x.val)))
                .WhereChanged((x, y) => x.val == y.val);
        }
    }
}