using System;
using System.Collections.Generic;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppCore.StoryGraph
{
    public class OrbitStoryLayout : IStoryLayout
    {
        public string UserFriendlyName => "Orbit";
        public Type Type => typeof(OrbitStoryLayout);

        private readonly IWorldTreeService worldTreeService;

        private readonly IFlexibleModel frustumModel;
        private readonly IFlexibleModel circleModel;

        private readonly IStandardMaterial frustumMaterial;
        private readonly IStandardMaterial circleMaterial;

        public OrbitStoryLayout(IEmbeddedResources embeddedResources, IWorldTreeService worldTreeService)
        {
            this.worldTreeService = worldTreeService;
            frustumModel = embeddedResources.SimpleFrustumModel();
            circleModel = embeddedResources.CircleModel(256);
            frustumMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Green))
            {
                IgnoreLighting = true,
            };
            circleMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Yellow))
            {
                IgnoreLighting = true,
            };
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            ArrangeAndDecorateLevel(sg, sg.Root, sg.Depth, true);
            return new BasicStoryLayoutInstance(worldTreeService);
        }

        private void ArrangeAndDecorateLevel(IStoryGraph sg, int nodeIndex, int scaleLevel, bool isRoot)
        {
            var aStory = sg.Aspects[nodeIndex];
            var node = sg.NodeObjects[nodeIndex];
            var abstractChildren = sg.Children[nodeIndex];

            var scale = MathHelper.Pow(2, scaleLevel / 4f);

            var halfSize = 1f;
            var fov = MathHelper.PiOver4;
            var distance = halfSize / MathHelper.Tan(fov / 2);

            var visuals = new List<IVisualElement>();
            
            visuals.Add(new CgModelVisualElement().SetModel(frustumModel).SetMaterial(frustumMaterial));
            
            var viewpointProps = isRoot
                ?   new TargetedControlledCameraY.Props
                    {
                        Target = Vector3.Zero,
                        Distance = distance * 10,
                        Pitch = MathHelper.Pi * 0.1f,
                        FieldOfView = fov,
                        ZNear = 0.1f * distance * scale,
                        ZFar = 100.0f * distance * scale
                    }
                :   new TargetedControlledCameraY.Props
                    {
                        Target = Vector3.Zero,
                        Distance = distance,
                        FieldOfView = fov,
                        ZNear = 0.1f * distance * scale,
                        ZFar = 100.0f * distance * scale
                    };
            
            if (isRoot)
            {
                // todo: change into additional local transform, or move to a 3D child space
                node.Transform = new Transform(1, Quaternion.RotationToFrame(Vector3.UnitX, Vector3.UnitZ), Vector3.Zero);
            }
            
            for (int i = 0; i < abstractChildren.Count; i++)
            {
                var adaptiveChild = abstractChildren[i];
                var childBoundingRadius = 5 * scale;
                var angle = MathHelper.Pi * 0.33f + 3 * MathHelper.TwoPi / 5 * i;
                var rotation = Quaternion.RotationY(-angle + MathHelper.Pi);
                var childScalingCompoensation = 10f;
                var radius = isRoot 
                    ? (childBoundingRadius / 3) * (2 + i) * childScalingCompoensation
                    : (childBoundingRadius / 3) * (2 + i * 0.5f) * childScalingCompoensation * 0.5f;

                var offset = new Vector3(radius * MathHelper.Sin(angle), 0, radius * MathHelper.Cos(angle));
                sg.NodeObjects[adaptiveChild].Transform = new Transform(5, rotation, offset);
                ArrangeAndDecorateLevel(sg, adaptiveChild, scaleLevel - 1, false);

                visuals.Add(new CgModelVisualElement()
                    .SetModel(circleModel)
                    .SetMaterial(circleMaterial)
                    .SetTransform(new Transform(radius / childScalingCompoensation, Quaternion.RotationToFrame(Vector3.UnitX, Vector3.UnitZ), Vector3.Zero)));
            }

            aStory.SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new WallDefaultViewpointMechanism(node, viewpointProps),
                Hittable = GetHittableComponent(node),
                VisualElements = visuals
            });
        }

        private static IRayHittable GetHittableComponent(ISceneNode layout)
        {
            return new RectangleHittable<ISceneNodeBound>(
                layout, Transform.Identity, x => new AaRectangle2(Vector2.Zero, 1, 1), x => 0);
        }
    }
}