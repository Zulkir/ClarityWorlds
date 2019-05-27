using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete
{
    public class FloatToP64Query : InfraQueryBase<IEnumerable<DPolyCubic>>
    {
        public IInfraQuery<IEnumerable<float>> InputQuery { get; }

        public FloatToP64Query(IInfraQuery<IEnumerable<float>> inputQuery)
        {
            InputQuery = inputQuery;
        }

        public override IEnumerable<DPolyCubic> Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            return InputQuery.Execute(context, timestamp).Select(x => new DPolyCubic(0, 0, 0, x));
        }

        public override IEnumerable<(double timestamp, IEnumerable<DPolyCubic> val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            return InputQuery.ExecuteOverTime(context, startTimestamp, endTimestamp)
                .Select(x => (x.timestamp, val: x.val.Select(y => new DPolyCubic(0, 0, 0, y))));
        }
    }
}