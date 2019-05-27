using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.CopyPaste
{
    public interface ICopyPasteContent
    {
        ISceneNode Source { get; }
        ISceneNode Item { get; }
        bool Copy { get; }
    }
}