using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.App.Worlds.CopyPaste
{
    public class WorldPasteUndoable : IUndoable
    {
        private readonly ISceneNode source;
        private readonly ISceneNode destination;
        private readonly ISceneNode sourceItem;
        private readonly bool copy;
        private ISceneNode destinationItem;
        private int index;

        public WorldPasteUndoable(ICopyPasteContent content, ISceneNode destination)
        {
            source = content.Source;
            sourceItem = content.Item;
            copy = content.Copy;
            this.destination = destination;
        }

        public void Apply()
        {
            index = source.ChildNodes.IndexOf(sourceItem);
            if (copy)
            {
                destinationItem = sourceItem.CloneTyped();
                destinationItem.Id = AmFactory.Create<SceneNode>().Id;
            }
            else
            {
                destinationItem = sourceItem;
            destinationItem.Deparent();
                
            }
            destination.ChildNodes.Add(destinationItem);
        }

        public void Undo()
        {
            if (copy)
                destination.ChildNodes.Remove(destinationItem);
            else
            {
                destinationItem.Deparent();
                source.ChildNodes.Insert(index, destinationItem);
            }
        }
    }
}