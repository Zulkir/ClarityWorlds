using Clarity.Engine.Interaction.Input.Keyboard;

namespace Clarity.Engine.Gui.Menus
{
    public interface IGuiCommand : IGuiMenuItem
    {
        KeyModifyers ShortcutModifyers { get; }
        Key ShortcutKey { get; }
        void Execute();
    }
}