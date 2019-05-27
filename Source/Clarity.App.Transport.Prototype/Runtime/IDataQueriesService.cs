using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Queries.Data;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IDataQueriesService
    {
        IReadOnlyList<IDataQuery> Queries { get; }
        void AddQuery(IDataQuery query);
        void RemoveQuery(int index);
        void OnTimestampChanged(double timestamp);
    }
}