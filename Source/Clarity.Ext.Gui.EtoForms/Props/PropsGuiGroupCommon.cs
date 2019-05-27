using System;
using System.Globalization;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupCommon : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly Label cId;
        private readonly TextBox cName;
        private ISceneNode boundNode;

        public PropsGuiGroupCommon(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            cId = new Label();

            cName = new TextBox();
            cName.TextChanged += OnNameTextChanged;

            var layout = new TableLayout(
                new TableRow(new Label { Text = "ID" }, cId),
                new TableRow(new Label { Text = "Name" }, cName)
                )
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5)
            };

            GroupBox = new GroupBox
            {
                Text = "Common",
                Content = layout
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundNode = null;

            var newNode = node;
            if (newNode == null)
                return false;

            cId.Text = node.Id.ToString(CultureInfo.InvariantCulture);
            cName.Text = node.Name;
            boundNode = newNode;
            return true;
        }

        private void OnNameTextChanged(object sender, EventArgs eventArgs)
        {
            if (boundNode == null)
                return;
            boundNode.Name = cName.Text;
            undoRedo.OnChange();
        }
    }
}