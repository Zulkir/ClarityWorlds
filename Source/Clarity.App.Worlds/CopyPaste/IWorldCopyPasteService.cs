using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.CopyPaste
{
    public interface IWorldCopyPasteService
    {
        string Text { get; set; }
        string Html { get; set; }
        IImage Image { get; set; }
        ISceneNode Node { get; set; }

        bool Contains(string dataType);

        void Clear();
    }
}