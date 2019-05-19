using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Visualization.Components
{
    public interface IVisualElement
    {
        ICgVisualElement Fallback { get; }
        bool Hide { get; }
    }
}