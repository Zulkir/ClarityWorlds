namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentTableControl<T> : IFluentControl<T>
    {
        void AddRow(IFluentTableRowControl row);
        IFluentGuiTableBuilder<T> Build();
        void OnChildLayoutChanged();
    }
}