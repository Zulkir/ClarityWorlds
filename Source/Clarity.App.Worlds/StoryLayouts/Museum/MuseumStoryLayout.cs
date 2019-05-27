using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Media.Models.Explicit.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.App.Worlds.StoryLayouts.Museum
{
    public class MuseumStoryLayout : IStoryLayout
    {
        public string UserFriendlyName => "Museum";
        public Type Type => typeof(MuseumStoryLayout);

        private readonly IWorldTreeService worldTreeService;

        private readonly IModel3D floorModel;
        private readonly IModel3D ceilingModel;
        private readonly IModel3D frustumModel;
        private readonly IModel3D exitWallModel;
        private readonly IModel3D corridorWallModel;

        private readonly IStandardMaterial floorMaterial;
        private readonly IStandardMaterial ceilingMaterial;
        private readonly IStandardMaterial wallMaterial;
        private readonly IStandardMaterial frustumMaterial;

        private const float FrustumDistance = 2.414213562373095f;
        private const float centerHeight = 3.25f;
        private const float ceilingHalfHeight = 5f;
        private const float corridorHalfWidth = 5f;
        private const float exitHalfLength = 6f;

        private static readonly float discRadius = MathHelper.Sqrt(2);
        private static readonly float globalRadius = discRadius + 2 * exitHalfLength * 100;

        public MuseumStoryLayout(IEmbeddedResources embeddedResources, IWorldTreeService worldTreeService)
        {
            this.worldTreeService = worldTreeService;
            floorModel = embeddedResources.PlaneModel(PlaneModelSourcePlane.Xy, PlaneModelSourceNormalDirection.Positive,
                globalRadius, globalRadius, 1, 1);
            ceilingModel = embeddedResources.PlaneModel(PlaneModelSourcePlane.Xy, PlaneModelSourceNormalDirection.Negative,
                globalRadius, globalRadius, 1, 1);
            exitWallModel = embeddedResources.PlaneModel(PlaneModelSourcePlane.Xz, PlaneModelSourceNormalDirection.Negative,
                exitHalfLength, ceilingHalfHeight, 1, 1);
            corridorWallModel = embeddedResources.PlaneModel(PlaneModelSourcePlane.Xz, PlaneModelSourceNormalDirection.Negative,
                corridorHalfWidth, ceilingHalfHeight, 1, 1);
            frustumModel = embeddedResources.SimpleFrustumModel();
            floorMaterial = StandardMaterial.New()
                .SetDiffuseMap(embeddedResources.Image("Textures/museum_floor.jpg"))
                .SetNoSpecular(true)
                .FromGlobalCache();
            ceilingMaterial = StandardMaterial.New()
                .SetDiffuseMap(embeddedResources.Image("Textures/museum_ceiling.jpg"))
                .SetNoSpecular(true)
                .FromGlobalCache();
            wallMaterial = StandardMaterial.New()
                .SetDiffuseMap(embeddedResources.Image("Textures/museum_wall.jpg"))
                .SetNoSpecular(true)
                .FromGlobalCache();
            frustumMaterial = StandardMaterial.New()
                .SetDiffuseColor(Color4.Green)
                .SetIgnoreLighting(true)
                .FromGlobalCache();
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            ArrangeAndDecorateRoot(sg);
            return new BasicStoryLayoutInstance(worldTreeService);
        }
        
        private void ArrangeAndDecorateRoot(IStoryGraph sg)
        {
            var nodeIndex = sg.Root;
            var aStory = sg.Aspects[nodeIndex];
            var node = sg.NodeObjects[nodeIndex];
            var adaptiveChildren = sg.Children[nodeIndex];
            var numChildren = adaptiveChildren.Count;
            
            var visuals = new List<IVisualElement>();
            
            // todo: refactor to CenterLayout struct
            var wallRadius = 1f;
            
            var alpha = MathHelper.PiOver2;

            if (numChildren > 2)
            {
                alpha = MathHelper.TwoPi / (2 * numChildren);
                wallRadius = corridorHalfWidth / MathHelper.Tan(alpha / 2f);
            }
            
            for (int i = 0; i < numChildren; i++)
            {
                var centerAngle = alpha * (1 + 2 * i);
                var pos = new Vector3(
                    wallRadius * MathHelper.Cos(centerAngle),
                    wallRadius * MathHelper.Sin(centerAngle),
                    0);
                var rotation = Quaternion.RotationZ(-centerAngle + MathHelper.PiOver2);
                visuals.Add(CorridorWall(rotation, pos));
            }
            
            for (int i = 0; i < numChildren; i++)
            {
                var child = adaptiveChildren[i];

                var centerAngle = alpha * (2 * i);
                var doorPos = new Vector3(
                    wallRadius * MathHelper.Cos(centerAngle),
                    wallRadius * MathHelper.Sin(centerAngle),
                    0);
                
                var forward = doorPos.Normalize();
                var rotation = Quaternion.RotationZ(-centerAngle + MathHelper.PiOver2);
                if (sg.Children[child].Any())
                {
                    sg.NodeObjects[child].Transform = new Transform(1, rotation, doorPos);
                    ArrangeAndDecorateIntermediate(child, sg);
                }
                else
                {
                    sg.NodeObjects[child].Transform = new Transform(1, rotation, doorPos - forward * FrustumDistance);
                    ArrangeAndDecorateLeaf(child, sg);
                }
            }

            // floor
            visuals.Add(ModelVisualElement.New()
                .SetModel(floorModel)
                .SetMaterial(floorMaterial)
                .SetRenderState(StandardRenderState.New()
                    .SetCullFace(CullFace.Back)
                    .FromGlobalCache())
                .SetTransform(new Transform(1, Quaternion.Identity, new Vector3(0, 0, centerHeight - ceilingHalfHeight))));

            // ceiling
            visuals.Add(ModelVisualElement.New()
                .SetModel(ceilingModel)
                .SetMaterial(ceilingMaterial)
                .SetRenderState(StandardRenderState.New()
                    .SetCullFace(CullFace.Back)
                    .FromGlobalCache())
                .SetTransform(new Transform(1, Quaternion.Identity, new Vector3(0, 0, centerHeight + ceilingHalfHeight))));

            var viewpointProps = new LookAroundCamera.Props
            {
                Distance = 24,
                FieldOfView = MathHelper.PiOver2,
                ZNear = 0.1f,
                ZFar = 100.0f,
                Pitch = 0
            };

            aStory.SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new SphereDefaultViewpointMechanism(node, viewpointProps),
                Hittable = new DummyHittable(),
                VisualElements = visuals
            });
        }

        private void ArrangeAndDecorateIntermediate(int nodeIndex, IStoryGraph sg)
        {
            var aStory = sg.Aspects[nodeIndex];
            var node = sg.NodeObjects[nodeIndex];
            var abstractChildren = sg.Children[nodeIndex];
            var numChildren = abstractChildren.Count;

            var forward = Vector3.UnitY;
            var right = Vector3.UnitX;
            var rightWallPos = forward * exitHalfLength + right * corridorHalfWidth;
            var rightWallRot = Quaternion.RotationZ(MathHelper.PiOver2);
            var leftWallPos = forward * exitHalfLength - right * corridorHalfWidth;
            var leftWallRot = Quaternion.RotationZ(-MathHelper.PiOver2);

            var visuals = new List<IVisualElement>();

            // right exit wall
            visuals.Add(ExitWall(rightWallRot, rightWallPos));

            // left exit wall
            visuals.Add(ExitWall(leftWallRot, leftWallPos));
            
            var viewpointProps = new TargetedControlledCamera.Props
            {
                Target = Vector3.Zero,
                Distance = FrustumDistance,
                FieldOfView = MathHelper.PiOver4,
                ZNear = 0.1f * FrustumDistance,
                ZFar = 100.0f * FrustumDistance
            };

            var i = 0;
            //var numSegments = adaptiveLayout.AdaptiveChildren.Count * 2 - 1;
            var numSegments = numChildren % 2 == 0
                ? numChildren + 1
                : numChildren;

            while (i < numSegments)
            {
                //var childIndex = i / 2;
                var childIndex = i;
                
                var exitOffset = 2 * exitHalfLength;
                var neighborOffset = 2 * corridorHalfWidth;
                Vector3 childForward;

                Vector3 position;
                Quaternion rotation;

                if (i < numSegments / 2)
                {
                    childForward = right;
                    position =
                        forward * (exitOffset + neighborOffset * i + corridorHalfWidth) +
                        childForward * corridorHalfWidth;
                    rotation = rightWallRot;
                }
                else if (i > numSegments / 2)
                {
                    childForward = -right;
                    var numNeighborsAfter = numSegments - i - 1;
                    position =
                        forward * (exitOffset + neighborOffset * numNeighborsAfter + corridorHalfWidth) +
                        childForward * corridorHalfWidth;
                    rotation = leftWallRot;
                }
                else
                {
                    childForward = forward;
                    position = forward * (exitOffset + neighborOffset * i);
                    rotation = Quaternion.Identity;
                }

                if (i < numChildren)
                {
                    var adaptiveChild = abstractChildren[childIndex];
                    if (abstractChildren.Any())
                    {
                        sg.NodeObjects[adaptiveChild].Transform = new Transform(1, rotation, position);
                        ArrangeAndDecorateIntermediate(adaptiveChild, sg);
                    }
                    else
                    {
                        sg.NodeObjects[adaptiveChild].Transform = new Transform(1, rotation, position - childForward * FrustumDistance);
                        ArrangeAndDecorateLeaf(adaptiveChild, sg);
                    }
                }
                else
                {
                    visuals.Add(CorridorWall(rotation, position));
                }

                i++;
            }
            
            aStory.SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new WallDefaultViewpointMechanismZ(node, viewpointProps),
                Hittable = new DummyHittable(),
                VisualElements = visuals
            });
        }

        private void ArrangeAndDecorateLeaf(int nodeIndex, IStoryGraph sg)
        {
            var aStory = sg.Aspects[nodeIndex];
            var node = sg.NodeObjects[nodeIndex];

            var wallVisuals = CorridorWall(Quaternion.Identity, Vector3.UnitY * FrustumDistance);
            var frustumVisuals = ModelVisualElement.New()
                .SetModel(frustumModel)
                .SetMaterial(frustumMaterial)
                .SetRenderState(StandardRenderState.New()
                    .SetCullFace(CullFace.Back)
                    .FromGlobalCache())
                .SetTransform(new Transform(1, Quaternion.RotationX(-MathHelper.PiOver2), Vector3.Zero));
            var visuals = new [] {frustumVisuals, wallVisuals};

            var viewpointProps = new TargetedControlledCamera.Props
            {
                Target = Vector3.Zero,
                Distance = FrustumDistance,
                FieldOfView = MathHelper.PiOver4,
                ZNear = 0.1f * FrustumDistance,
                ZFar = 100.0f * FrustumDistance
            };

            var transform2D = new Transform(2, Quaternion.RotationX(-MathHelper.PiOver2), FrustumDistance * Vector3.UnitY);
            aStory.SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new WallDefaultViewpointMechanismZ(node, viewpointProps),
                Hittable = GetHittableComponent(node, transform2D),
                VisualElements = visuals
            });
        }

        private static IRayHittable GetHittableComponent(ISceneNode layout, Transform planeTransform)
        {
            return new RectangleHittable<ISceneNodeBound>(
                layout, planeTransform, x => new AaRectangle2(Vector2.Zero, 1, 1), x => 0);
        }

        private IVisualElement ExitWall(Quaternion rotation, Vector3 position)
        {
            return ModelVisualElement.New()
                .SetModel(exitWallModel)
                .SetMaterial(wallMaterial)
                .SetTransform(new Transform(1, -rotation, position + Vector3.UnitZ * centerHeight));
        }

        private IVisualElement CorridorWall(Quaternion rotation, Vector3 position)
        {
            return ModelVisualElement.New()
                .SetModel(corridorWallModel)
                .SetMaterial(wallMaterial)
                .SetTransform(new Transform(1, -rotation, position + Vector3.UnitZ * centerHeight));
        }
    }
}