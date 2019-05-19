namespace Clarity.Core.AppCore.UndoRedo
{
    public static class UndoableExtensions
    {
        public static IUndoable Reverse(this IUndoable undoable)
        {
            return new ReversedUndoable(undoable);
        }
    }
}