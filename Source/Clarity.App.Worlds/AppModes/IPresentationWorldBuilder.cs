using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.AppModes
{
    public interface IPresentationWorldBuilder
    {
        IWorld BuildPreesentationWorld(IWorld editingWorld);
    }
}