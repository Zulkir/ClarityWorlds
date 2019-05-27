using Clarity.App.Transport.Prototype.DataSources;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IDataSourceRuntime
    {
        IDataSource Source { get; }

        void ChangeDataSource(IDataSource newDataSource);
    }
}