using System.Collections.Generic;
using System.Linq;
using Clarity.Engine.Visualization.Components;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class VisualElementHandlerChoices : IVisualElementHandlerChoices
    {
        private readonly IReadOnlyList<IVisualElementHandler> handlers;

        public VisualElementHandlerChoices(IReadOnlyList<IVisualElementHandler> handlers)
        {
            this.handlers = handlers;
        }

        public IVisualElementHandler ChooseHandler(IVisualElement visualElement) => 
            handlers.First(x => x.CanHandle(visualElement));
    }
}