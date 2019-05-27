using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.CopyPaste
{
    public interface ICopyPasteComponent : ISceneNodeComponent
    {
        bool CanExecute(CopyPasteCommand command);
        void Execute(CopyPasteCommand command);
    }
}