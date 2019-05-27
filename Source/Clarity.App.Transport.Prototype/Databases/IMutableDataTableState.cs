namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IMutableDataTableState : IDataTableState
    {
        void AddNewRow(out int rowKey);
        void InsertNewRow(int rowKey);
        void DeleteRow(int rowKey);
        void SetValue<T>(int rowKey, int fieldIndex, T value);
        void SetValueAbstract(int rowKey, int fieldIndex, object value);
        void AppendExisting(IDataTableState data);
    }
}