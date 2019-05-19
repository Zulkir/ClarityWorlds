using System;
using Clarity.Common.Numericals.Colors;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Core.AppFeatures.Text;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupRectangleEntity : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly ColorPicker cColor;
        private readonly CheckBox cDragByBorder;
        private IRectangleComponent boundComponent;

        public PropsGuiGroupRectangleEntity(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            cColor = new ColorPicker();
            cColor.ValueChanged += OnColorChanged;

            cDragByBorder = new CheckBox();
            cDragByBorder.CheckedChanged += OnDragByBorderChanged;

            var layout = new TableLayout(
                new TableRow(new Label { Text = "Color" }, cColor),
                new TableRow(new Label { Text = "DragBorders" }, cDragByBorder))
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            GroupBox = new GroupBox
            {
                Text = "Rectangle",
                Content = layout
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = null;

            var newNode = node.SearchComponent<IRectangleComponent>();
            if (newNode == null)
                return false;

            var colorComponent = node.SearchComponent<ColorRectangleComponent>();
            var imageComponent = node.SearchComponent<ImageRectangleComponent>();
            var movieComponent = node.SearchComponent<MovieRectangleComponent>();
            var textComponent = node.SearchComponent<RichTextComponent>();

            cColor.Enabled = colorComponent != null;
            cColor.Value = Color.FromArgb((colorComponent?.Color ?? Color4.White).ToArgb());

            cDragByBorder.Checked = newNode.DragByBorders;

            boundComponent = newNode;
            return true;
        }

        private void OnColorChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            var colorComponent = boundComponent.Node.SearchComponent<ColorRectangleComponent>();
            var eColor = this.cColor.Value;
            var cColor = new Color4(eColor.R, eColor.G, eColor.B, eColor.A);
            undoRedo.Common.ChangeProperty(colorComponent, x => x.Color, cColor);
        }

        private void OnDragByBorderChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            boundComponent.DragByBorders = cDragByBorder.Checked ?? false;
        }
    }
}