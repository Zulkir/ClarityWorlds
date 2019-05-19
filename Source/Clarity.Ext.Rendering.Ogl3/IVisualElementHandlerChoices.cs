using Clarity.Engine.Visualization.Components;

namespace Clarity.Ext.Rendering.Ogl3
{
    public interface IVisualElementHandlerChoices
    {
        IVisualElementHandler ChooseHandler(IVisualElement visualElement);
    }
}