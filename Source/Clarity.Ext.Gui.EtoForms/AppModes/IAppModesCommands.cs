using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.AppModes
{
    public interface IAppModesCommands
    {
        Command StartPresentation { get; }
        Command StopPresentation { get; }
    }
}