namespace Clarity.Core.AppCore.UndoRedo
{
    public interface IUndoable
    {
        void Apply();
        void Undo();
    }
}