namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentContainerControl : IFluentControl
    {
        IFluentControl Content { get; set; }
        int Width { get; set; }
    }
}