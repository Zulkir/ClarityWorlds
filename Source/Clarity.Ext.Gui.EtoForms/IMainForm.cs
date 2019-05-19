using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public interface IMainForm
    {
        Form Form { get; }
        RenderControl RenderControl { get; }
    }
}