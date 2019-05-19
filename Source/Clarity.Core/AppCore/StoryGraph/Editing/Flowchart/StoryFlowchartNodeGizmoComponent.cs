using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppCore.StoryGraph.Editing.Flowchart
{
    public abstract class StoryFlowchartNodeGizmoComponent : SceneNodeComponentBase<StoryFlowchartNodeGizmoComponent>,
        IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly IViewService viewService;

        private readonly IVisualElement visualElement;
        private readonly IRayHittable hittable;

        public ISceneNode ReferencedNode { get; set; }
        public AaRectangle2 GlobalRectangle { get; set; }
        public int Depth { get; set; }
        public bool UseThumbnail { get; set; }

        private readonly IPixelSource colorSource;

        private IImage GetThumbnail() => ReferencedNode.SearchComponent<IStoryComponent>()?.GetThumbnail();
        private IPixelSource TextureSourceToUse() => UseThumbnail ? GetThumbnail() : colorSource;

        private static Color4 ColorByDepth(int depth)
        {
            switch (depth % 6)
            {
                case 0: return new Color4(0.5f, 0, 0);
                case 1: return new Color4(0, 0.5f, 0);
                case 2: return new Color4(0, 0, 0.5f);
                case 3: return new Color4(0.5f, 0.5f, 0);
                case 4: return new Color4(0.5f, 0, 0.5f);
                case 5: return new Color4(0, 0.5f, 0.5f);
                default: return default(Color4);
            }
        }

        protected StoryFlowchartNodeGizmoComponent(IEmbeddedResources embeddedResources, IViewService viewService)
        {
            this.viewService = viewService;

            GlobalRectangle = new AaRectangle2(Vector2.Zero, 1, 1);

            var squareModel = embeddedResources.SimplePlaneXyModel();

            colorSource = new SingleColorPixelSourceProxy<StoryFlowchartNodeGizmoComponent>(this)
            {
                GetColor = x => ColorByDepth(x.Depth)
            };

            IMaterial material = new StandardMaterialProxy<StoryFlowchartNodeGizmoComponent>(this)
            {
                GetDiffuseTextureSource = x => x.TextureSourceToUse(),
                GetIgnoreLighting = x => true
            };

            visualElement = new CgModelVisualElement<StoryFlowchartNodeGizmoComponent>(this)
                .SetModel(squareModel)
                .SetMaterial(material)
                .SetTransform(x => Transform.Translation(new Vector3(x.GlobalRectangle.Center, x.Depth * 0.1f)))
                .SetNonUniformScale(x => new Vector3(x.GlobalRectangle.HalfWidth, -/*todo: correct text-coords*/x.GlobalRectangle.HalfHeight, 1))
                .SetHighlightEffect(x => x.viewService.SelectedNode == ReferencedNode ? CgHighlightEffect.RainbowBorder : CgHighlightEffect.None);

            hittable = new RectangleHittable<StoryFlowchartNodeGizmoComponent>(this,
                Transform.Identity,
                x => new AaRectangle2(Vector2.Zero, x.GlobalRectangle.HalfWidth, x.GlobalRectangle.HalfHeight),
                x => -x.Depth);
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements() => visualElement.EnumSelf();

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (!(args is IMouseEventArgs margs))
                return false;

            if (margs.IsLeftDoubleClickEvent() && margs.KeyModifyers == KeyModifyers.None)
            {
                viewService.MainView.FocusOn(ReferencedNode.GetComponent<IFocusNodeComponent>());
                return true;
            }

            if ((margs.IsLeftClickEvent() || margs.IsRightClickEvent()) && margs.KeyModifyers == KeyModifyers.None)
            {
                viewService.SelectedNode = ReferencedNode;
                return true;
            }

            return false;
        }

        // Hittable
        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}