using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.Media.Media3D;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree.MiscComponents;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Utilities;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public class FluentGuiService : IFluentGuiService
    {
        private readonly IUndoRedoService undoRedo;
        private readonly IViewService viewService;
        private readonly IAssetService assetService;

        private ISceneNode selectedSceneNode;
        private readonly IFluentControl rootControl;

        public Control RootEtoControl => rootControl.EtoControl;

        private class NewComponentViewModel
        {
            private readonly Func<ISceneNode> getNode;

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public Type ComponentType { get; set; }

            public NewComponentViewModel(Func<ISceneNode> getNode)
            {
                this.getNode = getNode;
            }

            public ISceneNode GetNode() => getNode();
        }

        public FluentGuiService(IViewService viewService, IRenderLoopDispatcher renderLoopDispatcher, IAssetService assetService, IUndoRedoService undoRedo, IEmbeddedResources embeddedResources)
        {
            this.viewService = viewService;
            this.assetService = assetService;
            this.undoRedo = undoRedo;

            var mainPanel = new FluentPanel<ISceneNode>(() => selectedSceneNode, x => x != null)
            {
                Width = 250
            };
            var builder = mainPanel.Build().Table();
            {
                var mainGroupBox = builder.Row().GroupBox("common", x => x, x => true).Table();
                mainGroupBox.Row().Label(x => x.Id.ToString());
                mainGroupBox.Row().TextBox(x => x.Name);
            }
            {
                var sceneGroupBox = builder.Row().GroupBox("Scene", x => x.AmParent as IScene, x => x != null).Table();
                var colorRow = sceneGroupBox.Row();
                colorRow.Label("Bgnd Color");
                colorRow.ColorPicker(x => x.BackgroundColor);
                var skyboxRow = sceneGroupBox.Row();
                skyboxRow.Label("Skybox");
                skyboxRow.DropDown(x => x.Skybox, new Dictionary<string, ISkybox>
                {
                    {"None", null},
                    {"Storm", embeddedResources.Skybox("Skyboxes/storm.skybox")},
                    {"Stars", embeddedResources.Skybox("Skyboxes/stars.skybox")}
                });
            }
            {
                var storyNode = builder.Row().GroupBox("Story Node", x => x.SearchComponent<IStoryComponent>(), x => x != null).Table();
                var row = storyNode.Row();
                row.CheckBox("Instant transition", x => x.InstantTransition);
                row.CheckBox("Skip", x => x.SkipOrder);
            }
            {
                var modelComponentBuilder = builder.Row().GroupBox("Model", x => x.SearchComponent<IModelComponent>(), x => x != null).Table();
                var colorRow = modelComponentBuilder.Row();
                colorRow.CheckBox("SingleColor", x => x.SingleColor);
                colorRow.ColorPicker(x => x.Color);
                var textureRow = modelComponentBuilder.Row();
                textureRow.Label("Image");
                textureRow.Button("Load", OnLoadTextureClicked);
                var checkBoxRow1 = modelComponentBuilder.Row();
                checkBoxRow1.CheckBox("Ignore Lighting", x => x.IgnoreLighting);
                checkBoxRow1.CheckBox("No Specular", x => x.NoSpecular);
                var checkBoxRow2 = modelComponentBuilder.Row();
                checkBoxRow2.CheckBox("Don't cull", x => x.DontCull);
                checkBoxRow2.CheckBox("Ortho", x => x.Ortho);
                modelComponentBuilder.Row().Button("Export", OnExportClick);
            }
            {
                var rectGroupBox = builder.Row().GroupBox("Rectangle", x => x, x => x.HasComponent<IRectangleComponent>()).Table();
                var commonRectRow = rectGroupBox.Row().Panel(x => x.SearchComponent<IRectangleComponent>(), x => x != null).Table().Row();
                commonRectRow.CheckBox("DragBorders", x => x.DragByBorders);
                var colorRow = rectGroupBox.Row().Panel(x => x.SearchComponent<ColorRectangleComponent>(), x => x != null).Table().Row();
                colorRow.Label("Color");
                colorRow.ColorPicker(x => x.Color);
                // todo: change image
            }
            {
                var componentsBuilder = builder.Row().GroupBox("Components", x => x, x => x != null).Table();
                // todo: to ArrayTable
                var componentsCache = (IEnumerable<ISceneNodeComponent>)null;
                var componentsStringCache = "";
                componentsBuilder.Row().Label(n =>
                {
                    if (n.Components != componentsCache)
                    {
                        componentsCache = n.Components;
                        componentsStringCache = string.Join("\n", n.Components.Select(c => c.AmInterface.Name));
                    }
                    return componentsStringCache;
                });

                var newComponentViewModel = new NewComponentViewModel(() => selectedSceneNode);
                var newComponentPanelBuilder = componentsBuilder.Row().Panel(x => newComponentViewModel, x => true).Table();
                var newComponentRow = newComponentPanelBuilder.Row();
                newComponentRow.DropDown(x => x.ComponentType, new Dictionary<string, Type>
                {
                    ["RotateOnce"] = typeof(RotateOnceComponent),
                });
                newComponentRow.Button("Add", x =>
                {
                    componentsCache = null;
                    if (x.ComponentType == typeof(RotateOnceComponent))
                    {
                        var component = AmFactory.Create<RotateOnceComponent>();
                        newComponentViewModel.GetNode().Components.Add(component);
                        undoRedo.OnChange();
                    }
                    else
                    {
                        throw new Exception("Wrong Choice");
                    }
                });
            }
            builder.Row().Panel(x => x, x => true);

            rootControl = mainPanel;
            renderLoopDispatcher.AfterUpdate += Update;
        }
        
        private void OnExportClick(IModelComponent component)
        {
            switch (component.Model)
            {
                case IFlexibleModel flexibleModel:
                {
                    var dialog = new SaveFileDialog();
                    dialog.FileName = ".cgm";
                    var result = dialog.ShowDialog(RootEtoControl.ParentWindow);
                    if (result != DialogResult.Ok)
                        return;
                    var fileName = dialog.FileName;
                    using (var writer = new StreamWriter(fileName))
                        FlexibleModelWriter.WriteModel(writer, flexibleModel);
                    break;
                }
            }
        }

        private void OnLoadTextureClicked (IModelComponent component)
        {
            var imageOpenFileDialog = new OpenFileDialog();
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("All Images", ".bmp", ".jpg", ".jpeg", ".png", ".tga"));
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("BMP", ".bmp"));
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("JPEG", ".jpg", ".jpeg"));
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("PNG", ".png"));
            imageOpenFileDialog.Filters.Add(new FileDialogFilter("TGA", ".tga"));
            imageOpenFileDialog.ShowDialog(RootEtoControl.ParentWindow);
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
            component.Texture = texture;
            undoRedo.OnChange();
        }

        public void Update(FrameTime frameTime)
        {
            selectedSceneNode = viewService.SelectedNode;
            if (rootControl.IsVisible)
                rootControl.Update();
        }
    }
}