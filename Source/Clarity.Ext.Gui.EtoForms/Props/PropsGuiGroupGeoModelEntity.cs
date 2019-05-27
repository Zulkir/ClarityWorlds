using System;
using Clarity.Common.Numericals.Colors;
using System.IO;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Media.Media3D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    // todo: refactor from Visual to Component
    public class PropsGuiGroupGeoModelEntity : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IAssetService assetService;
        private readonly IUndoRedoService undoRedo;
        private readonly ColorPicker colorControl;
        private readonly CheckBox cIgnoreLighting;
        private readonly CheckBox cNoSpecular;
        private readonly CheckBox cSingleColor;
        private readonly CheckBox cOrtho;
        private readonly CheckBox cDontCull;
        private readonly Button cExport;
        private ModelComponent boundComponent;

        public PropsGuiGroupGeoModelEntity(IUndoRedoService undoRedo, IAssetService assetService)
        {
            this.undoRedo = undoRedo;
            this.assetService = assetService;

            colorControl = new ColorPicker();
            colorControl.ValueChanged += OnColorChanged;

            var cLoadTexture = new Button { Text = "Load" };
            cLoadTexture.Click += OnLoadTextureClicked;

            cIgnoreLighting = new CheckBox { Text = "Ignore Lighting" };
            cIgnoreLighting.CheckedChanged += OnIgnoreLightingChanged;

            cNoSpecular = new CheckBox { Text = "No Specular" };
            cNoSpecular.CheckedChanged += OnNoSpecularChanged;

            cSingleColor = new CheckBox { Text = "Single Color" };
            cSingleColor.CheckedChanged += OnSingleColorChanged;

            cOrtho = new CheckBox { Text = "Ortho" };
            cOrtho.CheckedChanged += OnOrthoChanged;

            cDontCull = new CheckBox { Text = "Don't Cull" };
            cDontCull.CheckedChanged += OnDontCullChanged;

            cExport = new Button { Text = "Export" };
            cExport.Click += OnExportClick;

            var layout = new TableLayout(
                new TableRow(cSingleColor, colorControl),
                new TableRow(new Label { Text = "Image" }, cLoadTexture),
                new TableRow(cIgnoreLighting, cNoSpecular),
                new TableRow(cDontCull, cOrtho),
                new TableRow(cExport))
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            GroupBox = new GroupBox
            {
                Text = "Visual",
                Content = layout
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = null;

            var newComponent = node.SearchComponent<ModelComponent>();
            if (newComponent == null)
                return false;

            colorControl.Value = Color.FromArgb(newComponent.Color.ToArgb());
            cIgnoreLighting.Checked = newComponent.IgnoreLighting;
            cNoSpecular.Checked = newComponent.NoSpecular;
            cSingleColor.Checked = newComponent.SingleColor;
            cOrtho.Checked = newComponent.Ortho;
            cDontCull.Checked = newComponent.DontCull;

            boundComponent = newComponent;
            return true;
        }

        private void OnColorChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            var eColor = colorControl.Value;
            var cColor = new Color4(eColor.R, eColor.G, eColor.B, eColor.A);
            boundComponent.Color = cColor;
            undoRedo.OnChange();
        }

        private void OnLoadTextureClicked(object sender, EventArgs eventArgs)
        {
            var imageOpenFileDialog = new OpenFileDialog();
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("All Images", ".bmp", ".jpg", ".jpeg", ".png", ".tga"));
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("BMP", ".bmp"));
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("JPEG", ".jpg", ".jpeg"));
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("PNG", ".png"));
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("TGA", ".tga"));

            imageOpenFileDialog.ShowDialog(GroupBox);
            var loadPath = imageOpenFileDialog.FileName;
            if (loadPath == null)
                return;
            var loadInfo = new AssetLoadInfo
            {
                FileSystem = new ActualFileSystem(),
                LoadPath = loadPath,
                ReferencePath = loadPath,
                StorageType = AssetStorageType.CopyLocal
            };
            var assetLoadResult = assetService.Load(loadInfo);
            var texture = assetLoadResult.Successful ? assetLoadResult.Asset.Resource as IImage : null;
            boundComponent.Texture = texture;
            undoRedo.OnChange();
        }

        private void OnIgnoreLightingChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            boundComponent.IgnoreLighting = cIgnoreLighting.Checked ?? false;
            undoRedo.OnChange();
        }

        private void OnNoSpecularChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            boundComponent.NoSpecular = cNoSpecular.Checked ?? false;
            undoRedo.OnChange();
        }

        private void OnSingleColorChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            boundComponent.SingleColor = cSingleColor.Checked ?? false;
            undoRedo.OnChange();
        }

        private void OnOrthoChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            boundComponent.Ortho = cOrtho.Checked ?? false;
            undoRedo.OnChange();
        }

        private void OnDontCullChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            boundComponent.DontCull = cDontCull.Checked ?? false;
            undoRedo.OnChange();
        }

        private void OnExportClick(object sender, EventArgs eventArgs)
        {
            switch (boundComponent.Model)
            {
                case IFlexibleModel flexibleModel:
                {
                    var dialog = new SaveFileDialog();
                    dialog.FileName = ".cgm";
                    var result = dialog.ShowDialog(cExport.ParentWindow);
                    if (result != DialogResult.Ok)
                        return;
                    var fileName = dialog.FileName;
                    using (var writer = new StreamWriter(fileName))
                        FlexibleModelWriter.WriteModel(writer, flexibleModel);
                    break;
                }
            }
        }
    }
}