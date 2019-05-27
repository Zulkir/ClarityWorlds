using System;
using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.App.Worlds.Interaction.Manipulation3D
{
    public abstract class DragAlongAxisGizmoComponent : SceneNodeComponentBase<DragAlongAxisGizmoComponent>, IVisualComponent, IRayHittableComponent
    {
        private const float PixelLineWidth = 3f;

        private readonly IModel3D model;
        private readonly IVisualElement visualElement;
        private Axis3D axis;
        private IStandardMaterial material;
        private IStandardRenderState renderState;
        private Transform transform;

        private readonly IRayHittable hittable;

        public DragAlongAxisGizmoComponent(IEmbeddedResources embeddedResources)
        {
            model = embeddedResources.LineModel();
            visualElement = new ModelVisualElement<DragAlongAxisGizmoComponent>(this)
                .SetModel(model)
                .SetMaterial(x => x.material)
                .SetTransform(x => x.transform);
            hittable = new LineHittable<DragAlongAxisGizmoComponent>(this, x => x.GetGlobalLine(), PixelLineWidth);
        }

        public Axis3D Axis
        {
            get => axis;
            set
            {
                Color4 color;
                switch (value)
                {
                    case Axis3D.X:
                        color = Color4.Red;
                        transform = Transform.Rotate(Quaternion.RotationToFrame(Vector3.UnitX, Vector3.UnitY));
                        break;
                    case Axis3D.Y:
                        color = Color4.Green;
                        transform = Transform.Rotate(Quaternion.RotationToFrame(Vector3.UnitY, Vector3.UnitZ));
                        break;
                    case Axis3D.Z:
                        color = Color4.Blue;
                        transform = Transform.Rotate(Quaternion.RotationToFrame(Vector3.UnitZ, Vector3.UnitX));
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
                material = StandardMaterial.New()
                    .SetDiffuseColor(color)
                    .SetIgnoreLighting(true)
                    .FromGlobalCache();
                renderState = StandardRenderState.New()
                    .SetLineWidth(PixelLineWidth)
                    .FromGlobalCache();
                axis = value;
            }
        }

        private Line3 GetGlobalLine()
        {
            var globalTransform = Node.GlobalTransform;
            var point = globalTransform.Offset;
            switch (Axis)
            {
                case Axis3D.X: return new Line3(point, Vector3.UnitX * globalTransform);
                case Axis3D.Y: return new Line3(point, Vector3.UnitX * globalTransform);
                case Axis3D.Z: return new Line3(point, Vector3.UnitX * globalTransform);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public IEnumerable<IVisualElement> GetVisualElements()
        {
            return visualElement.EnumSelf();
        }

        public IEnumerable<IVisualEffect> GetVisualEffects()
        {
            yield break;
        }

        public RayHitResult HitWithClick(RayHitInfo clickInfo)
        {
            return hittable.HitWithClick(clickInfo);
        }
    }
}