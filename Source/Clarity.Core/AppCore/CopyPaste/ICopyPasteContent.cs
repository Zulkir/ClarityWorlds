using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.CopyPaste
{
    public interface ICopyPasteContent
    {
        ISceneNode Source { get; }
        ISceneNode Item { get; }
        bool Copy { get; }
    }
}