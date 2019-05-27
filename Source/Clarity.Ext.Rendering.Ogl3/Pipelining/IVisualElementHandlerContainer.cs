using Clarity.Engine.Visualization.Elements;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining
{
    public interface IVisualElementHandlerContainer
    {
        IVisualElementHandler ChooseHandler(IVisualElement visualElement);
    }
}