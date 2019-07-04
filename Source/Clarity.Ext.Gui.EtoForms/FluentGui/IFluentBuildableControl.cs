namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentBuildableControl : IFluentControl
    {
        void AddChild(IFluentControl control);
        void OnChildLayoutChanged();
    }

    public interface IFluentBuildableControl<T> : IFluentBuildableControl, IFluentControl<T>
    {
        IFluentGuiBuilder<T> Build();
    }
}