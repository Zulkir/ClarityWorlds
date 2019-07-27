using Clarity.Engine.Interaction.Input.Keyboard;

namespace Clarity.Engine.Gui.Menus
{
    public interface IGuiCommand : IGuiMenuItem
    {
        KeyModifiers ShortcutModifiers { get; }
        Key ShortcutKey { get; }
        void Execute();
    }
}