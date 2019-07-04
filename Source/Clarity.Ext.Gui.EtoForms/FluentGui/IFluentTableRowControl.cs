namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentTableRowControl : IFluentBuildableControl
    {
    }

    public interface IFluentTableRowControl<T> : IFluentTableRowControl, IFluentBuildableControl<T>
    {
    }
}