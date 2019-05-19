using System;
using System.Collections.Generic;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.AppModes;
using Clarity.Core.AppCore.Interaction;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppFeatures.Text
{
    public abstract class RichTextComponent : SceneNodeComponentBase<RichTextComponent>, IRichTextComponent,
        IVisualComponent, IInteractionComponent, IRayHittableComponent,
        IRichTextPixelSource
    {
        private readonly IEmbeddedResources embeddedResources;
        private readonly Lazy<IAppModeService> appModeServiceLazy;
        private readonly IViewService viewService;

        public abstract IRichTextBox TextBox { get; set; }

        public Vector2[] BorderCurve { get => TextBox.BorderCurve; set => TextBox.BorderCurve = value; }
        public RtPosition CursorPosition { get; set; }
        public RtPosition? SelectionStart { get; set; }
        public RtRange? SelectionRange => SelectionStart.HasValue ? new RtRange(SelectionStart.Value, CursorPosition) : (RtRange?)null;
        public IRtSpanStyle InputTextStyle { get; set; }

        private readonly IMaterial selectionRectMaterial;
        private readonly IInteractionElement editInteractionElement;
        private readonly IVisualElement rectVisualElement;
        private readonly IVisualElement cursorVisualElement;
        private readonly List<AaRectangle2> selectionRectangles;
        private readonly List<IVisualElement> selectionVisualElements;
        // todo: remove after rayhittable can passthrough
        private readonly IRayHittable hittable;

        public IList<Vector2> VisualBorderCurve { get; set; }
        public bool BorderComplete { get; set; }

        public bool HasTransparency => TextBox.Text.Style.HasTransparency;

        protected RichTextComponent(IEmbeddedResources embeddedResources, Lazy<IAppModeService> appModeServiceLazy, 
            IUndoRedoService undoRedo, IInputHandler inputHandler, IViewService viewService, IClipboardService clipboardService) 
        {
            this.embeddedResources = embeddedResources;
            this.appModeServiceLazy = appModeServiceLazy;
            this.viewService = viewService;
            TextBox = AmFactory.Create<RichTextBox>();
            var rectMaterial = new StandardMaterial(this)
            {
                IgnoreLighting = true
            };
            var rectModel = embeddedResources.SimplePlaneXyModel();
            rectVisualElement = new CgModelVisualElement<RichTextComponent>(this)
                .SetModel(rectModel)
                .SetMaterial(rectMaterial)
                .SetTransform(x => Transform.Translation(new Vector3(x.Node.GetComponent<IRectangleComponent>().Rectangle.Center, 0)))
                .SetNonUniformScale(x =>
                {
                    var rect = x.GetRect();
                    return new Vector3(rect.HalfWidth, rect.HalfHeight, 1);
                });
            var lineModel = embeddedResources.LineModel();
            cursorVisualElement = new CgModelVisualElement<RichTextComponent>(this)
                .SetModel(lineModel)
                .SetMaterial(new StandardMaterial(new SingleColorPixelSource(Color4.Black))
                {
                    IgnoreLighting = true,
                    LineWidth = 2
                })
                .SetTransform(x =>
                {
                    var rect = x.GetRect();
                    var textBox = x.TextBox;
                    textBox.Layout.GetCursorPoint(x.CursorPosition, out var point, out var height);
                    var nodeCoordPoint = rect.MinMax + new Vector2(point.X, -point.Y) / textBox.PixelScaling;
                    return new Transform(
                        height / textBox.PixelScaling, 
                        Quaternion.RotationZ(-MathHelper.PiOver2), 
                        new Vector3(nodeCoordPoint, 0));
                })
                .SetZOffset(GraphicsHelper.MinZOffset * 2);
            selectionRectMaterial = new StandardMaterial(new SingleColorPixelSource(new Color4(0.5f, 0.5f, 0.5f, 0.5f)))
            {
                IgnoreLighting = true,
            };
            selectionRectangles = new List<AaRectangle2>();
            selectionVisualElements = new List<IVisualElement>();
            editInteractionElement = new RichTextEditInteractionElement(this, inputHandler, undoRedo, clipboardService);
            hittable = new RectangleHittable<RichTextComponent>(this, 
                Transform.Identity, c => c.Node.GetComponent<IRectangleComponent>().Rectangle, c => GraphicsHelper.MinZOffset * 2);
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
        public RayHitResult HitWithClick(RayHitInfo clickInfo)
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
                if (SelectionRange.HasValue)
                    selectionRectangles.AddRange(TextBox.Layout.GetSelectionRectangles(SelectionRange.Value));
                while (selectionVisualElements.Count < selectionRectangles.Count)
                {
                    var index = selectionVisualElements.Count;
                    var elem = new CgModelVisualElement<RichTextComponent>(this)
                        .SetHide(x => index >= x.selectionRectangles.Count)
                        .SetModel(embeddedResources.SimplePlaneXyModel())
                        .SetMaterial(selectionRectMaterial)
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
                        })
                        .SetZOffset(GraphicsHelper.MinZOffset);
                    selectionVisualElements.Add(elem);
                }
                for (var i = 0; i < selectionRectangles.Count; i++)
                    yield return selectionVisualElements[i];
            }
        }

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            return appModeServiceLazy.Value.Mode == AppMode.Editing && 
                   editInteractionElement.TryHandleInteractionEvent(args);
        }

        private AaRectangle2 GetRect()
        {
            return Node.SearchComponent<IRectangleComponent>()?.Rectangle ?? new AaRectangle2(Vector2.Zero, 1, 1);
        }

        
    }
}