using System.Collections.Generic;
using System.Linq;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Runtime;

namespace Clarity.App.Transport.Prototype.Queries.Infra
{
    public class InfraQueryExecutionContext : IInfraQueryExecutionContext
    {
        private readonly IDataRetrievalService dataRetrievalService;

        public InfraQueryExecutionContext(IDataRetrievalService dataRetrievalService)
        {
            this.dataRetrievalService = dataRetrievalService;
        }

        public IDataTableState GetTableState(IDataTable table, double timestamp)
        {
            return dataRetrievalService.GetLogForTable(table).GetStateAt(timestamp).GetTableState(table);
        }

        public IEnumerable<DataLogReadingState> ReadTable(IDataTable table, double startTimestamp, double endTimestamp)
        {
            return dataRetrievalService.GetLogForTable(table).Read(startTimestamp, endTimestamp).Where(x => x.Entry.Table == table);
        }
    }
}