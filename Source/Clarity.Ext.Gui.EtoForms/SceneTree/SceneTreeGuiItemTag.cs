using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Ext.Gui.EtoForms.SceneTree
{
    public class SceneTreeGuiItemTag
    {
        public SceneTreeGuiItemType Type { get; private set; }
        public ISceneNode Node { get; private set; }

        public SceneTreeGuiItemTag(ISceneNode node)
        {
            Node = node;
            Type = SceneTreeGuiItemType.Unknown;
        }
    }
}