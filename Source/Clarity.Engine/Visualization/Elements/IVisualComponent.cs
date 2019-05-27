using System.Collections.Generic;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Elements.Effects;

namespace Clarity.Engine.Visualization.Elements
{
    public interface IVisualComponent : ISceneNodeComponent
    {
        IEnumerable<IVisualElement> GetVisualElements();
        IEnumerable<IVisualEffect> GetVisualEffects();
    }
}