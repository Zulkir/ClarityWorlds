namespace Clarity.Engine.Gui.Menus
{
    public interface IGuiMenuBuilder
    {
        void StartSection();
        void AddCommand(IGuiCommand command);
        IGuiMenuBuilder AddSubmenu(string text);
    }
}