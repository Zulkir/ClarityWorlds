using System;
using System.Collections.Generic;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Worlds.Interaction.RectangleManipulation
{
    public abstract class ResizeRectangleGizmoComponent : SceneNodeComponentBase<ResizeRectangleGizmoComponent>,
        IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        private static readonly AaRectangle2 DefaultRect = new AaRectangle2(Vector2.Zero, 0.5f, 0.5f);

        public ResizeRectangleGizmoPlace Place { get; set; }
        
        private readonly IRayHittable hittable;

        private readonly IVisualElement visualElement;
        private readonly IInteractionElement interactionElement;

        protected ResizeRectangleGizmoComponent(IInputHandler inputHandler, IEmbeddedResources embeddedResources, IUndoRedoService undoRedo)
        {
            visualElement = ModelVisualElement.New()
                .SetModel(embeddedResources.CubeModel())
                .SetMaterial(StandardMaterial.New()
                    .SetNoSpecular(true)
                    .FromGlobalCache())
                .SetTransform(Transform.Scaling(0.025f));
            interactionElement = new ResizeRectangleInteractionElement<ResizeRectangleGizmoComponent>(
                this, x => x.GetRectAspect(), x => x.GetChildSpace(), x => x.Place, inputHandler, undoRedo);
            hittable = new SphereHittable<ResizeRectangleGizmoComponent>(this, x =>
            {
                var globalTransform = Node.GlobalTransform;
                return new Sphere(globalTransform.Offset, 0.025f * globalTransform.Scale);
            });
        }

        private EditRectangleGizmoComponent GetGizmoRootComponent() => Node.ParentNode.GetComponent<EditRectangleGizmoComponent>();
        private ISceneNode GetRectNode() => GetGizmoRootComponent().RectangleNode;
        private IRectangleComponent GetRectAspect() => GetRectNode()?.SearchComponent<IRectangleComponent>();
        private AaRectangle2 GetRectangle() => GetRectAspect()?.Rectangle ?? DefaultRect;
        private IPlacementSurface GetChildSpace() => GetRectNode()?.ParentNode?.PresentationInfra().Placement.PlacementSurface2D;

        private Vector3 GetPosition()
        {
            var rect = GetRectangle();
            switch (Place)
            {
                case ResizeRectangleGizmoPlace.Left:
                    return new Vector3(rect.MinX, rect.Center.Y, 0);
                case ResizeRectangleGizmoPlace.Right:
                    return new Vector3(rect.MaxX, rect.Center.Y, 0);
                case ResizeRectangleGizmoPlace.Bottom:
                    return new Vector3(rect.Center.X, rect.MinY, 0);
                case ResizeRectangleGizmoPlace.Top:
                    return new Vector3(rect.Center.X, rect.MaxY, 0);
                case ResizeRectangleGizmoPlace.TopLeft:
                    return new Vector3(rect.MinX, rect.MaxY, 0);
                case ResizeRectangleGizmoPlace.TopRight:
                    return new Vector3(rect.MaxX, rect.MaxY, 0);
                case ResizeRectangleGizmoPlace.BottomLeft:
                    return new Vector3(rect.MinX, rect.MinY, 0);
                case ResizeRectangleGizmoPlace.BottomRight:
                    return new Vector3(rect.MaxX, rect.MinY, 0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Update(FrameTime frameTime)
        {
            Node.Transform = Transform.Translation(GetPosition());
        }
        
        // Visual
        public IEnumerable<IVisualElement> GetVisualElements() => visualElement.EnumSelf();

        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEvent args) => interactionElement.TryHandleInteractionEvent(args);

        // Hittable
        public RayHitResult HitWithClick(RayCastInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}