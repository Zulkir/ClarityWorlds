using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.SaveLoad
{
    public interface ISaveLoadGuiCommands
    {
        Command New { get; }
        Command Save { get; }
        Command SaveAs { get; }
        Command Open { get; }
    }
}