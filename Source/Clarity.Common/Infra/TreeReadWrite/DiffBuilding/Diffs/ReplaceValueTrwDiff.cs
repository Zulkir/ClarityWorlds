namespace Clarity.Common.Infra.TreeReadWrite.DiffBuilding.Diffs
{
    public class ReplaceValueTrwDiff : ITrwDiff
    {
        public object OldValue { get; }
        public object NewValue { get; }

        public bool IsEmpty => false;

        public ReplaceValueTrwDiff(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}