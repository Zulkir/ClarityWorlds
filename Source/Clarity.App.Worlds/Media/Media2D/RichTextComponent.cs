using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.CopyPaste;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Misc.HighlightOnMouse;
using Clarity.App.Worlds.SaveLoad.ReadOnly;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Media.Media2D
{
    public abstract class RichTextComponent : SceneNodeComponentBase<RichTextComponent>, IRichTextComponent,
        IVisualComponent, IInteractionComponent, IRayHittableComponent, ICopyPasteComponent, IReadOnlyOverrideComponent
    {
        private readonly IEmbeddedResources embeddedResources;
        private readonly Lazy<IAppModeService> appModeServiceLazy;
        private readonly IViewService viewService;
        private readonly IRtImageBuilder imageBuilder;
        private readonly IHighlightOnMouseService highlightOnMouseService;

        public abstract IRichTextBox TextBox { get; set; }

        public Vector2[] BorderCurve { get => TextBox.BorderCurve; set => TextBox.BorderCurve = value; }
        public IRichTextHeadlessEditor HeadlessEditor => editInteractionElement.HeadlessEditor;

        private readonly IMaterial selectionRectMaterial;
        private readonly IMaterial highlightRectMaterial;
        private readonly RichTextEditInteractionElement editInteractionElement;
        private readonly IVisualElement rectVisualElement;
        private readonly IVisualElement cursorVisualElement;
        private readonly List<AaRectangle2> selectionRectangles;
        private readonly List<AaRectangle2> highlightRectangles;
        private readonly List<IVisualElement> selectionVisualElements;
        private readonly List<IVisualElement> highlightVisualElements;

        public IList<Vector2> VisualBorderCurve { get; set; }
        public bool BorderComplete { get; set; }

        private RawImage image;
        private bool imageIsDirty = true;

        private string lastHighlightGroup;

        protected RichTextComponent(IEmbeddedResources embeddedResources, Lazy<IAppModeService> appModeServiceLazy, 
            IUndoRedoService undoRedo, IInputHandler inputHandler, IViewService viewService, IClipboard clipboard, IRtImageBuilder imageBuilder, 
            IHighlightOnMouseService highlightOnMouseService) 
        {
            this.embeddedResources = embeddedResources;
            this.appModeServiceLazy = appModeServiceLazy;
            this.viewService = viewService;
            this.imageBuilder = imageBuilder;
            this.highlightOnMouseService = highlightOnMouseService;
            TextBox = AmFactory.Create<RichTextBox>();
            var rectMaterial = StandardMaterial.New(this)
                .SetDiffuseMap(x => x.GetTextImage())
                .SetIgnoreLighting(true)
                .SetRtTransparencyMode(x => x.TextBox.Text.Style.TransparencyMode);
            var rectModel = embeddedResources.SimplePlaneXyModel();
            rectVisualElement = new ModelVisualElement<RichTextComponent>(this)
                .SetModel(rectModel)
                .SetMaterial(rectMaterial)
                .SetTransform(x => Transform.Translation(new Vector3(x.Node.GetComponent<IRectangleComponent>().Rectangle.Center, 0)))
                .SetNonUniformScale(x =>
                {
                    var rect = x.GetRect();
                    return new Vector3(rect.HalfWidth, rect.HalfHeight, 1);
                });
            var lineModel = embeddedResources.LineModel();
            cursorVisualElement = new ModelVisualElement<RichTextComponent>(this)
                .SetModel(lineModel)
                .SetMaterial(StandardMaterial.New()
                    .SetDiffuseColor(Color4.Black)
                    .SetIgnoreLighting(true)
                    .FromGlobalCache())
                .SetRenderState(StandardRenderState.New()
                    .SetLineWidth(2)
                    .SetZOffset(GraphicsHelper.MinZOffset * 2)
                    .FromGlobalCache())
                .SetTransform(x =>
                {
                    var rect = x.GetRect();
                    var textBox = x.TextBox;
                    textBox.Layout.GetCursorPoint(x.editInteractionElement.HeadlessEditor.CursorPos, out var point, out var height);
                    var nodeCoordPoint = rect.MinMax + new Vector2(point.X, -point.Y) / textBox.PixelScaling;
                    return new Transform(
                        height / textBox.PixelScaling, 
                        Quaternion.RotationZ(-MathHelper.PiOver2), 
                        new Vector3(nodeCoordPoint, 0));
                });
            selectionRectMaterial = StandardMaterial.New()
                .SetDiffuseColor(new Color4(0.5f, 0.5f, 0.5f, 0.5f))
                .SetIgnoreLighting(true)
                .FromGlobalCache();
            highlightRectMaterial = StandardMaterial.New()
                .SetDiffuseColor(new Color4(1f, 0.5f, 0, 0.5f))
                .SetIgnoreLighting(true)
                .FromGlobalCache();
            selectionRectangles = new List<AaRectangle2>();
            highlightRectangles = new List<AaRectangle2>();
            selectionVisualElements = new List<IVisualElement>();
            highlightVisualElements = new List<IVisualElement>();
            editInteractionElement = new RichTextEditInteractionElement(this, inputHandler, undoRedo, clipboard);
        }

        public override void Update(FrameTime frameTime)
        {
            // todo: refactor to events (with smart routing or something)
            var rectangleAspect = Node.SearchComponent<IRectangleComponent>();
            if (rectangleAspect != null)
            {
                var rect = rectangleAspect.Rectangle;
                var width = Math.Max((int)(rect.Width * TextBox.PixelScaling), 1);
                var height = Math.Max((int)(rect.Height * TextBox.PixelScaling), 1);
                if (TextBox.Size.Width != width || TextBox.Size.Height != height)
                {
                    TextBox.Size = new IntSize2(width, height);
                }
            }
            base.Update(frameTime);
        }

        // Hittable
        public RayHitResult HitWithClick(RayCastInfo clickInfo)
        {
            return RayHitResult.Failure();
            //return hittable.HitWithClick(clickInfo);
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements()
        {
            yield return rectVisualElement;

            if (viewService.SelectedNode == Node)
            {
                yield return cursorVisualElement;

                selectionRectangles.Clear();
                if (HeadlessEditor.SelectionRange.HasValue)
                    selectionRectangles.AddRange(TextBox.Layout.GetSelectionRectangles(HeadlessEditor.SelectionRange.Value));
                while (selectionVisualElements.Count < selectionRectangles.Count)
                {
                    var index = selectionVisualElements.Count;
                    var elem = new ModelVisualElement<RichTextComponent>(this)
                        .SetHide(x => index >= x.selectionRectangles.Count)
                        .SetModel(embeddedResources.SimplePlaneXyModel())
                        .SetMaterial(selectionRectMaterial)
                        .SetRenderState(StandardRenderState.New()
                            .SetZOffset(GraphicsHelper.MinZOffset)
                            .FromGlobalCache())
                        .SetTransform(x =>
                        {
                            var rect = x.GetRect();
                            var textBox = x.TextBox;
                            var selectionRect = x.selectionRectangles[index];
                            var point = selectionRect.Center;
                            var nodeCoordPoint = rect.MinMax + new Vector2(point.X, -point.Y) / textBox.PixelScaling;
                            return Transform.Translation(new Vector3(nodeCoordPoint, 0));
                        })
                        .SetNonUniformScale(x =>
                        {
                            var textBox = x.TextBox;
                            var selectionRect = x.selectionRectangles[index];
                            return new Vector3(selectionRect.HalfWidth / textBox.PixelScaling, selectionRect.HalfHeight / textBox.PixelScaling, 1);
                        });
                    selectionVisualElements.Add(elem);
                }
                for (var i = 0; i < selectionRectangles.Count; i++)
                    yield return selectionVisualElements[i];
            }

            highlightRectangles.Clear();
            highlightRectangles.AddRange(TextBox.Layout.LayoutSpans
                .Where(x => x.Style.HighlightGroup != null && highlightOnMouseService.GroupIsHighlighted(x.Style.HighlightGroup))
                .Select(x => x.Bounds));
            while (highlightVisualElements.Count < highlightRectangles.Count)
            {
                var index = highlightVisualElements.Count;
                if (highlightVisualElements.Count <= index)
                {
                    var elem = ModelVisualElement.New(this)
                        .SetHide(x => index >= x.highlightRectangles.Count)
                        .SetModel(embeddedResources.SimplePlaneXyModel())
                        .SetMaterial(highlightRectMaterial)
                        .SetRenderState(StandardRenderState.New()
                            .SetZOffset(2 * GraphicsHelper.MinZOffset)
                            .FromGlobalCache())
                        .SetTransform(x =>
                        {
                            var rect = x.GetRect();
                            var textBox = x.TextBox;
                            var selectionRect = x.highlightRectangles[index];
                            var point = selectionRect.Center;
                            var nodeCoordPoint = rect.MinMax + new Vector2(point.X, -point.Y) / textBox.PixelScaling;
                            return Transform.Translation(new Vector3(nodeCoordPoint, 0));
                        })
                        .SetNonUniformScale(x =>
                        {
                            var textBox = x.TextBox;
                            var selectionRect = x.highlightRectangles[index];
                            return new Vector3(selectionRect.HalfWidth / textBox.PixelScaling, selectionRect.HalfHeight / textBox.PixelScaling, 1);
                        });
                    highlightVisualElements.Add(elem);
                }
            }
            for (var i = 0; i < highlightRectangles.Count; i++)
                yield return highlightVisualElements[i];
        }

        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;

        private IImage GetTextImage()
        {
            if (!imageIsDirty)
                return image;
            var imageRgba = imageBuilder.BuildImageRgba(TextBox);
            var hasTransparency = TextBox.Text.Style.HasTransparency;
            if (image == null || image.Size != TextBox.Size)
            {
                image?.Dispose();
                image = new RawImage(ResourceVolatility.Stable, TextBox.Size, hasTransparency, imageRgba);
            }
            else
                image.SetData(imageRgba, hasTransparency);
            imageIsDirty = false;
            return image;
        }

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return appModeServiceLazy.Value.Mode == AppMode.Editing && 
                   editInteractionElement.TryHandleInteractionEvent(args);
        }

        // todo: implement mouse-in / mouse-out interaction events instead
        public override void OnRoutedEvent(IRoutedEvent evnt)
        {
            if (!(evnt is IMouseEvent mevent))
                return;
            var newHighlightGroup = SearchHighlightGroup(mevent);
            if (newHighlightGroup == lastHighlightGroup)
                return;
            if (lastHighlightGroup != null)
                highlightOnMouseService.OnObjectOut(this, lastHighlightGroup);
            if (newHighlightGroup != null)
                highlightOnMouseService.OnObjectIn(this, newHighlightGroup);
            lastHighlightGroup = newHighlightGroup;
        }

        private string SearchHighlightGroup(IMouseEvent mevent)
        {
            var layer = mevent.Viewport.View.Layers.FirstOrDefault(x => x.VisibleScene == Node.Scene);
            if (layer == null)
                return null;
        
            // todo: refactor for better Rectangle modification code
            var placementSurface = Node.PresentationInfra().Placement;
            if (placementSurface == null)
                return null;
            var globalRay = mevent.Viewport.GetGlobalRayForPixelPos(mevent.State.Position);
            if (!placementSurface.PlacementSurface2D.TryFindPoint2D(globalRay, out var point2D))
                return null;
            var cRect = Node.GetComponent<IRectangleComponent>();
            var rect = cRect.Rectangle;
            var textBoxPointYswapped = (point2D - rect.MinMax) * TextBox.PixelScaling;
            var textBoxPoint = new Vector2(textBoxPointYswapped.X, -textBoxPointYswapped.Y);

            var pos = TextBox.Layout.GetPosition(textBoxPoint);
            var spanStyle = TextBox.Layout.GetSpanStyleAt(pos);
            return spanStyle.HighlightGroup;
        }

        private AaRectangle2 GetRect()
        {
            return Node.SearchComponent<IRectangleComponent>()?.Rectangle ?? new AaRectangle2(Vector2.Zero, 1, 1);
        }

        // Copy Paste
        public bool CanExecute(CopyPasteCommand command)
        {
            switch (command)
            {
                case CopyPasteCommand.Cut:
                case CopyPasteCommand.Copy:
                    return editInteractionElement.CanCopy();
                case CopyPasteCommand.Paste:
                    return editInteractionElement.CanPaste();
                case CopyPasteCommand.Duplicate:
                case CopyPasteCommand.Delete:
                case CopyPasteCommand.MoveTop:
                case CopyPasteCommand.MoveUp:
                case CopyPasteCommand.MoveDown:
                case CopyPasteCommand.MoveBottom:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }

        public void Execute(CopyPasteCommand command)
        {
            switch (command)
            {
                case CopyPasteCommand.Cut:
                    editInteractionElement.Cut();
                    break;
                case CopyPasteCommand.Copy:
                    editInteractionElement.Copy();
                    break;
                case CopyPasteCommand.Paste:
                    editInteractionElement.Paste();
                    break;
                case CopyPasteCommand.Duplicate:
                case CopyPasteCommand.Delete:
                case CopyPasteCommand.MoveTop:
                case CopyPasteCommand.MoveUp:
                case CopyPasteCommand.MoveDown:
                case CopyPasteCommand.MoveBottom:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }
        }

        // Read-Only Override
        public IEnumerable<ISceneNodeComponent> ToReadOnlyComponents()
        {
            var cImage = AmFactory.Create<ImageRectangleComponent>();
            cImage.Image = GetTextImage().WithSource(x => new GeneratedResourceSource(x, typeof(IImage)));
            yield return cImage;
        }

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            if (message.Object == TextBox || message.Object.AmIsDescendantOf(TextBox))
                imageIsDirty = true;
            base.AmOnChildEvent(message);
        }
    }
}