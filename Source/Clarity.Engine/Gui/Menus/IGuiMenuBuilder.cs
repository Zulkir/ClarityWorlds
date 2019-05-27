namespace Clarity.Engine.Gui.Menus
{
    public interface IGuiMenuBuilder
    {
        void StartSection();
        void AddCommand(IGuiCommand command, bool enabled = true);
        IGuiMenuBuilder AddSubmenu(string text);
    }
}