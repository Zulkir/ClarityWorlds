using System;
using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete
{
    public class WhereQuery : InfraQueryBase<TableRowEnumeration>
    {
        public IInfraQuery<TableRowEnumeration> InputQuery { get; }
        public Func<TableRowEnumeration, IEnumerable<int>> Filter { get; }

        public WhereQuery(IInfraQuery<TableRowEnumeration> inputQuery, Func<TableRowEnumeration, IEnumerable<int>> filter)
        {
            InputQuery = inputQuery;
            Filter = filter;
        }

        public override TableRowEnumeration Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            var enumeration = InputQuery.Execute(context, timestamp);
            var filteredKeys = Filter(enumeration);
            return new TableRowEnumeration(enumeration.State, filteredKeys);
        }

        public override IEnumerable<(double timestamp, TableRowEnumeration val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            foreach (var (timestamp, enumeration) in InputQuery.ExecuteOverTime(context, startTimestamp, endTimestamp))
            {
                var filteredKeys = Filter(enumeration);
                yield return (timestamp, val: new TableRowEnumeration(enumeration.State, filteredKeys));
            }
        }
    }
}