using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.Coroutines;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Models.Flexible.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Objects.WorldTree.RenderStageDistribution;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Ext.StoryLayout.Building
{
    public class BuildingStoryLayout : IStoryLayout
    {
        public Type Type => typeof(BuildingStoryLayout);
        public string UserFriendlyName => "Building";

        private readonly ICoroutineService coroutineService;
        private readonly IEmbeddedResources embeddedResources;
        private readonly IInputService inputService;
        private readonly Lazy<INavigationService> navigationServiceLazy;

        private readonly IFlexibleModel planeModel;
        private readonly IFlexibleModel lineModel;
        private readonly IFlexibleModel frustumModel;
        private readonly IMaterial[] colorMaterials;
        private readonly IStandardMaterial floorMaterial;
        private readonly IStandardMaterial ceilingMaterial;
        private readonly IStandardMaterial wallMaterial;
        private readonly IStandardMaterial rawWallMaterial;
        private readonly IStandardMaterial frustumMaterial;
        private readonly IStandardMaterial lineMaterial;
        private readonly IStandardMaterial currentLineMaterial;

        private const float FrustumDistance = 2.414213562373095f;

        public BuildingStoryLayout(ICoroutineService coroutineService, IEmbeddedResources embeddedResources, 
            IInputService inputService, Lazy<INavigationService> navigationServiceLazy)
        {
            this.embeddedResources = embeddedResources;
            this.inputService = inputService;
            this.navigationServiceLazy = navigationServiceLazy;
            this.coroutineService = coroutineService;
            planeModel = embeddedResources.SimplePlaneXzModel();
            lineModel = embeddedResources.LineModel();
            frustumModel = embeddedResources.SimpleFrustumModel();
            colorMaterials = new IMaterial[]
            {
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1f, 0f, 0f) * 0.8f, 1.0f))) {IgnoreLighting = true},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0f, 1f, 0f) * 0.8f, 1.0f))) {IgnoreLighting = true},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0f, 0f, 1f) * 0.8f, 1.0f))) {IgnoreLighting = true},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1f, 1f, 0f) * 0.8f, 1.0f))) {IgnoreLighting = true},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1f, 0f, 1f) * 0.8f, 1.0f))) {IgnoreLighting = true},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0f, 1f, 1f) * 0.8f, 1.0f))) {IgnoreLighting = true},
            };
            frustumMaterial = new StandardMaterial(new SingleColorPixelSource(new Color4(0f, 1f, 0f))) {IgnoreLighting = true};
            lineMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.White)) {IgnoreLighting = true, LineWidth = 3};
            currentLineMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Red)) {IgnoreLighting = true, LineWidth = 3};

            floorMaterial = new StandardMaterial(embeddedResources.Image("Textures/museum_floor.jpg"))
            {
                NoSpecular = true,
            };
            ceilingMaterial = new StandardMaterial(embeddedResources.Image("Textures/museum_ceiling.jpg"))
            {
                NoSpecular = true,
            };
            wallMaterial = new StandardMaterial(embeddedResources.Image("Textures/museum_wall.jpg"))
            {
                NoSpecular = true,
            };
            rawWallMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Green))
            {
                IgnoreLighting = true,
                LineWidth = 2
            };
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            var placementAlgorithm = new BuildingStoryLayoutPlacementAlgorithm(coroutineService, x => ArrangeAndDecorateInternal(sg.Root, x, new List<BuildingWallSegment>()), sg);
            placementAlgorithm.Run();
            var globalWallSegments = new List<BuildingWallSegment>();
            ArrangeAndDecorateInternal(sg.Root, placementAlgorithm, globalWallSegments);
            var sceneComponent = sg.NodeObjects[sg.Root].Scene;
            sceneComponent.RenderStageDistribution = new FocusedOnlyRenderStageDistribution();
            return new BuildingStoryLayoutInstance(inputService, placementAlgorithm, globalWallSegments);
        }

        private void ArrangeAndDecorateInternal(int subtreeRoot, BuildingStoryLayoutPlacementAlgorithm placementAlgorithm, List<BuildingWallSegment> globalWallSegments)
        {
            var sg = placementAlgorithm.StoryGraph;
            var node = sg.NodeObjects[subtreeRoot];
            var aspect = sg.Aspects[subtreeRoot];
            var index = node.Id;
            var children = sg.Children[index];

            foreach (var child in children)
            {
                ArrangeAndDecorateInternal(child, placementAlgorithm, globalWallSegments);
            }
            
            var depth = sg.Depths[subtreeRoot];

            var hasChildren = children.Any();

            var halfSize = placementAlgorithm.HalfSizes[index];
            node.Transform = placementAlgorithm.RelativeTransforms[index];

            var dynamicParts = new StoryNodeDynamicParts();
            var visualElems = new List<IVisualElement>();

            var corridorDigger = new BuildingCorridorDigger();
            var digResult = corridorDigger.Dig(placementAlgorithm, index);

            var globalTransform = placementAlgorithm.GetGlobalTransform(index);

            var rootStoryComponent = sg.Aspects[sg.Root];
            if (digResult.WallSegments.Any())
            {
                var wallModel = BuildWallModel(digResult.WallSegments);
                    visualElems.Add(new CgModelVisualElement<IStoryComponent>(rootStoryComponent)
                        .SetModel(wallModel)
                        .SetMaterial(wallMaterial)
                        .SetHide(x => x.HideMain)
                        //.SetZOffset(GraphicsHelper.MinZOffset * depth)
                );
                // todo: remove?
                foreach (var wallSegment in digResult.WallSegments)
                {
                    globalWallSegments.Add(new BuildingWallSegment
                    {
                        Basement = new LineSegment3(
                            wallSegment.Basement.Point1 * globalTransform,
                            wallSegment.Basement.Point2 * globalTransform),
                        Height = wallSegment.Height
                    });
                }


                var wallPrimitivesModel = BuildWallModel4(digResult.RawWallSegments);
                visualElems.Add(new CgModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(wallPrimitivesModel)
                    .SetMaterial(rawWallMaterial)
                    .SetHide(x => !x.ShowAux3));

                var filteredWallPrimitivesModel = BuildWallModel4(digResult.WallSegments);
                visualElems.Add(new CgModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(filteredWallPrimitivesModel)
                    .SetMaterial(rawWallMaterial)
                    .SetHide(x => !x.ShowAux4));
            }

            foreach (var flooring in digResult.Floorings)
            {
                visualElems.Add(new CgModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(BuildFloorOrCeiling(new Size3(flooring.HalfWidth, 0, flooring.HalfHeight), PlaneModelSourceNormalDirection.Positive))
                    .SetMaterial(floorMaterial)
                    .SetTransform(Transform.Translation(flooring.Center.X, 0, flooring.Center.Y))
                    .SetCullFace(CgCullFace.Back)
                    .SetHide(x => x.HideMain));
            }

            if (index == sg.Root)
            {
                foreach (var lane in placementAlgorithm.Lanes.Values)
                {
                    var laneLoc = lane;
                    var navigationService = navigationServiceLazy.Value;
                    var model = BuildLaneModel(lane);
                    visualElems.Add(new CgModelVisualElement()
                        .SetModel(model)
                        .SetMaterial(x => new UnorderedPair<int>(navigationService.Previous.Id, navigationService.Current.Id) == new UnorderedPair<int>(laneLoc.Edge.First, laneLoc.Edge.Second) ? currentLineMaterial : lineMaterial)
                        // todo: remove *5
                        .SetZOffset(GraphicsHelper.MinZOffset * 5));
                }
            }

            dynamicParts.Hittable = new RectangleHittable<ISceneNode>(node, Transform.Rotate(Quaternion.RotationToFrame(Vector3.UnitX, Vector3.UnitZ)), x => new AaRectangle2(Vector2.Zero, halfSize.Width, halfSize.Height), x => -1f * depth);

            if (depth == 1)
            {
                visualElems.Add(new CgModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(BuildFloorOrCeiling(halfSize, PlaneModelSourceNormalDirection.Negative))
                    .SetMaterial(ceilingMaterial)
                    .SetTransform(Transform.Translation(0, BuildingConstants.CeilingHeight, 0))
                    .SetCullFace(CgCullFace.Back)
                    .SetHide(x => x.HideMain));
            }

            if (hasChildren)
            {
                var size = halfSize.ToVector().Length();
                dynamicParts.DefaultViewpointMechanism =
                    new WallDefaultViewpointMechanism(node, new TargetedControlledCameraY.Props
                    {
                        Target = Vector3.Zero,
                        Distance = size,
                        FieldOfView = MathHelper.PiOver4,
                        Pitch = MathHelper.PiOver4,
                        Yaw = -MathHelper.PiOver4,
                        ZNear = 0.01f,
                        ZFar = 1000f
                    });
            }
            else
            {
                var target = Vector3.UnitY * halfSize.Height;
                dynamicParts.DefaultViewpointMechanism =
                    new WallDefaultViewpointMechanism(node, new TargetedControlledCameraY.Props
                    {
                        Target = target,
                        Distance = FrustumDistance,
                        FieldOfView = MathHelper.PiOver4,
                        Pitch = 0,
                        Yaw = 0,
                        ZNear = 0.01f,
                        ZFar = 1000f
                    });
                
                visualElems.Add(new CgModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(frustumModel)
                    .SetMaterial(frustumMaterial)
                    .SetTransform(Transform.Translation(target))
                    .SetZOffset(GraphicsHelper.MinZOffset * (depth + 1))
                    .SetHide(x => !x.ShowAux1));
            }

            dynamicParts.VisualElements = visualElems;

            aspect.SetDynamicParts(dynamicParts);
        }

        private CgModelVisualElement<object> CreateLaneElem(Vector3 prevPoint, Vector3 currPoint, int disambiguator, int depth)
        {
            return new CgModelVisualElement()
                .SetModel(lineModel)
                .SetMaterial(lineMaterial)
                .SetTransform(CalcCorridorSegmentTransform(prevPoint, currPoint, disambiguator))
                .SetZOffset(GraphicsHelper.MinZOffset * (depth + 1));
        }

        private static Transform CalcCorridorSegmentTransform(Vector3 p1, Vector3 p2, int disambiguator)
        {
            var forward = p2 - p1;
            var scale = forward.Length();
            var xzDiff = (p1.Xz - p2.Xz).LengthSquared();
            var rotation = Quaternion.RotationToFrame(forward, xzDiff < MathHelper.Eps8 ? Vector3.UnitX : Vector3.UnitY);
            var offset = p1 + Vector3.UnitZ * disambiguator * BuildingConstants.CorridorDisambiguationOffset;
            return new Transform(scale, rotation, offset);
        }

        private IFlexibleModel BuildLaneModel(BuildingLane lane)
        {
            var vertices = lane.GlobalPath
                .Where(x => x.P0.X != x.P2.X)
                .Select(x => ApplyDisambiguator(x, lane.Disambiguator))
                .SelectMany(x => x.ToEnumPolyline(0.01f))
                .Select(x => new CgVertexPosNormTex(x, Vector3.UnitY, Vector2.Zero))
                .ToArray();
            return FlexibleModelHelpers.CreateSimple(null, vertices, null, FlexibleModelPrimitiveTopology.LineStrip);
        }

        private static BezierQuadratic3 ApplyDisambiguator(BezierQuadratic3 bezier, int disambiguator)
        {
            var offset = bezier.P2.X >= bezier.P0.X
                ? BuildingConstants.CorridorDisambiguationOffset * disambiguator * Vector3.UnitZ
                : -BuildingConstants.CorridorDisambiguationOffset * disambiguator * Vector3.UnitZ;
            if (disambiguator % 2 == 1)
                offset = -offset;
            return new BezierQuadratic3(bezier.P0 + offset, bezier.P1 + offset, bezier.P2 + offset);
        }

        private IFlexibleModel BuildWallModel(List<Vector3> points)
        {
            var vertices = points.Select(x => new CgVertexPosNormTex(x, Vector3.UnitY, Vector2.Zero)).ToArray();
            return FlexibleModelHelpers.CreateSimple(null, vertices, null, FlexibleModelPrimitiveTopology.LineStrip);
        }

        private IFlexibleModel BuildWallModel2(List<LineSegment3> segments)
        {
            var vertices = segments.SelectMany(x => new[]{x.Point1, x.Point2}).Select(x => new CgVertexPosNormTex(x, Vector3.UnitY, Vector2.Zero)).ToArray();
            return FlexibleModelHelpers.CreateSimple(null, vertices, null, FlexibleModelPrimitiveTopology.LineList);
        }

        private static IFlexibleModel BuildWallModel(List<BuildingWallSegment> wallSegments)
        {
            var vertices = new CgVertexPosNormTex[wallSegments.Count * 4];
            var indices = new int[wallSegments.Count * 6];
            for (int i = 0; i < wallSegments.Count; i++)
            {
                var segment = wallSegments[i].Basement;
                var height = wallSegments[i].Height;
                var vertexOffset = i * 4;
                var indexOffset = i * 6;
                var normal = Vector3.Cross(segment.Point2 - segment.Point1, Vector3.UnitY).Normalize();
                var texCoordWidth = Math.Max((float)Math.Round(segment.Length / 2), 1f);
                var texCoordHeight = Math.Max((float)Math.Round(height / 2), 1f);
                vertices[vertexOffset + 0] = new CgVertexPosNormTex(segment.Point1, normal, new Vector2(0, 0));
                vertices[vertexOffset + 1] = new CgVertexPosNormTex(segment.Point1 + height * Vector3.UnitY, normal, new Vector2(0, texCoordHeight));
                vertices[vertexOffset + 2] = new CgVertexPosNormTex(segment.Point2 + height * Vector3.UnitY, normal, new Vector2(texCoordWidth, texCoordHeight));
                vertices[vertexOffset + 3] = new CgVertexPosNormTex(segment.Point2, normal, new Vector2(texCoordWidth, 0));
                indices[indexOffset + 0] = vertexOffset;
                indices[indexOffset + 1] = vertexOffset + 1;
                indices[indexOffset + 2] = vertexOffset + 2;
                indices[indexOffset + 3] = vertexOffset;
                indices[indexOffset + 4] = vertexOffset + 2;
                indices[indexOffset + 5] = vertexOffset + 3;
            }
            
            return FlexibleModelHelpers.CreateSimple(null, vertices, indices, FlexibleModelPrimitiveTopology.TriangleList);
        }

        private static IFlexibleModel BuildWallModel4(List<BuildingWallSegment> wallSegments)
        {
            var vertices = new CgVertexPosNormTex[wallSegments.Count * 2];
            for (int i = 0; i < wallSegments.Count; i++)
            {
                var segment = wallSegments[i].Basement;
                var vertexOffset = i * 2;
                vertices[vertexOffset + 0] = new CgVertexPosNormTex(segment.Point1, Vector3.UnitY, new Vector2(0, 0));
                vertices[vertexOffset + 1] = new CgVertexPosNormTex(segment.Point2, Vector3.UnitY, new Vector2(0, 0));
            }
            
            return FlexibleModelHelpers.CreateSimple(null, vertices, null, FlexibleModelPrimitiveTopology.LineList);
        }

        private IFlexibleModel BuildFloorOrCeiling(Size3 halfSize, PlaneModelSourceNormalDirection direction)
        {
            return embeddedResources.PlaneModel(PlaneModelSourcePlane.Xz, direction, halfSize.Width, halfSize.Depth, 1, 1);
        }
    }
}