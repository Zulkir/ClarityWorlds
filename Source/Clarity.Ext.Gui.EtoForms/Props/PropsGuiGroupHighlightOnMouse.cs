using System;
using Clarity.App.Worlds.Misc.HighlightOnMouse;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupHighlightOnMouse : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly TextBox cGroupName;
        private HighlightOnMouseComponent boundComponent;

        public PropsGuiGroupHighlightOnMouse(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            cGroupName = new TextBox();
            cGroupName.TextChanged += OnGroupNameChanged;

            var layout = new TableLayout(
                new TableRow(new Label{Text = "GrpName"}, cGroupName))
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            GroupBox = new GroupBox
            {
                Text = "Highlight on Mouse",
                Content = layout
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = null;
            var newBoundComponent = node.SearchComponent<HighlightOnMouseComponent>();
            if (newBoundComponent == null)
                return false;
            cGroupName.Text = newBoundComponent.GroupName;
            boundComponent = newBoundComponent;
            return true;
        }

        private void OnGroupNameChanged(object sender, EventArgs e)
        {
            if (boundComponent == null)
                return;
            boundComponent.GroupName = cGroupName.Text;
            undoRedo.OnChange();
        }
    }
}