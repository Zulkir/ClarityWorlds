namespace Clarity.App.Worlds.UndoRedo
{
    public interface IUndoable
    {
        void Apply();
        void Undo();
    }
}