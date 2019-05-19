using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.CopyPaste
{
    public interface IWorldCopyPasteService
    {
        void Copy(ISceneNode item);
        void Cut(ISceneNode item);
        bool CanPasteTo(ISceneNode destination);
        void PasteTo(ISceneNode destination);
        void Duplicate(ISceneNode item);
        void MoveUp(ISceneNode item);
        void MoveDown(ISceneNode item);
    }
}