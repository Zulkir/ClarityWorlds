using System;
using Clarity.Engine.Interaction.Input.Keyboard;

namespace Clarity.Engine.Gui.Menus
{
    public class GuiCommand : IGuiCommand
    {
        public string Text { get; }
        public KeyModifiers ShortcutModifiers { get; }
        public Key ShortcutKey { get; }
        public Action Action { get; }

        public GuiCommand(string text, KeyModifiers shortcutModifiers, Key shortcutKey, Action action)
        {
            Text = text;
            ShortcutModifiers = shortcutModifiers;
            ShortcutKey = shortcutKey;
            Action = action;
        }

        public GuiCommand(string text, Key shortcutKey, Action action) : this(text, KeyModifiers.None, shortcutKey, action)
        {
        }

        public GuiCommand(string text, Action action) : this(text, KeyModifiers.None, Key.None, action)
        {
        }
        
        public void Execute() => Action();
    }
}