using System.Collections.Generic;

namespace Clarity.Engine.Gui.Menus
{
    public interface IGuiMenu
    {
        IReadOnlyList<IGuiMenuSection> Sections { get; }
    }
}