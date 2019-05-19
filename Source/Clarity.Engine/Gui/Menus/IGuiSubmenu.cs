namespace Clarity.Engine.Gui.Menus
{
    public interface IGuiSubmenu : IGuiMenuItem
    {
        IGuiMenu InternalMenu { get; }
    }
}