using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.CopyPaste
{
    public class CopyPasteContent : ICopyPasteContent
    {
        public ISceneNode Source { get; }
        public ISceneNode Item { get; }
        public bool Copy { get; }

        public CopyPasteContent(ISceneNode source, ISceneNode item, bool copy)
        {
            Source = source;
            Item = item;
            Copy = copy;
        }
    }
}