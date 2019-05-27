using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Queries.Infra.Concrete.Types;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete
{
    public class ForFromLogQuery : InfraQueryBase<TableReadingState>
    {
        public IDataTable Table { get; }
        public IDataLog Log { get; }

        public ForFromLogQuery(IDataTable table, IDataLog log)
        {
            Table = table;
            Log = log;
        }

        public override TableReadingState Execute(IInfraQueryExecutionContext context, double timestamp)
        {
            return new TableReadingState(Log.GetStateAt(timestamp).GetTableState(Table), null);
        }

        public override IEnumerable<(double timestamp, TableReadingState val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            return Log.Read(startTimestamp, endTimestamp)
                .Select(readingState => 
                    (readingState.Entry.Timestamp, 
                     new TableReadingState(readingState.State.GetTableState(Table), readingState.Entry)));
        }
    }
}