using System;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentTableBuilder<T> : IFluentTableBuilder<T>
    {
        private readonly Func<T> getObject;
        private readonly Action<IFluentTableRowControl> addRow;
        private readonly Action onChildLayoutChanged;

        public FluentTableBuilder(Func<T> getObject, Action<IFluentTableRowControl> addRow, Action onChildLayoutChanged)
        {
            this.getObject = getObject;
            this.addRow = addRow;
            this.onChildLayoutChanged = onChildLayoutChanged;
        }

        public IFluentGuiBuilder<T> Row()
        {
            return Row(x => x);
        }

        public IFluentGuiBuilder<TChild> Row<TChild>(Func<T, TChild> getChild)
        {
            var row = new FluentTableRowControl<TChild>(onChildLayoutChanged, () => getChild(getObject()));
            addRow(row);
            return row.Build();
        }
    }
}