using System;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentGuiTableBuilder<T> : IFluentGuiTableBuilder<T>
    {
        private readonly IFluentTableControl<T> control;

        public FluentGuiTableBuilder(IFluentTableControl<T> control)
        {
            this.control = control;
        }

        public IFluentGuiBuilder<T> Row()
        {
            return Row(x => x);
        }

        public IFluentGuiBuilder<TChild> Row<TChild>(Func<T, TChild> getChild)
        {
            var row = new FluentTableRowControl<TChild>(control.OnChildLayoutChanged, () => getChild(control.GetObject()));
            control.AddRow(row);
            return row.Build();
        }
    }
}