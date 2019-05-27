using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Queries.Infra
{
    public interface IInfraQueryExecutionContext
    {
        IDataTableState GetTableState(IDataTable table, double timestamp);
        IEnumerable<DataLogReadingState> ReadTable(IDataTable table, double startTimestamp, double endTimestamp);
    }
}