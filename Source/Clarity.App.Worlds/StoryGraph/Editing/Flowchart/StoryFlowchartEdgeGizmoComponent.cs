using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.App.Worlds.StoryGraph.Editing.Flowchart
{
    public abstract class StoryFlowchartEdgeGizmoComponent : SceneNodeComponentBase<StoryFlowchartEdgeGizmoComponent>,
        IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        private readonly IVisualElement[] visualElements;
        private IRayHittable hittable;

        public IStandardMaterial Material { get; set; }
        public IStandardRenderState RenderState { get; set; }
        public Vector3 FirstPoint { get; set; }
        public Vector3 MiddlePoint { get; set; }
        public Vector3 LastPoint { get; set; }

        protected StoryFlowchartEdgeGizmoComponent(IEmbeddedResources embeddedResources)
        {
            var lineModel = embeddedResources.LineModel();

            Material = StandardMaterial.New()
                .SetDiffuseColor(new Color4(0f, 0.5f, 0f))
                .SetIgnoreLighting(true)
                .FromGlobalCache();

            RenderState = StandardRenderState.New()
                .SetLineWidth(3)
                .FromGlobalCache();

            visualElements = new IVisualElement[]
            {
                new ModelVisualElement<StoryFlowchartEdgeGizmoComponent>(this)
                    .SetModel(lineModel)
                    .SetMaterial(x => x.Material)
                    .SetTransform(x => GetTransformForLine(x.FirstPoint, x.MiddlePoint)),
                new ModelVisualElement<StoryFlowchartEdgeGizmoComponent>(this)
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

        public IEnumerable<IVisualEffect> GetVisualEffects() => EmptyArrays<IVisualEffect>.Array;

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return false;
        }

        public RayHitResult HitWithClick(RayCastInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}