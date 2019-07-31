using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.External.SpherePacking;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.Media.Media3D;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree.MiscComponents;
using Clarity.Common.Infra.Files;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Utilities;
using Eto.Drawing;
using Eto.Forms;
using FontDecoration = Clarity.Engine.Media.Text.Rich.FontDecoration;

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

        public FluentGuiService(IViewService viewService, IRenderLoopDispatcher renderLoopDispatcher, IAssetService assetService, IUndoRedoService undoRedo, IEmbeddedResources embeddedResources)
        {
            this.viewService = viewService;
            this.assetService = assetService;
            this.undoRedo = undoRedo;

            var mainPanel = new FluentPanel<ISceneNode>(() => selectedSceneNode, x => x != null)
            {
                Width = 300
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
                var viewModel = new RichTextViewModel(() => selectedSceneNode.SearchComponent<IRichTextComponent>());
                var textGroupBox = builder.Row().GroupBox("Rich Text", x => viewModel, x => x.Valid).Table();
                var bgndModeRow = textGroupBox.Row();
                bgndModeRow.Label("Bgnd Mode");
                bgndModeRow.DropDown(x => x.BackgroundMode, new Dictionary<string, RtTransparencyMode>
                {
                    ["Opaque"] = RtTransparencyMode.Opaque,
                    ["Native"] = RtTransparencyMode.Native,
                    ["White is transparent"] = RtTransparencyMode.WhiteIsTransparent,
                    ["Black is transparent"] = RtTransparencyMode.BlackIsTransparent,
                });
                var bgndColorRow = textGroupBox.Row();
                bgndColorRow.Label("Bgnd Color");
                bgndColorRow.ColorPicker(x => x.BackgroundColor);
                var bgndOpacityRow = textGroupBox.Row();
                bgndOpacityRow.Label("Bgnd Opacity");
                bgndOpacityRow.Slider(x => x.BackgroundOpacity, 0, 1, 256);

                var alignmentRow = textGroupBox.Row();
                alignmentRow.Label("Alignment");
                alignmentRow.DropDown(x => x.Alignment, new Dictionary<string, RtParagraphAlignment>
                {
                    ["Left"] = RtParagraphAlignment.Left,
                    ["Center"] = RtParagraphAlignment.Center,
                    ["Right"] = RtParagraphAlignment.Right,
                    ["Justify"] = RtParagraphAlignment.Justify,
                });
                var directionRow = textGroupBox.Row();
                directionRow.Label("Direction");
                directionRow.DropDown(x => x.Direction, new Dictionary<string, RtParagraphDirection>
                {
                    ["LeftToRight"] = RtParagraphDirection.LeftToRight,
                    ["RightToLeft"] = RtParagraphDirection.RightToLeft,
                });
                var tabsRow = textGroupBox.Row();
                tabsRow.Label("Tabs");
                tabsRow.NumericUpDown(x => x.Tabs, 0, 16);
                var marginUpRow = textGroupBox.Row();
                marginUpRow.Label("MarginUp");
                marginUpRow.NumericUpDown(x => x.MarginUp, 0, 1000);

                var fontRow = textGroupBox.Row();
                fontRow.Label("Font");
                fontRow.DropDown(x => x.FontFamily, Fonts.AvailableFontFamilies
                    .Select(x => x.Name)
                    .OrderBy(r => r)
                    .ToDictionary(x => x));
                var sizeRow = textGroupBox.Row();
                sizeRow.Label("Size");
                sizeRow.NumericUpDown(x => x.Size, 1, 200);
                var colorRow = textGroupBox.Row();
                colorRow.Label("Color");
                colorRow.ColorPicker(x => x.Color);
                var decorationRow1 = textGroupBox.Row();
                decorationRow1.CheckBox("Bold", x => x.Bold);
                decorationRow1.CheckBox("Italic", x => x.Italic);
                var decorationRow2 = textGroupBox.Row();
                decorationRow2.CheckBox("Underline", x => x.Underline);
                decorationRow2.CheckBox("Strikethrough", x => x.Strikethrough);
                var highlightGroupRow = textGroupBox.Row();
                highlightGroupRow.Label("Highlight Group");
                highlightGroupRow.TextBox(x => x.HighlightGroup);

                var formulaRow = textGroupBox.Row();
                formulaRow.Button("Insert Formula", vm => vm.InsertFormula());
            }
            {
                var circlePackingGroupBox = builder.Row().GroupBox("Circle Packing", x => x.SearchComponent<ICirclePackingComponent>(), x => x != null).Table();
                var radiusRow = circlePackingGroupBox.Row();
                radiusRow.Label("Radius");
                radiusRow.TextBox(x => x.CircleRadius);
                var areaRow = circlePackingGroupBox.Row();
                areaRow.Label("Area");
                areaRow.Label(x => x.Area.ToString(CultureInfo.InvariantCulture));
                var numCirclesRow = circlePackingGroupBox.Row();
                numCirclesRow.Label(x => "Num Circles: " + x.CurrentNumCircles);
                numCirclesRow.Label(x => "Uppber Bound: " + x.MaxCircles);
                var resetRow = circlePackingGroupBox.Row();
                resetRow.Label("");
                resetRow.Button("Reset Circles", x => x.ResetPacker());
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
                        throw new ArgumentOutOfRangeException();
                    }
                });
            }
            builder.Row().Panel(x => x, x => true);

            rootControl = mainPanel;
            renderLoopDispatcher.AfterUpdate += Update;
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private class NewComponentViewModel
        {
            private readonly Func<ISceneNode> getNode;
            
            public Type ComponentType { get; set; }

            public NewComponentViewModel(Func<ISceneNode> getNode)
            {
                this.getNode = getNode;
            }

            public ISceneNode GetNode() => getNode();
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private class RichTextViewModel
        {
            private readonly Func<IRichTextComponent> getComponent;

            public bool Valid => getComponent() != null;

            public RichTextViewModel(Func<IRichTextComponent> getComponent)
            {
                this.getComponent = getComponent;
            }

            private IRichTextBox TextBox => getComponent()?.TextBox;
            private IRichText Text => getComponent()?.TextBox?.Text;

            #region Text Properties
            public RtTransparencyMode BackgroundMode
            {
                get => Text?.Style.TransparencyMode ?? RtTransparencyMode.Opaque;
                set
                {
                    if (Valid)
                        Text.Style.TransparencyMode = value;
                }
            }

            public Color3 BackgroundColor
            {
                get => Text?.Style.BackgroundColor.RGB ?? Color3.Black;
                set
                {
                    if (Valid)
                        Text.Style.BackgroundColor = new Color4(value, Text.Style.BackgroundColor.A);
                }
            }

            public float BackgroundOpacity
            {
                get => Text?.Style.BackgroundColor.A ?? 0;
                set
                {
                    if (Valid && Text.Style.TransparencyMode == RtTransparencyMode.Native)
                        Text.Style.BackgroundColor = new Color4(Text.Style.BackgroundColor.RGB, value);
                }
            }
            #endregion

            #region Paragraph Properties
            private T GetParagraphProperty<T>(Func<IRtParagraphStyle, T> getProp)
                where T : IEquatable<T>
            {
                var component = getComponent();
                return component != null && component.HeadlessEditor.TryGetParaStyleProp(getProp, out var prop) ? prop : default(T);
            }

            private void SetParagraphProperty<T>(T value, Action<IRtParagraphStyle, T> setProp)
            {
                getComponent()?.HeadlessEditor.SetParaStyleProp(value, setProp);
            }

            public RtParagraphAlignment Alignment
            {
                get => (RtParagraphAlignment)GetParagraphProperty(p => (int)p.Alignment);
                set => SetParagraphProperty(value, (p, v) => p.Alignment = v);
            }

            public RtParagraphDirection Direction
            {
                get => (RtParagraphDirection)GetParagraphProperty(p => (int)p.Direction);
                set => SetParagraphProperty(value, (p, v) => p.Direction = v);
            }

            public double Tabs
            {
                get => GetParagraphProperty(p => p.TabCount);
                set => SetParagraphProperty(value, (p, v) => p.TabCount = (int)Math.Round(v));
            }

            public double MarginUp
            {
                get => GetParagraphProperty(p => p.MarginUp);
                set => SetParagraphProperty(value, (p, v) => p.MarginUp = (int)Math.Round(v));
            }
            #endregion

            #region Span Properties
            private T GetSpanStyleProperty<T>(Func<IRtSpanStyle, T> getProp, bool firstSpanAsDefault = false)
                where T : IEquatable<T>
            {
                var component = getComponent();
                return component != null && component.HeadlessEditor.TryGetSpanStyleProp(getProp, out var prop) ? prop : default(T);
            }

            private T? GetSpanStylePropertyNullable<T>(Func<IRtSpanStyle, T> getProp, bool firstSpanAsDefault = false)
                where T : struct, IEquatable<T>
            {
                var component = getComponent();
                return component != null && component.HeadlessEditor.TryGetSpanStyleProp(getProp, out var prop) ? prop : default(T?);
            }

            private void SetSpanStyleProperty<T>(T value, Action<IRtSpanStyle, T> setProp)
            {
                getComponent()?.HeadlessEditor.SetSpanStyleProp(value, setProp);
            }

            private static void SetFontDecoration(FontDecoration flag, IRtSpanStyle spanStyle, bool value)
            {
                if (value)
                    spanStyle.FontDecoration |= flag;
                else
                    spanStyle.FontDecoration &= ~flag;
            }

            public string FontFamily
            {
                get => GetSpanStyleProperty(x => x.FontFamily);
                set => SetSpanStyleProperty(value, (s, v) => s.FontFamily = v);
            }

            public double Size
            {
                get => GetSpanStyleProperty(x => x.Size, true);
                set => SetSpanStyleProperty(value, (s, v) => s.Size = (float)v);
            }

            public Color4 Color
            {
                get => GetSpanStyleProperty(x => x.TextColor);
                set => SetSpanStyleProperty(value, (s, v) => s.TextColor = v);
            }

            public bool? Bold
            {
                get => GetSpanStylePropertyNullable(x => x.FontDecoration.HasFlags(FontDecoration.Bold));
                set => SetSpanStyleProperty(value, (s, v) => SetFontDecoration(FontDecoration.Bold, s, v ?? false));
            }

            public bool? Italic
            {
                get => GetSpanStylePropertyNullable(x => x.FontDecoration.HasFlags(FontDecoration.Italic));
                set => SetSpanStyleProperty(value, (s, v) => SetFontDecoration(FontDecoration.Italic, s, v ?? false));
            }

            public bool? Underline
            {
                get => GetSpanStylePropertyNullable(x => x.FontDecoration.HasFlags(FontDecoration.Underline));
                set => SetSpanStyleProperty(value, (s, v) => SetFontDecoration(FontDecoration.Underline, s, v ?? false));
            }

            public bool? Strikethrough
            {
                get => GetSpanStylePropertyNullable(x => x.FontDecoration.HasFlags(FontDecoration.Strikethrough));
                set => SetSpanStyleProperty(value, (s, v) => SetFontDecoration(FontDecoration.Strikethrough, s, v ?? false));
            }

            public string HighlightGroup
            {
                get => GetSpanStyleProperty(x => x.HighlightGroup ?? "");
                set => SetSpanStyleProperty(value, (s, v) => s.HighlightGroup = string.IsNullOrEmpty(v) ? null : v);
            }
            #endregion

            public void InsertFormula()
            {
                getComponent()?.HeadlessEditor.InsertEmbedding("latex", @"y=\frac{x^2}{2}+\alpha");
            }
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