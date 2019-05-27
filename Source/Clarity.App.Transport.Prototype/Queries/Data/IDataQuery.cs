using System;
using Clarity.App.Transport.Prototype.Databases;
using Clarity.App.Transport.Prototype.Runtime;

namespace Clarity.App.Transport.Prototype.Queries.Data
{
    public interface IDataQuery : IQuery
    {
        string Text { get; }
        double Param0 { get; set; }

        IDataTable ResultTable { get; }
        IDataLog ResultDataLog { get; }
        event Action<IDataLogUpdatedEvent> DataLogUpdated;
    }
}