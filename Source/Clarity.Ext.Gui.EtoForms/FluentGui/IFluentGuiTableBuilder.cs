using System;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentGuiTableBuilder<T>
    {
        IFluentGuiBuilder<T> Row();
        IFluentGuiBuilder<TChild> Row<TChild>(Func<T, TChild> getChild);
    }
}