using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public static class FlagExtensions
    {
        public static bool HasFlagFast(this MouseButtons val, MouseButtons flag) { return (val & flag) != 0; }
    }
}