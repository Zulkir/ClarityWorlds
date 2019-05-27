using System.Collections.Generic;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentContainerControl<T> : IFluentControl
    {
        IList<IFluentControl> Children { get; }
        int Width { get; set; }

        T GetObject();
        IFluentGuiBuilder<T> Build();
    }
}