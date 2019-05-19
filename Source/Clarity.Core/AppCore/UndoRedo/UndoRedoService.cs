using System.Collections.Generic;
using Clarity.Core.AppCore.Gui;

namespace Clarity.Core.AppCore.UndoRedo
{
    public class UndoRedoService : IUndoRedoService, IGuiObservable<IUndoRedoService, object>
    {
        private readonly Stack<IUndoable> undoStack;
        private readonly Stack<IUndoable> redoStack;
        private IGuiObserver<IUndoRedoService, object> guiObserver;
        public ICommonUndoRedo Common { get; private set; }

        public UndoRedoService()
        {
            undoStack = new Stack<IUndoable>();
            redoStack = new Stack<IUndoable>();
            guiObserver = null;
            Common = new CommonUndoRedo(this);
        }

        public void Apply(IUndoable change)
        {
            change.Apply();
            undoStack.Push(change);
            redoStack.Clear();
            NotifyObserver();
        }

        public void OnChange()
        {
            // todo: implement
        }

        public void Undo()
        {
            var lastChange = undoStack.Pop();
            lastChange.Undo();
            redoStack.Push(lastChange);
            NotifyObserver();
        }

        public void Redo()
        {
            var lastUndoneChange = redoStack.Pop();
            lastUndoneChange.Apply();
            undoStack.Push(lastUndoneChange);
            NotifyObserver();
        }

        public bool CanUndo { get { return undoStack.Count > 0; } }

        public bool CanRedo { get { return redoStack.Count > 0; } }

        public void SetObserver(IGuiObserver<IUndoRedoService, object> observer)
        {
            guiObserver = observer;
        }

        private void NotifyObserver()
        {
            if (guiObserver != null)
                guiObserver.OnEvent(this, null);
        }
    }
}