using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
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
    public abstract class StoryFlowchartEdgeGizmoComponent : SceneNodeComponentBase<StoryFlowchartEdgeGizmoComponent>,
        IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly IVisualElement[] visualElements;
        private IRayHittable hittable;

        public IStandardMaterial Material { get; set; }
        public Vector3 FirstPoint { get; set; }
        public Vector3 MiddlePoint { get; set; }
        public Vector3 LastPoint { get; set; }

        protected StoryFlowchartEdgeGizmoComponent(IEmbeddedResources embeddedResources)
        {
            var lineModel = embeddedResources.LineModel();

            Material = new StandardMaterial(new SingleColorPixelSource(new Color4(0f, 0.5f, 0f)))
            {
                LineWidth = 3,
                IgnoreLighting = true,
            };

            visualElements = new IVisualElement[]
            {
                new CgModelVisualElement<StoryFlowchartEdgeGizmoComponent>(this)
                    .SetModel(lineModel)
                    .SetMaterial(x => x.Material)
                    .SetTransform(x => GetTransformForLine(x.FirstPoint, x.MiddlePoint)),
                new CgModelVisualElement<StoryFlowchartEdgeGizmoComponent>(this)
                    .SetModel(lineModel)
                    .SetMaterial(x => x.Material)
                    .SetTransform(x => GetTransformForLine(x.MiddlePoint, x.LastPoint))
            };
        }

        public override void AmOnAttached()
        {
            hittable = new SphereHittable<ISceneNode>(Node, x => new Sphere(x.GlobalTransform.Offset, 0.02f));
        }

        private static Transform GetTransformForLine(Vector3 point1, Vector3 point2)
        {
            var dir = point2 - point1;
            var scale = dir.Length();
            var rotation = Quaternion.RotationToFrame(dir, Vector3.UnitZ);
            var offset = point1 + 0.9f * Vector3.UnitZ;
            return new Transform(scale, rotation, offset);
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements()
        {
            return visualElements;
        }

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            return false;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}