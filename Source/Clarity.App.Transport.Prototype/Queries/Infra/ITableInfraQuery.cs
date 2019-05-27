using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Queries.Infra
{
    public interface ITableInfraQuery<TResult> : IInfraQuery<TResult>
    {
        IDataTable Table { get; }
    }
}