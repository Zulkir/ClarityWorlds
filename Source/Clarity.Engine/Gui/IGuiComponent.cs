using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Gui
{
    public interface IGuiComponent : ISceneNodeComponent
    {
        void BuildMenuSection(IGuiMenuBuilder menuBuilder);
    }
}