using Clarity.Core.AppCore.Gui;

namespace Clarity.Core.AppCore.UndoRedo
{
    public interface IUndoRedoService
    {
        void Apply(IUndoable change);
        ICommonUndoRedo Common { get; }
        void OnChange();

        void Undo();
        void Redo();
        bool CanUndo { get; }
        bool CanRedo { get; }
        void SetObserver(IGuiObserver<IUndoRedoService, object> observer);
    }
}