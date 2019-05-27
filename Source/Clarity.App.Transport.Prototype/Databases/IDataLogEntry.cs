using JetBrains.Annotations;

namespace Clarity.App.Transport.Prototype.Databases
{
    public interface IDataLogEntry
    {
        double Timestamp { get; }
        int OrdinalDisambiguator { get; set; }

        IDataTable Table { get; }
        int RowKey { get; }
        [CanBeNull] IDataField Field { get; }

        void MakeUndoable(IDataTableState tableStateRightBefore);
        void Apply(IMutableDataTableState tableState);
        void Undo(IMutableDataTableState tableState);
    }
}