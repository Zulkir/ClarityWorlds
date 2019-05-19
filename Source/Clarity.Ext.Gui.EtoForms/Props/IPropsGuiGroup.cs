using Clarity.Engine.Objects.WorldTree;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public interface IPropsGuiGroup
    {
        GroupBox GroupBox { get; }
        bool Actualize(ISceneNode node);
    }
}