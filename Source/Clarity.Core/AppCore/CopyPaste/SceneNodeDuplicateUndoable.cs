using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.CopyPaste
{
    public class SceneNodeDuplicateUndoable : IUndoable
    {
        private readonly ISceneNode node;
        private readonly ISceneNode copy;
        private readonly ISceneNode parent;

        public SceneNodeDuplicateUndoable(ISceneNode node)
        {
            this.node = node;
            copy = node.Clone<ISceneNode>();
            parent = node.ParentNode;
        }

        public void Apply()
        {
            parent.ChildNodes.Insert(parent.ChildNodes.IndexOf(node) + 1, copy);
        }

        public void Undo()
        {
            parent.ChildNodes.Remove(copy);
        }
    }
}