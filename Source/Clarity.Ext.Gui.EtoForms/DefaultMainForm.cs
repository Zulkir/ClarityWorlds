using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.Media.Media3D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.Files;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Utilities;
using Clarity.Ext.Gui.EtoForms.AppModes;
using Clarity.Ext.Gui.EtoForms.Commands;
using Clarity.Ext.Gui.EtoForms.FluentGui;
using Clarity.Ext.Gui.EtoForms.Props;
using Clarity.Ext.Gui.EtoForms.ResourceExplorer;
using Clarity.Ext.Gui.EtoForms.SaveLoad;
using Clarity.Ext.Gui.EtoForms.SceneTree;
using Clarity.Ext.Gui.EtoForms.StoryGraph;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms
{
    public class DefaultMainForm : Form, IMainForm
    {
        private readonly RenderControl renderControl;
        private readonly UndoRedoGui undoRedoGui;
        private readonly Random random = new Random();
        private readonly IStoryGraphGui storyGraphGui;
        private bool isFullscreen;

        public Form Form => this;
        public RenderControl RenderControl => renderControl;

        public DefaultMainForm(IUndoRedoService undoRedoService, IToolFactory toolFactory, IToolService toolService,
            RenderControl renderControl, IAppModesCommands appModesCommands,
            ISaveLoadGuiCommands saveLoadGuiCommands, ISceneTreeGui sceneTreeGui, IPropsGui propsGui, IFluentGuiService fluentGuiService,
            IAmDiBasedObjectFactory objectFactory, IAssetService assetService, IEmbeddedResources embeddedResources,
            IResourceExplorerGui resourceExplorerGui, IReadOnlyList<IToolMenuItem> toolMenuItems, IStoryGraphGui storyGraphGui,
            IReadOnlyList<IAssetLoader> assetLoaders, IViewService viewService, ICommonGuiObjects commonGuiObjects,
            ISceneNodeContextMenuBuilder sceneNodeContextMenuBuilder)
        {
            this.storyGraphGui = storyGraphGui;
            ClientSize = new Size(1280, 720);
            Title = "Clarity Worlds";

            var assetOpenFileDialog = new OpenFileDialog();
            assetOpenFileDialog.Filters.Add(new FileDialogFilter("All Assets", assetLoaders.SelectMany(x => x.FileExtensions).Distinct().ToArray()));
            foreach (var assetLoader in assetLoaders)
                assetOpenFileDialog.Filters.Add(new FileDialogFilter(assetLoader.AssetTypeString,
                    assetLoader.FileExtensions.ToArray()));

            var toolCommands = new Command[]
            {
                new ToolCommand("Cube", toolService, () =>
                {
                    var entity = objectFactory.Create<SceneNode>();
                    entity.Name = "NewCube";
                    entity.Components.Add(PresentationComponent.Create());
                    var modelComponent = objectFactory.Create<ModelComponent>();
                    modelComponent.Model = embeddedResources.CubeModel();
                    modelComponent.Color = GetRandomSaturatedColor();
                    entity.Components.Add(modelComponent);
                    return toolFactory.MoveEntity(entity, true);
                }),
                new ToolCommand("Sphere", toolService, () =>
                {
                    var entity = objectFactory.Create<SceneNode>();
                    entity.Name = "NewSphere";
                    entity.Components.Add(PresentationComponent.Create());
                    var modelComponent = objectFactory.Create<ModelComponent>();
                    modelComponent.Model = embeddedResources.SphereModel(64);
                    modelComponent.Color = GetRandomSaturatedColor();
                    entity.Components.Add(modelComponent);
                    return toolFactory.MoveEntity(entity, true);
                }),
                new ToolCommand("Rectangle", toolService, toolFactory.AddRectangle),
                new ToolCommand("Text", toolService, toolFactory.AddText),
                new ToolCommand("Asset", toolService, () =>
                {
                    assetOpenFileDialog.ShowDialog(this);
                    var loadPath = assetOpenFileDialog.FileName;
                    if (loadPath == null)
                        return null;
                    var loadInfo = new AssetLoadInfo
                    {
                        FileSystem = new ActualFileSystem(),
                        LoadPath = loadPath,
                        ReferencePath = loadPath,
                        StorageType = AssetStorageType.CopyLocal
                    };
                    var assetLoadResult = assetService.Load(loadInfo);
                    if (!assetLoadResult.Successful)
                    {
                        MessageBox.Show(assetLoadResult.Message);
                        return null;
                    }
                    var asset = assetLoadResult.Asset;      
                    var resource = asset.Resource is ResourcePack pack ? pack.MainSubresource : asset.Resource;
                    switch (resource)
                    {
                        case IImage image:
                            return toolFactory.AddImage(image);
                        case IMovie movie:
                            return toolFactory.AddMovie(movie);
                        case IFlexibleModel fModel:
                            var entity = AmFactory.Create<SceneNode>();
                            entity.Name = "NewModel";
                            entity.Components.Add(PresentationComponent.Create());
                            var modelComponent = AmFactory.Create<ModelComponent>();
                            modelComponent.Model = fModel;
                            modelComponent.Color = GetRandomSaturatedColor();
                            entity.Components.Add(modelComponent);
                            return toolFactory.MoveEntity(entity, true);
                        default:
                            return null;
                    }
                })
            }
            .Concat(toolMenuItems.Select(x => new Command((s, e) => x.OnActivate()) {ToolBarText = x.Text, }))
            .ToArray();

            undoRedoGui = new UndoRedoGui(undoRedoService);

            CreateMenuBar(saveLoadGuiCommands, appModesCommands);
            CreateToolBar(toolCommands);

            var leftLayout = new TabControl();
            leftLayout.Pages.Add(new TabPage(sceneTreeGui.TreeView) { Text = "World"});
            leftLayout.Pages.Add(new TabPage(resourceExplorerGui.TreeView) { Text = "Resources" });
            //var leftLayout = new TableLayout();
            //leftLayout.Rows.Add(new TableRow(new TableCell(sceneTreeGui.TreeView)));

            this.renderControl = renderControl;
            renderControl.InitGraphics();
            //viewService.ChangeRenderingArea(renderControl);

            viewService.Update += (sender, args) =>
            {
                if (args.Type == ViewEventType.SelectedNodeChanged)
                {
                    var builder = new EtoMenuBuilder(commonGuiObjects.SelectionContextMenu);
                    if (viewService.SelectedNode != null)
                        sceneNodeContextMenuBuilder.Build(builder, viewService.SelectedNode);
                    else
                        commonGuiObjects.SelectionContextMenu.Items.Clear();
                }
            };

            var rightLayout = new TableLayout();
            rightLayout.Rows.Add(new TableRow(propsGui.PropsPanel, fluentGuiService.RootEtoControl));

            var layout = new TableLayout();
            layout.Rows.Add(
                new TableRow(
                    new TableCell(new TableLayout(new TableRow(new TableCell(leftLayout)))),
                    new TableCell(new TableLayout(new TableRow(new TableCell(renderControl))), true),
                    new TableCell(new TableLayout(new TableRow(new TableCell(rightLayout))))));

            Content = layout;
            
            renderControl.FullscreenEnded += OnFullscreenEnded;
        }

        private void CreateMenuBar(ISaveLoadGuiCommands saveLoadGuiCommands, IAppModesCommands appModesCommands)
        {
            var menu = new MenuBar();
            
            var fileMenuItem = new ButtonMenuItem { Text = "&File" };
            var quitCommand = new Command((s, e) => Application.Instance.Quit())
            {
                MenuText = "&Quit"
            };
            fileMenuItem.Items.Add(saveLoadGuiCommands.New);
            fileMenuItem.Items.Add(saveLoadGuiCommands.Open);
            fileMenuItem.Items.AddSeparator();
            fileMenuItem.Items.Add(saveLoadGuiCommands.Save);
            fileMenuItem.Items.Add(saveLoadGuiCommands.SaveAs);
            fileMenuItem.Items.AddSeparator();
            fileMenuItem.Items.Add(quitCommand);
            menu.Items.Add(fileMenuItem);
            
            var editMenuItem = new ButtonMenuItem { Text = "&Edit" };
            editMenuItem.Items.Add(undoRedoGui.UndoCommand);
            editMenuItem.Items.Add(undoRedoGui.RedoCommand);
            menu.Items.Add(editMenuItem);
            
            var viewMenu = new ButtonMenuItem { Text = "&View" };
            viewMenu.Items.Add(storyGraphGui.ViewStoryGraphCommand);
            menu.Items.Add(viewMenu);

            var presentationMenu = new ButtonMenuItem { Text = "&Presentation" };
            presentationMenu.Items.Add(appModesCommands.StartPresentation);
            presentationMenu.Items.Add(appModesCommands.StopPresentation);
            menu.Items.Add(presentationMenu);

            Menu = menu;
        }

        private void CreateToolBar(IEnumerable<Command> toolCommands)
        {
            var toolBar = new ToolBar();
            toolBar.Items.Add(undoRedoGui.UndoCommand);
            toolBar.Items.Add(undoRedoGui.RedoCommand);
            toolBar.Items.AddSeparator();
            toolBar.Items.Add(storyGraphGui.ViewStoryGraphCommand);
            toolBar.Items.AddSeparator();
            toolBar.Items.AddRange(toolCommands);
            var collectButton = new ButtonToolItem()
            {
                Text = "Collect",  
            };
            collectButton.Click += (s, a) => GC.Collect();
            toolBar.Items.Add(collectButton);
            ToolBar = toolBar;
        }

        private void OnFullscreenEnded()
        {
            isFullscreen = false;
            //appModeService.SetMode(AppMode.Editing);
        }

        private Color4 GetRandomSaturatedColor()
        {
            int type = random.Next(0, 5);
            switch (type)
            {
                default:
                case 0: return new Color4(0f, 1f, (float)random.NextDouble());
                case 1: return new Color4(0f, (float)random.NextDouble(), 1f);
                case 2: return new Color4((float)random.NextDouble(), 0f, 1f);
                case 3: return new Color4(1f, 0f, (float)random.NextDouble());
                case 4: return new Color4(1f, (float)random.NextDouble(), 0f);
                case 5: return new Color4((float)random.NextDouble(), 1f, 0f);
            }
        }
    }
}