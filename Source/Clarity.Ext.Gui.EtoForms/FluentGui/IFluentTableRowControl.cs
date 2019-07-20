using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentTableRowControl : IFluentControl
    {
        TableRow EtoTableRow { get; }
    }
}