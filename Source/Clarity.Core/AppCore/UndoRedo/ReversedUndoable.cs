namespace Clarity.Core.AppCore.UndoRedo
{
    public class ReversedUndoable : IUndoable
    {
        private readonly IUndoable internalUndoable;

        public ReversedUndoable(IUndoable internalUndoable) { this.internalUndoable = internalUndoable; }

        public void Apply() { internalUndoable.Undo(); }
        public void Undo() { internalUndoable.Apply(); }
    }
}