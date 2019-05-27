using Clarity.Engine.Objects.WorldTree;
using JetBrains.Annotations;

namespace Clarity.App.Worlds.WorldTree
{
    public interface IWorldTreeService
    {
        // todo: remove these two
        [CanBeNull]
        IWorld ParentWorld { get; set; }
        [NotNull]
        IWorld World { get; set; }

        IWorld EditingWorld { get; }
        IWorld PresentationWorld { get; }

        ISceneNode MainRoot { get; }

        ISceneNode GetById(int id);
        bool TryGetById(int id, out ISceneNode node);
    }
}