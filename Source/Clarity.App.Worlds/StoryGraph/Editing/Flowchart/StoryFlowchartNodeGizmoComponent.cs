using System.Collections.Generic;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Worlds.StoryGraph.Editing.Flowchart
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

        private IImage GetThumbnail() => ReferencedNode.SearchComponent<IStoryComponent>()?.GetThumbnail();

        private Color4 DiffuseColorToUse() => UseThumbnail ? Color4.White : ColorByDepth(Depth);
        private IImage DiffuseMapToUse() => UseThumbnail ? GetThumbnail() : null;

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

            var material = StandardMaterial.New(this)
                .SetDiffuseColor(x => x.DiffuseColorToUse())
                .SetDiffuseMap(x => x.DiffuseMapToUse())
                .SetIgnoreLighting(true)
                .SetHighlightEffect(x => x.viewService.SelectedNode == ReferencedNode ? HighlightEffect.RainbowBorder : HighlightEffect.None);

            visualElement = new ModelVisualElement<StoryFlowchartNodeGizmoComponent>(this)
                .SetModel(squareModel)
                .SetMaterial(material)
                .SetTransform(x => Transform.Translation(new Vector3(x.GlobalRectangle.Center, x.Depth * 0.1f)))
                .SetNonUniformScale(x => new Vector3(x.GlobalRectangle.HalfWidth, -/*todo: correct text-coords*/x.GlobalRectangle.HalfHeight, 1));

            hittable = new RectangleHittable<StoryFlowchartNodeGizmoComponent>(this,
                Transform.Identity,
                x => new AaRectangle2(Vector2.Zero, x.GlobalRectangle.HalfWidth, x.GlobalRectangle.HalfHeight),
                x => -x.Depth);
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements() => visualElement.EnumSelf();
        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;

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