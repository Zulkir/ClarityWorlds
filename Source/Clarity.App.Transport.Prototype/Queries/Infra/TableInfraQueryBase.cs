using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Queries.Infra
{
    public abstract class TableInfraQueryBase<TResult> : InfraQueryBase<TResult>, ITableInfraQuery<TResult>
    {
        public IDataTable Table { get; }

        protected TableInfraQueryBase(IDataTable table)
        {
            Table = table;
        }
    }
}