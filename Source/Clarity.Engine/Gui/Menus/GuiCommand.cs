using System;
using Clarity.Engine.Interaction.Input.Keyboard;

namespace Clarity.Engine.Gui.Menus
{
    public class GuiCommand : IGuiCommand
    {
        public string Text { get; }
        public KeyModifyers ShortcutModifyers { get; }
        public Key ShortcutKey { get; }
        public Action Action { get; }

        public GuiCommand(string text, KeyModifyers shortcutModifyers, Key shortcutKey, Action action)
        {
            Text = text;
            ShortcutModifyers = shortcutModifyers;
            ShortcutKey = shortcutKey;
            Action = action;
        }

        public GuiCommand(string text, Key shortcutKey, Action action) : this(text, KeyModifyers.None, shortcutKey, action)
        {
        }

        public GuiCommand(string text, Action action) : this(text, KeyModifyers.None, Key.None, action)
        {
        }
        
        public void Execute() => Action();
    }
}