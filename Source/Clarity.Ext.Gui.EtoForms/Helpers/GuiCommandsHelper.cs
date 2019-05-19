using System;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Helpers
{
    public static class GuiCommandsHelper
    {
        public static Command Create(string text, EventHandler<EventArgs> handler, Keys? shortcut = null)
        {
            var command = new Command(handler)
            {
                MenuText = text,
                ToolBarText = text,
                ToolTip = text
            };
            if (shortcut.HasValue)
                command.Shortcut = shortcut.Value;
            return command;
        }
    }
}