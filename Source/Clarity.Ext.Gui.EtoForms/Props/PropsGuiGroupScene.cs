using System;
using Clarity.Common.Numericals.Colors;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupScene : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly ColorPicker cBackgroundColor;
        private readonly DropDown cSkybox;
        private readonly ISkybox stormTexture;
        private readonly ISkybox starsTexture;
        private IScene boundComponent;

        public PropsGuiGroupScene(IUndoRedoService undoRedo, IEmbeddedResources embeddedResources)
        {
            this.undoRedo = undoRedo;

            cBackgroundColor = new ColorPicker();
            cBackgroundColor.ValueChanged += OnBackgroundColorChanged;

            cSkybox = new DropDown
            {
                Width = 120,
                DataStore = new[] {"None", "Storm", "Stars"}
            };
            cSkybox.SelectedValueChanged += OnSkyboxChanged;

            stormTexture = embeddedResources.Skybox("Skyboxes/storm.skybox");
            starsTexture = embeddedResources.Skybox("Skyboxes/stars.skybox");

            var layout = new TableLayout(
                new TableRow(new Label { Text = "Bgnd Color" }, cBackgroundColor),
                new TableRow(new Label { Text = "Skybox" }, cSkybox)
                )
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            GroupBox = new GroupBox
            {
                Text = "Scene",
                Content = layout
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = null;

            if (!(node.AmParent is IScene newAspect))
                return false;

            cBackgroundColor.Value = Color.FromArgb(newAspect.BackgroundColor.ToArgb());
            if (newAspect.Skybox == null)
                cSkybox.SelectedValue = "None";
            else if (newAspect.Skybox.Source.Equals(stormTexture.Source))
                cSkybox.SelectedValue = "Storm";
            else if (newAspect.Skybox.Source.Equals(starsTexture.Source))
                cSkybox.SelectedValue = "Stars";
            else
                cSkybox.SelectedValue = "None";

            boundComponent = newAspect;
            return true;
        }

        private void OnBackgroundColorChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            var color = new Color4(cBackgroundColor.Value.ToArgb());
            undoRedo.Common.ChangeProperty(boundComponent, x => x.BackgroundColor, color);
        }

        private void OnSkyboxChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            var skyboxName = (string)cSkybox.SelectedValue;
            var source = 
                skyboxName == "None" ? null : 
                skyboxName == "Storm" ? stormTexture : 
                skyboxName == "Stars" ? starsTexture : 
                null;
            undoRedo.Common.ChangeProperty(boundComponent, x => x.Skybox, source);
        }
    }
}