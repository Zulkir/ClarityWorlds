using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Queries.Infra.Concrete
{
    public class ConstantQuery<T> : InfraQueryBase<T>
    {
        public T Value { get; }

        public ConstantQuery(T value)
        {
            Value = value;
        }

        public override T Execute(IInfraQueryExecutionContext context, double timestamp) => Value;
        public override IEnumerable<(double timestamp, T val)> ExecuteOverTime(IInfraQueryExecutionContext context, double startTimestamp, double endTimestamp)
        {
            yield return (startTimestamp, Value);
        }
    }
}