using System.Collections.Generic;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad.ReadOnly
{
    public interface IReadOnlyOverrideComponent : ISceneNodeComponent
    {
        IEnumerable<ISceneNodeComponent> ToReadOnlyComponents();
    }
}