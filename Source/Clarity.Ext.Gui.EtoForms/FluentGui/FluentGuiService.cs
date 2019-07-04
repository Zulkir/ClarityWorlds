using System;
using System.Collections.Generic;
using System.IO;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.Media.Media3D;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
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
            var mainGroupBox = builder.Row().GroupBox("common", x => x, x => true).Table();
            mainGroupBox.Row().Label(x => x.Id.ToString());
            mainGroupBox.Row().TextBox(x => x.Name);
            {
                var modelComponentBuilder = builder.Row().GroupBox("Model", x => x.SearchComponent<IModelComponent>(), x => x != null).Table();
                modelComponentBuilder.Row().ColorPicker("Color", x => x.Color);
                modelComponentBuilder.Row().CheckBox("Ignore Lighting", x => x.IgnoreLighting);
                modelComponentBuilder.Row().CheckBox("Dont cull", x => x.DontCull);
                modelComponentBuilder.Row().CheckBox("No Specular", x => x.NoSpecular);
                modelComponentBuilder.Row().CheckBox("Ortho", x => x.Ortho);
                modelComponentBuilder.Row().Button("Load", OnLoadTextureClicked);
                modelComponentBuilder.Row().Button("Export", OnExportClick);
            }
            {
                var sceneGroupBox = builder.Row().GroupBox("Scene", x => x.AmParent as IScene, x => x != null).Table();
                sceneGroupBox.Row().ColorPicker("Bgnd Color", x => x.BackgroundColor);
                var dict = new Dictionary<string, ISkybox>
                {
                    {"None", null},
                    {"Storm", embeddedResources.Skybox("Skyboxes/storm.skybox")},
                    {"Stars", embeddedResources.Skybox("Skyboxes/stars.skybox")}
                };
                sceneGroupBox.Row().DropDown(x => x.Skybox, dict);
            }
            {
                var storyNode = builder.Row().GroupBox("Story Node", x => x.SearchComponent<IStoryComponent>(), x => x != null).Table();
                storyNode.Row().CheckBox("Instant transition", x => x.InstantTransition);
                storyNode.Row().CheckBox("Skip", x => x.SkipOrder);
            }
            {
                var rectGroupBox = builder.Row().GroupBox("Rectangle", x => x, x => x.HasComponent<IRectangleComponent>()).Table();
                var colorRectPanel = rectGroupBox.Row().Panel(x => x.SearchComponent<ColorRectangleComponent>(), x => x != null);
                colorRectPanel.ColorPicker("Color", x => x.Color);
                var rectangleComponent = rectGroupBox.Row().Panel(x => x.SearchComponent<IRectangleComponent>(), x => x != null);
                rectangleComponent.CheckBox("DragBorders", x => x.DragByBorders);
            }
            {
                var components = builder.Row().GroupBox("Components", x => x, x => x != null);
                var componentsTable = components.Table();
                // todo: list
                var newComponentViewModel = new NewComponentViewModel(() => components.GetObject());
                var newComponentPanelBuilder = componentsTable.Row().Panel(x => newComponentViewModel, x => true).Table();
                newComponentPanelBuilder.Row().DropDown(x => x.ComponentType, new Dictionary<string, Type>
                {
                    ["RotateOnce"] = typeof(RotateOnceComponent),
                    ["Something"] = typeof(PresentationComponent),
                });
                newComponentPanelBuilder.Row().Button("Add", x =>
                {
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