using Clarity.Engine.Gui;
using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Gui
{
    public class SceneNodeContextMenuBuilder : ISceneNodeContextMenuBuilder
    {
        public void Build(IGuiMenuBuilder builder, ISceneNode node)
        {
            foreach (var cGui in node.SearchComponents<IGuiComponent>())
            {
                builder.StartSection();
                cGui.BuildMenuSection(builder);
            }
        }
    }
}