using System;
using System.Globalization;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
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
        private readonly TextBox cScale;
        private readonly TextBox cOffsetX;
        private readonly TextBox cOffsetY;
        private readonly TextBox cOffsetZ;
        private ISceneNode boundNode;

        public PropsGuiGroupCommon(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            cId = new Label();

            cName = new TextBox();
            cName.TextChanged += OnNameTextChanged;

            cScale = new TextBox { Width = 50 };
            cOffsetX = new TextBox { Width = 50 };
            cOffsetY = new TextBox { Width = 50 };
            cOffsetZ = new TextBox { Width = 50 };
            cScale.TextChanged += OnTransformChanged;
            cOffsetX.TextChanged += OnTransformChanged;
            cOffsetY.TextChanged += OnTransformChanged;
            cOffsetZ.TextChanged += OnTransformChanged;

            var layout = new TableLayout(
                new TableLayout(
                    new TableRow(new Label { Text = "ID" }, cId),
                    new TableRow(new Label { Text = "Name" }, cName))
                {
                    Padding = new Padding(5),
                    Spacing = new Size(5, 5)
                },
                new TableLayout(new TableRow(
                    cScale, cOffsetX, cOffsetY, cOffsetZ, new TableCell()))
                {
                    Padding = new Padding(5),
                    Spacing = new Size(5, 5)
                }
            );

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
            cScale.Text = node.Transform.Scale.ToString(CultureInfo.InvariantCulture);
            cOffsetX.Text = node.Transform.Offset.X.ToString(CultureInfo.InvariantCulture);
            cOffsetY.Text = node.Transform.Offset.Y.ToString(CultureInfo.InvariantCulture);
            cOffsetZ.Text = node.Transform.Offset.Z.ToString(CultureInfo.InvariantCulture);
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

        private void OnTransformChanged(object sender, EventArgs eventArgs)
        {
            if (boundNode == null)
                return;
            var scale = float.TryParse(cScale.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var pScale) ? pScale : boundNode.Transform.Scale;
            var x = float.TryParse(cOffsetX.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var px) ? px : boundNode.Transform.Offset.X;
            var y = float.TryParse(cOffsetY.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var py) ? py : boundNode.Transform.Offset.Y;
            var z = float.TryParse(cOffsetZ.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var pz) ? pz : boundNode.Transform.Offset.Z;
            boundNode.Transform = new Transform(
                scale,
                boundNode.Transform.Rotation,
                new Vector3(x, y, z));
        }
    }
}