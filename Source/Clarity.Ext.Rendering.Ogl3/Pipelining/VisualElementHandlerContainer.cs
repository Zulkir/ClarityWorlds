using System.Collections.Generic;
using System.Linq;
using Clarity.Engine.Visualization.Elements;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining
{
    public class VisualElementHandlerContainer : IVisualElementHandlerContainer
    {
        private readonly IReadOnlyList<IVisualElementHandler> handlers;

        public VisualElementHandlerContainer(IReadOnlyList<IVisualElementHandler> handlers)
        {
            this.handlers = handlers;
        }

        public IVisualElementHandler ChooseHandler(IVisualElement visualElement) => 
            handlers.First(x => x.CanHandle(visualElement));
    }
}