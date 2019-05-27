using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete
{
    public class EnumerateRowsQuery : TableInfraQueryBase<TableRowEnumeration>
    {
        public EnumerateRowsQuery(IDataTable table)
            : base(table)
        {
        }

        public override TableRowEnumeration Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            var state = context.GetTableState(Table, timestamp);
            return new TableRowEnumeration(state, state.Keys);
        }

        public override IEnumerable<(double timestamp, TableRowEnumeration val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            var initialState = context.GetTableState(Table, startTimestamp);
            yield return (startTimestamp, new TableRowEnumeration(initialState, initialState.Keys));
            foreach (var readingState in context.ReadTable(Table, startTimestamp, endTimestamp))
            {
                var state = readingState.State.GetTableState(Table);
                yield return (readingState.Entry.Timestamp, new TableRowEnumeration(state, state.Keys));
            }
        }
    }
}