using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentTableRowControl : IFluentBuildableControl
    {
        TableRow EtoTableRow { get; }
    }

    public interface IFluentTableRowControl<T> : IFluentTableRowControl, IFluentBuildableControl<T>
    {
    }
}