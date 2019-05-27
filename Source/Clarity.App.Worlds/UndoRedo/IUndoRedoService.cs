using Clarity.App.Worlds.Gui;

namespace Clarity.App.Worlds.UndoRedo
{
    public interface IUndoRedoService
    {
        //void Apply(IUndoable change);
        //ICommonUndoRedo Common { get; }
        void OnChange();

        void Undo();
        void Redo();
        bool CanUndo { get; }
        bool CanRedo { get; }
        
        // todo: remove
        void SetObserver(IGuiObserver<IUndoRedoService, object> observer);
    }
}