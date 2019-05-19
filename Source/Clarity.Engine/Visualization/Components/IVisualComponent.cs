using System.Collections.Generic;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Visualization.Components
{
    public interface IVisualComponent : ISceneNodeComponent
    {
        IEnumerable<IVisualElement> GetVisualElements();
    }
}