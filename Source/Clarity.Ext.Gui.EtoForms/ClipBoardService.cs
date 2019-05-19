using Clarity.Engine.Gui;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class ClipboardService : IClipboardService
    {
        private readonly Clipboard clipboard;

        public ClipboardService()
        {
            clipboard = new Clipboard();
        }

        public void CopyString(string str)
        {
            clipboard.Text = str;
        }

        public bool TryGetString(out string str)
        {
            str = clipboard.Text;
            return !string.IsNullOrEmpty(str);
        }
    }
}