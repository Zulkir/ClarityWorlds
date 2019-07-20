using System;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentTableBuilder<T>
    {
        IFluentGuiBuilder<T> Row();
        IFluentGuiBuilder<TChild> Row<TChild>(Func<T, TChild> getChild);
    }
}