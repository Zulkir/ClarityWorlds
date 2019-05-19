using System;
using Clarity.Common.Numericals.Colors;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupAbstractNode : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly CheckBox cInstantTransition;
        private readonly CheckBox cSkipOrder;
        private readonly CheckBox cUseBackGroundColor;
        private readonly ColorPicker cBackgroundColor;
        private IStoryComponent boundComponent;

        public PropsGuiGroupAbstractNode(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            cInstantTransition = new CheckBox { Text = "Instant transition" };
            cInstantTransition.CheckedChanged += OnInstantTransitionFlagChanged;

            cSkipOrder = new CheckBox { Text = "Skip" };
            cSkipOrder.CheckedChanged += OnSkipOrderChanged;

            cUseBackGroundColor = new CheckBox { Text = "Bgnd Color"};
            cUseBackGroundColor.CheckedChanged += OnBackgroundColorChanged;

            cBackgroundColor = new ColorPicker();
            cBackgroundColor.ValueChanged += OnBackgroundColorChanged;

            //var cDecorate = new Button { Text = "Decorate" };
            //cDecorate.Click += (s, a) => { boundComponent.Redecorate(); };

            var cUpdateThumbnails = new Button { Text = "Update Thumbnails" };
            cUpdateThumbnails.Click += (s, a) => { boundComponent.InvalidateThumbnails(); };

            var layout = new TableLayout(
                new TableRow(cInstantTransition, cSkipOrder),
                new TableRow(cUseBackGroundColor, cBackgroundColor)
                //new TableRow(cDecorate, cUpdateThumbnails)
                )
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5)
            };

            GroupBox = new GroupBox
            {
                Text = "Abstract Node",
                Content = layout
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = null;

            var newComponent = node.SearchComponent<StoryComponent>();
            if (newComponent == null)
                return false;

            cInstantTransition.Checked = newComponent.InstantTransition;
            cSkipOrder.Checked = newComponent.SkipOrder;
            //cUseBackGroundColor.Checked = newComponent.BackgroundComponent.Color != null;
            //if (newComponent.BackgroundComponent.Color != null)
            //    cBackgroundColor.Value = Color.FromArgb(newComponent.BackgroundComponent.Color.Value.ToArgb());

            boundComponent = newComponent;
            return true;
        }

        private void OnInstantTransitionFlagChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            undoRedo.Common.ChangeProperty(boundComponent, x => x.InstantTransition, cInstantTransition.Checked ?? false);
        }

        private void OnSkipOrderChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            undoRedo.Common.ChangeProperty(boundComponent, x => x.SkipOrder, cSkipOrder.Checked ?? false);
        }

        private void OnBackgroundColorChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            var newValue = (cUseBackGroundColor.Checked ?? false)
                ? (Color4?)new Color4(cBackgroundColor.Value.ToArgb())
                : null;
            undoRedo.Common.ChangeProperty(boundComponent, x => x.BackgroundColor, newValue);
        }
    }
}