using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.CopyPaste
{
    public class WorldCopyPasteService : IWorldCopyPasteService
    {
        private readonly IUndoRedoService undoRedo;
        private ICopyPasteContent content;

        public WorldCopyPasteService(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;
        }

        public void Copy(ISceneNode item) => 
            content = new CopyPasteContent(item.ParentNode, item.Clone<ISceneNode>(), true);

        public void Cut(ISceneNode item) =>
            content = new CopyPasteContent(item.ParentNode, item, false);

        public bool CanPasteTo(ISceneNode destination) => true;

        public void PasteTo(ISceneNode destination) => 
            undoRedo.Apply(new WorldPasteUndoable(content, destination));

        public void Duplicate(ISceneNode item) => 
            undoRedo.Apply(new SceneNodeDuplicateUndoable(item));

        public void MoveUp(ISceneNode item)
        {
            var parent = item.ParentNode;
            var index = parent.ChildNodes.IndexOf(item);
            if (index == 0)
                return;
            undoRedo.Apply(new SwapWorldItemsUndoable(item.ParentNode, index - 1, index));
        }

        public void MoveDown(ISceneNode item)
        {
            var parent = item.ParentNode;
            var index = parent.ChildNodes.IndexOf(item);
            if (index == parent.ChildNodes.Count - 1)
                return;
            undoRedo.Apply(new SwapWorldItemsUndoable(item.ParentNode, index, index + 1));
        }
    }
}