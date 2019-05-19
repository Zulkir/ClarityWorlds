using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class CommonGuiObjects : ICommonGuiObjects
    {
        public ContextMenu SelectionContextMenu { get; }

        public CommonGuiObjects()
        {
            SelectionContextMenu = new ContextMenu();
        }
    }
}