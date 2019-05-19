using System.Collections.Generic;

namespace Clarity.Engine.Gui.Menus
{
    public class GuiMenu : IGuiMenu
    {
        public IReadOnlyList<IGuiMenuSection> Sections { get; }

        public GuiMenu(IReadOnlyList<IGuiMenuSection> sections)
        {
            Sections = sections;
        }
    }
}