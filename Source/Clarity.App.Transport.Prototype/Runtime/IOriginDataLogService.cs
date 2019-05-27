using Clarity.App.Transport.Prototype.Databases;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IOriginDataLogService
    {
        IDataLog DataLog { get; }
    }
}