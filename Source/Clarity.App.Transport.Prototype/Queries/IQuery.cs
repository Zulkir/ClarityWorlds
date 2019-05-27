using Clarity.App.Transport.Prototype.Runtime;

namespace Clarity.App.Transport.Prototype.Queries
{
    public interface IQuery
    {
        bool CheckIsValid(out string errorMessage);
        void OnTimestampChanged(double timestamp);

        void OnAttached();
        void OnTableLayoutChanged();
        void OnDataLogUpdated(IDataLogUpdatedEvent evnt);
    }
}