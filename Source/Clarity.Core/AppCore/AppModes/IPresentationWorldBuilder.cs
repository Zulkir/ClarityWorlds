using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.AppModes
{
    public interface IPresentationWorldBuilder
    {
        IWorld BuildPreesentationWorld(IWorld editingWorld);
    }
}