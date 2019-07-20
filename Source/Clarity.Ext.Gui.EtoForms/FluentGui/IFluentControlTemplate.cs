using System;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentControlTemplate<in TParent>
    {
        IFluentControl Instantiate(Func<TParent> getParentObj);
    }
}