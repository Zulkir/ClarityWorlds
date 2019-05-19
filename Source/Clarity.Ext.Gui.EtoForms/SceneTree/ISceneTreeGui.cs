using Clarity.Engine.Objects.WorldTree;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.SceneTree
{
    public interface ISceneTreeGui
    {
        TreeView TreeView { get; }
        TreeItem SelectedItem { get; }
        SceneTreeGuiItemTag SelectedItemTag { get; }
        ISceneNode SelectedNode { get; }
    }
}