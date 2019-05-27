using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad.ReadOnly
{
    public interface IReadOnlyWorldBuilder
    {
        IWorld BuildReadOnly(IWorld originalWorld);
    }
}