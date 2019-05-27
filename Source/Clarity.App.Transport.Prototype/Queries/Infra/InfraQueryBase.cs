using System;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.App.Transport.Prototype.Queries.Infra
{
    public abstract class InfraQueryBase<TResult> : IInfraQuery<TResult>
    {
        public Type ReturnType => typeof(TResult);

        public object ExecuteAbstract(IInfraQueryExecutionContext context, double timestamp) => 
            Execute(context, timestamp);
        public IEnumerable<(double timestamp, object val)> ExecuteAbstractOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp) =>
            ExecuteOverTime(context, startTimestamp, endTimestamp).Select(x => (x.timestamp, (object)x.val));

        public abstract TResult Execute(IInfraQueryExecutionContext context, double timestamp);
        public abstract IEnumerable<(double timestamp, TResult val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp);

        protected static IEnumerable<(double, TVal)> RemoveUnchanged<TVal>(IEnumerable<(double, TVal)> resultsOverTime)
        {
            var last = default(TVal);
            var first = true;
            foreach (var (t, v) in resultsOverTime)
            {
                if (first)
                {
                    first = false;
                    yield return (t, v);
                    last = v;
                }
                else if (!EqualityComparer<TVal>.Default.Equals(last, v))
                {
                    yield return (t, v);
                    last = v;
                }
            }
        }
    }
}