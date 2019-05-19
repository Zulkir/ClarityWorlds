using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppCore.Interaction.RectangleManipulation
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
            var model = embeddedResources.CubeModel();
            var material = new StandardMaterial(new SingleColorPixelSource(Color4.White))
            {
                NoSpecular = true
            };
            visualElement = new CgModelVisualElement()
                .SetModel(model)
                .SetMaterial(material)
                .SetTransform(Transform.Scaling(0.025f));
            interactionElement = new ResizeRectangleInteractionElement<ResizeRectangleGizmoComponent>(
                this, x => x.GetRectAspect(), x => x.GetChildSpace(), x => x.Place, inputHandler, undoRedo);
            hittable = new SphereHittable<ResizeRectangleGizmoComponent>(this, x =>
            {
                var globalTransform = Node.GlobalTransform;
                return new Sphere(globalTransform.Offset, 0.025f * globalTransform.Scale);
            });
        }

        private ISceneNode GetGizmoRootNode() => Node?.ParentNode;
        private ISceneNode GetRectNode() => GetGizmoRootNode()?.ParentNode;
        private IRectangleComponent GetRectAspect() => GetRectNode()?.SearchComponent<IRectangleComponent>();
        private AaRectangle2 GetRectangle() => GetRectAspect()?.Rectangle ?? DefaultRect;
        private IPlacementPlane GetChildSpace()
        {
            var rectAspectNode = GetRectNode();
            return rectAspectNode?.ParentNode?.GetComponent<IPlacementPlaneComponent>().PlacementPlane;
        }

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

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEventArgs args) => interactionElement.TryHandleInteractionEvent(args);

        // Hittable
        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}