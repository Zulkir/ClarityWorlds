using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataTableState
    {
        IDataTable Table { get; }
        int RowCount { get; }

        ICollection<int> Keys { get; }

        // todo: EntriesByFilter

        bool HasRow(int key);

        T GetValue<T>(int rowKey, int fieldIndex);
        T GetValueByIndex<T>(int rowIndex, int fieldIndex);

        object GetValueAbstract(int rowKey, int fieldIndex);
        object GetValueAbstractByIndex(int rowIndex, int fieldIndex);

        IMutableDataTableState CloneAsMutable();
    }
}