using System.Collections.Generic;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentTableControl<T> : IFluentControl<T>
    {
        void AddRow(IFluentTableRowControl row);
        IFluentTableBuilder<T> Build();
        void OnChildLayoutChanged();
    }

    public interface IFluentArrayTableControl<T> : IFluentControl<IEnumerable<T>>, IFluentBuildableControl
    {

    }
}