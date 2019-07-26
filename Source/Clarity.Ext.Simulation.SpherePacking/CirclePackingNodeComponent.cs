using System.Collections.Generic;
using Clarity.App.Worlds.Interaction.Manipulation3D;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;

namespace Clarity.Ext.Simulation.SpherePacking 
{
    public abstract class CirclePackingNodeComponent : SceneNodeComponentBase<CirclePackingNodeComponent>,
        ITransformable3DComponent, IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        public abstract float CircleRadius { get; set; }

        protected CirclePackingNodeComponent()
        {

        }

        public Sphere LocalBoundingSphere { get; }
        public IEnumerable<IVisualElement> GetVisualElements() { throw new System.NotImplementedException(); }
        public IEnumerable<IVisualEffect> GetVisualEffects() { throw new System.NotImplementedException(); }
        public bool TryHandleInteractionEvent(IInteractionEvent args) { throw new System.NotImplementedException(); }
        public RayHitResult HitWithClick(RayHitInfo clickInfo) { throw new System.NotImplementedException(); }
    }
}