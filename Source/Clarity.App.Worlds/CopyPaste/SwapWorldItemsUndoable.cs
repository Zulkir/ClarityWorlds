using Clarity.App.Worlds.UndoRedo;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.CopyPaste
{
    public class SwapWorldItemsUndoable : IUndoable
    {
        private readonly ISceneNode parent;
        private readonly int index1;
        private readonly int index2;

        public SwapWorldItemsUndoable(ISceneNode parent, int index1, int index2)
        {
            this.parent = parent;
            this.index1 = index1;
            this.index2 = index2;
        }

        public void Apply() => Swap();
        public void Undo() => Swap();

        private void Swap()
        {
            var node1 = parent.ChildNodes[index1];
            var node2 = parent.ChildNodes[index2];

            if (index1 < index2)
            {
                node2.Deparent();
                parent.ChildNodes.Insert(index1, node2);
                node1.Deparent();
                parent.ChildNodes.Insert(index2, node1);
            }
            else
            {
                node1.Deparent();
                parent.ChildNodes.Insert(index2, node1);
                node2.Deparent();
                parent.ChildNodes.Insert(index1, node2);
            }
        }
    }
}