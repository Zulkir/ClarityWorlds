using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.Gui
{
    public interface ISceneNodeContextMenuBuilder
    {
        void Build(IGuiMenuBuilder builder, ISceneNode node);
    }
}