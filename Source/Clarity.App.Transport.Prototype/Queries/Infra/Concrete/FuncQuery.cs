using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete
{
    public class FuncQuery<T> : InfraQueryBase<IEnumerable<T>>
    {
        public IInfraQuery<TableRowEnumeration> RowEnumerationQuery { get; }
        public Func<IDataTableState, int, T> Function { get; }

        public FuncQuery(IInfraQuery<TableRowEnumeration> rowEnumerationQuery, Func<IDataTableState, int, T> function)
        {
            RowEnumerationQuery = rowEnumerationQuery;
            Function = function;
        }

        public override IEnumerable<T> Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            var enumeration = RowEnumerationQuery.Execute(context, timestamp);
            return enumeration.Keys.Select(x => Function(enumeration.State, x));
        }

        public override IEnumerable<(double timestamp, IEnumerable<T> val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            return RowEnumerationQuery.ExecuteOverTime(context, startTimestamp, endTimestamp)
                .Select(x => (x.timestamp, val: x.val.Keys.Select(y => Function(x.val.State, y))));
        }
    }
}