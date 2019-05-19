using System.Collections.Generic;

namespace Clarity.Engine.Gui.Menus
{
    public interface IGuiMenuSection
    {
        IReadOnlyList<IGuiMenuItem> Items { get; }
    }
}