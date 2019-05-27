using System;
using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Queries.Infra
{
    public interface IInfraQuery
    {
        Type ReturnType { get; }
        object ExecuteAbstract(IInfraQueryExecutionContext context, double timestamp);
        IEnumerable<(double timestamp, object val)> ExecuteAbstractOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp);
    }

    public interface IInfraQuery<TResult> : IInfraQuery
    {
        TResult Execute(IInfraQueryExecutionContext context, double timestamp);
        IEnumerable<(double timestamp, TResult val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp);
    }
}