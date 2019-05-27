using Clarity.Common.Numericals.Algebra;
using JetBrains.Annotations;

namespace Clarity.App.Transport.Prototype.Databases.LogEntries
{
    public class SetValueLogEntry<T> : IDataLogEntry
    {
        public double Timestamp { get; }
        public int OrdinalDisambiguator { get; set; }
        public IDataTable Table { get; }
        [NotNull] public IDataField Field { get; }
        public int RowKey { get; }
        public T NewValue { get; }
        public T OldValue { get; private set; }

        public SetValueLogEntry(double timestamp, IDataTable table, int rowKey, IDataField field, T newValue)
        {
            Timestamp = timestamp;
            Table = table;
            Field = field;
            RowKey = rowKey;
            NewValue = newValue;
        }

        public void MakeUndoable(IDataTableState tableStateRightBefore)
        {
            OldValue = tableStateRightBefore.GetValue<T>(RowKey, Field.Index);
        }

        public void Apply(IMutableDataTableState tableState)
        {
            tableState.SetValue(RowKey, Field.Index, NewValue);
        }

        public void Undo(IMutableDataTableState tableState)
        {
            tableState.SetValue(RowKey, Field.Index, OldValue);
        }

        public override string ToString()
        {
            return typeof(T) == typeof(DPolyCubic) && NewValue is DPolyCubic newValueP64
                ? $"{Timestamp}: {Table.Name}[{RowKey}].{Field.Name} = {newValueP64.ToStringAt(Timestamp)}" 
                : $"{Timestamp}: {Table.Name}[{RowKey}].{Field.Name} = {NewValue}";
        }
    }
}