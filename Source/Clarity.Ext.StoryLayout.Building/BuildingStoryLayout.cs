using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Media.Models.Explicit;
using Clarity.Engine.Media.Models.Explicit.Embedded;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;
using Clarity.Engine.Visualization.Elements.Samplers;
using Clarity.Engine.Visualization.Graphics;

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

        private readonly IModel3D planeModel;
        private readonly IModel3D lineModel;
        private readonly IModel3D frustumModel;
        private readonly IMaterial[] colorMaterials;
        private readonly IStandardMaterial floorMaterial;
        private readonly IStandardMaterial ceilingMaterial;
        private readonly IStandardMaterial wallMaterial;
        private readonly IStandardMaterial rawWallMaterial;
        private readonly IStandardMaterial frustumMaterial;
        private readonly IStandardMaterial lineMaterial;
        private readonly IStandardMaterial currentLineMaterial;
        private readonly IRenderState lineRenderState;

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
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1f, 0f, 0f) * 0.8f, 1.0f)).SetIgnoreLighting(true),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0f, 1f, 0f) * 0.8f, 1.0f)).SetIgnoreLighting(true),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0f, 0f, 1f) * 0.8f, 1.0f)).SetIgnoreLighting(true),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1f, 1f, 0f) * 0.8f, 1.0f)).SetIgnoreLighting(true),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1f, 0f, 1f) * 0.8f, 1.0f)).SetIgnoreLighting(true),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0f, 1f, 1f) * 0.8f, 1.0f)).SetIgnoreLighting(true),
            };
            frustumMaterial = StandardMaterial.New()
                .SetDiffuseColor(new Color4(0f, 1f, 0f))
                .SetIgnoreLighting(true)
                .FromGlobalCache();
            lineMaterial = StandardMaterial.New()
                .SetDiffuseColor(Color4.White)
                .SetIgnoreLighting(true)
                .FromGlobalCache();
            currentLineMaterial = StandardMaterial.New()
                .SetDiffuseColor(Color4.Red)
                .SetIgnoreLighting(true)
                .FromGlobalCache();
            lineRenderState = StandardRenderState.New().SetLineWidth(3).FromGlobalCache();

            var mirrorSampler = new ImageSampler
            {
                AddressModeU = ImageSamplerAddressMode.Mirror,
                AddressModeV = ImageSamplerAddressMode.Mirror,
                AddressModeW = ImageSamplerAddressMode.Mirror,
            }.FromGlobalCache();

            floorMaterial = StandardMaterial.New()
                .SetDiffuseMap(embeddedResources.Image("Textures/museum_floor.jpg"))
                .SetNoSpecular(true)
                .SetSampler(mirrorSampler)
                .FromGlobalCache();
            ceilingMaterial = StandardMaterial.New()
                .SetDiffuseMap(embeddedResources.Image("Textures/museum_ceiling.jpg"))
                .SetNoSpecular(true)
                .SetSampler(mirrorSampler)
                .FromGlobalCache();
            wallMaterial = StandardMaterial.New()
                .SetDiffuseMap(embeddedResources.Image("Textures/museum_wall.jpg"))
                .SetDiffuseColor(new Color4(92, 82, 72))
                //.SetNoSpecular(true)
                .SetNormalMap(embeddedResources.Image("Textures/museum_wall_2_norm.jpg"))
                .FromGlobalCache();
            rawWallMaterial = StandardMaterial.New()
                .SetDiffuseColor(Color4.Green)
                .SetIgnoreLighting(true)
                .FromGlobalCache();
        }
        //
        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            var placementAlgorithm = new BuildingStoryLayoutPlacementAlgorithm(coroutineService, x => ArrangeAndDecorateInternal(sg.Root, x, new List<BuildingWallSegment>()), sg);
            placementAlgorithm.Run();
            var collisionSegments = new List<BuildingWallSegment>();
            ArrangeAndDecorateInternal(sg.Root, placementAlgorithm, collisionSegments);
            var floors = sg.Children[sg.Root]
                .Select(x => new AaBox(placementAlgorithm.RelativeTransforms[x].Offset, placementAlgorithm.HalfSizes[x]))
                .ToArray();
            var zonesWithProperties = floors.Select(x => Tuples.Pair(x, new StoryLayoutZoneProperties(-15f))).ToArray();
            var defaultZoneProperties = new StoryLayoutZoneProperties(0);
            var zoning = new AaBoxStoryLayoutZoning(zonesWithProperties, defaultZoneProperties);
            var buildingCollisionMesh = new CollisionMesh(collisionSegments, floors, zoning);
            return new BuildingStoryLayoutInstance(inputService, placementAlgorithm, buildingCollisionMesh, zoning);
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
            node.Transform = hasChildren
                ? placementAlgorithm.RelativeTransforms[index]
                : placementAlgorithm.RelativeTransforms[index] * Transform.Translation(0, BuildingConstants.EyeHeight, 0);

            var dynamicParts = new StoryNodeDynamicParts();
            var visualElems = new List<IVisualElement>();

            var corridorDigger = new BuildingCorridorDigger();
            var digResult = corridorDigger.Dig(placementAlgorithm, index);

            var globalTransform = placementAlgorithm.GetGlobalTransform(index);

            var rootStoryComponent = sg.Aspects[sg.Root];
            if (digResult.WallSegments.Any())
            {
                var wallModel = BuildWallModel(digResult.WallSegments);
                visualElems.Add(new ModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(wallModel)
                    .SetMaterial(wallMaterial)
                    .SetRenderState(StandardRenderState.New()
                        .SetCullFace(CullFace.Back)
                        .FromGlobalCache())
                    .SetHide(x => x.HideMain));
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
                visualElems.Add(new ModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(wallPrimitivesModel)
                    .SetMaterial(rawWallMaterial)
                    .SetHide(x => !x.ShowAux3));

                var filteredWallPrimitivesModel = BuildWallModel4(digResult.WallSegments);
                visualElems.Add(new ModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(filteredWallPrimitivesModel)
                    .SetMaterial(rawWallMaterial)
                    .SetHide(x => !x.ShowAux4));
            }

            foreach (var flooring in digResult.Floorings)
            {
                visualElems.Add(new ModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(BuildFloorOrCeiling(new Size3(flooring.HalfWidth, 0, flooring.HalfHeight), PlaneModelSourceNormalDirection.Positive))
                    .SetMaterial(floorMaterial)
                    .SetRenderState(StandardRenderState.New()
                        .SetCullFace(CullFace.Back)
                        .FromGlobalCache())
                    .SetTransform(Transform.Translation(flooring.Center.X, 0, flooring.Center.Y))
                    .SetHide(x => x.HideMain));
            }

            if (index == sg.Root)
            {
                foreach (var lane in placementAlgorithm.Lanes.Values)
                {
                    var laneLoc = lane;
                    var navigationService = navigationServiceLazy.Value;
                    var model = BuildLaneModel(lane);
                    visualElems.Add(ModelVisualElement.New()
                        .SetModel(model)
                        .SetMaterial(x => new UnorderedPair<int>(navigationService.Previous.Id, navigationService.Current.Id) == new UnorderedPair<int>(laneLoc.Edge.First, laneLoc.Edge.Second) ? currentLineMaterial : lineMaterial)
                        .SetRenderState(StandardRenderState.New()
                            // todo: remove *5
                            .SetZOffset(GraphicsHelper.MinZOffset * 5)
                            .SetLineWidth(3)
                            .FromGlobalCache()));
                }
            }

            dynamicParts.Hittable = hasChildren
                ? new RectangleHittable<ISceneNode>(node, Transform.Rotate(Quaternion.RotationToFrame(Vector3.UnitX, Vector3.UnitZ)), x => new AaRectangle2(Vector2.Zero, halfSize.Width, halfSize.Height), x => -0.01f * depth)
                : new RectangleHittable<ISceneNode>(node, new Transform(1, Quaternion.RotationToFrame(Vector3.UnitX, Vector3.UnitZ), new Vector3(0, -BuildingConstants.EyeHeight, 0)), x => new AaRectangle2(Vector2.Zero, halfSize.Width, halfSize.Height), x => -0.01f * depth);

            if (depth == 1)
            {
                visualElems.Add(new ModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(BuildFloorOrCeiling(halfSize, PlaneModelSourceNormalDirection.Negative))
                    .SetMaterial(ceilingMaterial)
                    .SetRenderState(StandardRenderState.New()
                        .SetCullFace(CullFace.Back)
                        .FromGlobalCache())
                    .SetTransform(Transform.Translation(0, BuildingConstants.CeilingHeight, 0))
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
                dynamicParts.DefaultViewpointMechanism =
                    new WallDefaultViewpointMechanism(node, new TargetedControlledCameraY.Props
                    {
                        Distance = FrustumDistance,
                        FieldOfView = MathHelper.PiOver4,
                        Pitch = 0,
                        Yaw = 0,
                        ZNear = 0.01f,
                        ZFar = 1000f
                    });

                visualElems.Add(new ModelVisualElement<IStoryComponent>(rootStoryComponent)
                    .SetModel(frustumModel)
                    .SetMaterial(frustumMaterial)
                    .SetRenderState(StandardRenderState.New()
                        .SetZOffset(GraphicsHelper.MinZOffset * (depth + 1))
                        .FromGlobalCache())
                    .SetHide(x => !x.ShowAux1));

                dynamicParts.PlacementSurface2D = new PlanarPlacementSurface(node, new Transform(2f, Quaternion.Identity, new Vector3(0, 0, -MathHelper.FrustumDistance)));
                dynamicParts.PlacementSurface3D = new PlanarPlacementSurface(node, Transform.Scaling(0.1f));
            }

            dynamicParts.VisualElements = visualElems;

            aspect.SetDynamicParts(dynamicParts);
        }

        private ModelVisualElement<object> CreateLaneElem(Vector3 prevPoint, Vector3 currPoint, int disambiguator, int depth)
        {
            return ModelVisualElement.New()
                .SetModel(lineModel)
                .SetMaterial(lineMaterial)
                .SetRenderState(StandardRenderState.New()
                    .SetLineWidth(3)
                    .SetZOffset(GraphicsHelper.MinZOffset * (depth + 1))
                    .FromGlobalCache())
                .SetTransform(CalcCorridorSegmentTransform(prevPoint, currPoint, disambiguator));
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

        private IExplicitModel BuildLaneModel(BuildingLane lane)
        {
            var vertices = lane.GlobalPath
                .Where(x => x.P0.X != x.P2.X)
                .Select(x => ApplyDisambiguator(x, lane.Disambiguator))
                .SelectMany(x => x.ToEnumPolyline(0.01f))
                .Select(x => new VertexPos(x))
                .ToArray();
            return ExplicitModel.FromVertices(vertices, null, ExplicitModelPrimitiveTopology.LineStrip);
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

        private static IExplicitModel BuildWallModel(List<BuildingWallSegment> wallSegments)
        {
            var vertices = new VertexPosTanNormTex[wallSegments.Count * 4];
            var indices = new int[wallSegments.Count * 6];
            for (int i = 0; i < wallSegments.Count; i++)
            {
                var segment = wallSegments[i].Basement;
                var height = wallSegments[i].Height;
                var vertexOffset = i * 4;
                var indexOffset = i * 6;
                var tangent = (segment.Point2 - segment.Point1).Normalize();
                var normal = Vector3.Cross(tangent, Vector3.UnitY).Normalize();
                var texCoordWidth = Math.Max((float)Math.Round(segment.Length / 2), 1f);
                var texCoordHeight = Math.Max((float)Math.Round(height / 2), 1f);
                vertices[vertexOffset + 0] = new VertexPosTanNormTex(segment.Point1, tangent, normal, new Vector2(0, texCoordHeight));
                vertices[vertexOffset + 1] = new VertexPosTanNormTex(segment.Point1 + height * Vector3.UnitY, tangent, normal, new Vector2(0, 0));
                vertices[vertexOffset + 2] = new VertexPosTanNormTex(segment.Point2 + height * Vector3.UnitY, tangent, normal, new Vector2(texCoordWidth, 0));
                vertices[vertexOffset + 3] = new VertexPosTanNormTex(segment.Point2, tangent, normal, new Vector2(texCoordWidth, texCoordHeight));
                indices[indexOffset + 0] = vertexOffset;
                indices[indexOffset + 1] = vertexOffset + 1;
                indices[indexOffset + 2] = vertexOffset + 2;
                indices[indexOffset + 3] = vertexOffset;
                indices[indexOffset + 4] = vertexOffset + 2;
                indices[indexOffset + 5] = vertexOffset + 3;
            }
            return ExplicitModel.FromVertices(vertices, indices, ExplicitModelPrimitiveTopology.TriangleList);
        }

        private static IExplicitModel BuildWallModel4(List<BuildingWallSegment> wallSegments)
        {
            var vertices = new VertexPos[wallSegments.Count * 2];
            for (int i = 0; i < wallSegments.Count; i++)
            {
                var segment = wallSegments[i].Basement;
                var vertexOffset = i * 2;
                vertices[vertexOffset + 0] = new VertexPos(segment.Point1);
                vertices[vertexOffset + 1] = new VertexPos(segment.Point2);
            }
            return ExplicitModel.FromVertices(vertices, null, ExplicitModelPrimitiveTopology.LineList);
        }

        private IModel3D BuildFloorOrCeiling(Size3 halfSize, PlaneModelSourceNormalDirection direction)
        {
            return embeddedResources.PlaneModel(PlaneModelSourcePlane.Xz, direction, halfSize.Width, halfSize.Depth, 1, 1);
        }
    }
}