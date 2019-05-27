using System.Collections.Generic;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.SaveLoad;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Serialization;

namespace Clarity.App.Worlds.UndoRedo
{
    public class UndoRedoService : IUndoRedoService, IGuiObservable<IUndoRedoService, object>
    {
        private readonly IWorldTreeService worldTreeService;
        private readonly ITrwDiffBuilder diffBuilder;
        private readonly ITrwSerializationDiffApplier diffApplier;
        private readonly UndoRedoDiffIdentityComparer diffIdentityComparer;
        private readonly Stack<IUndoable> undoStack;
        private readonly Stack<IUndoable> redoStack;
        private IGuiObserver<IUndoRedoService, object> guiObserver;
        private object previousDynamicTree;

        public UndoRedoService(IWorldTreeService worldTreeService, IAssetService assetService, ISerializationNecessities serializationNecessities, ITrwDiffBuilder diffBuilder)
        {
            this.worldTreeService = worldTreeService;
            this.diffBuilder = diffBuilder;
            var handlers = serializationNecessities.GetTrwHandlerContainer(SaveLoadConstants.WorldSerializationType);
            var typeRedirects = serializationNecessities.GetTrwHandlerTypeRedirects(SaveLoadConstants.WorldSerializationType);
            diffApplier = new TrwSerializationDiffApplier(handlers, typeRedirects, 
                x => x.Add(SaveLoadConstants.AssetDictBagKey, new Dictionary<string, IAsset>()),
                x => x.Add(SaveLoadConstants.AssetDictBagKey, assetService.Assets));
            diffIdentityComparer = new UndoRedoDiffIdentityComparer();
            undoStack = new Stack<IUndoable>();
            redoStack = new Stack<IUndoable>();
            guiObserver = null;
        }

        public void OnChange()
        {
            var newDynamicTree = diffApplier.ToDynamic(worldTreeService.EditingWorld);
            // todo: initialize this
            previousDynamicTree = previousDynamicTree ?? newDynamicTree;

            var diff = diffBuilder.BuildDiffs(previousDynamicTree, newDynamicTree, diffIdentityComparer);
            if (diff.IsEmpty)
                return;
            var undoable = new DiffUndoable(diffApplier, worldTreeService.EditingWorld, diff);
            undoStack.Push(undoable);
            redoStack.Clear();
            previousDynamicTree = newDynamicTree;
            NotifyObserver();
        }

        public void Undo()
        {
            ActualizeToLastState();
            var last = undoStack.Pop();
            last.Undo();
            redoStack.Push(last);
            UpdateDynamicTree();
            NotifyObserver();
        }

        public void Redo()
        {
            ActualizeToLastState();
            var last = redoStack.Pop();
            last.Apply();
            undoStack.Push(last);
            UpdateDynamicTree();
            NotifyObserver();
        }

        private void ActualizeToLastState()
        {
            var newDynamicTree = diffApplier.ToDynamic(worldTreeService.EditingWorld);
            // todo: initialize this
            previousDynamicTree = previousDynamicTree ?? newDynamicTree;

            var diff = diffBuilder.BuildDiffs(previousDynamicTree, newDynamicTree, diffIdentityComparer);
            if (!diff.IsEmpty)
                diffApplier.ApplyDiff(worldTreeService.EditingWorld, diff, TrwDiffDirection.Backward);
        }

        private void UpdateDynamicTree()
        {
            previousDynamicTree = diffApplier.ToDynamic(worldTreeService.EditingWorld);
        }

        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

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