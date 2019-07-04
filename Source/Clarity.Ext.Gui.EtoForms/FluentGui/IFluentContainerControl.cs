namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentContainerControl : IFluentBuildableControl
    {
        IFluentControl Content { get; set; }
        int Width { get; set; }
    }

    public interface IFluentContainerControl<T> : IFluentContainerControl, IFluentBuildableControl<T>
    {
    }
}