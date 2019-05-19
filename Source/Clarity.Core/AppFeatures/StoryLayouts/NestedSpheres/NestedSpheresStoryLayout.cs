using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
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
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppFeatures.StoryLayouts.NestedSpheres
{
    public class NestedSpheresStoryLayout : IStoryLayout
    {
        public string UserFriendlyName => "Nested Spheres";
        public Type Type => typeof(NestedSpheresStoryLayout);

        private readonly IInputService inputService;
        private readonly ICoroutineService coroutineService;
        private readonly IWorldTreeService worldTreeService;
        private readonly Lazy<INavigationService> navigationServiceLazy;

        private readonly IFlexibleModel mainModel;
        private readonly IFlexibleModel lineModel;
        private readonly IMaterial[] sphereMaterials;
        private readonly IMaterial lineMaterial;
        private readonly IMaterial lineMaterialExternal;

        private const float ChildScale = 0.1f;
        private static readonly Vector2 DefaultEndYawPitch = new Vector2(MathHelper.Pi, 0);

        public NestedSpheresStoryLayout(IEmbeddedResources embeddedResources, IInputService inputService, ICoroutineService coroutineService, IWorldTreeService worldTreeService, Lazy<INavigationService> navigationServiceLazy)
        {
            this.inputService = inputService;
            this.coroutineService = coroutineService;
            this.worldTreeService = worldTreeService;
            this.navigationServiceLazy = navigationServiceLazy;
            mainModel = embeddedResources.SphereModel(64, true);
            lineModel = embeddedResources.LineModel();
            sphereMaterials = new IMaterial[]
            {
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1.0f, 0.5f, 0.5f) * 1.0f, 0.5f))) {IgnoreLighting = false, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0.5f, 1.0f, 0.5f) * 1.0f, 0.5f))) {IgnoreLighting = false, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0.5f, 0.5f, 1.0f) * 1.0f, 0.5f))) {IgnoreLighting = false, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1.0f, 1.0f, 0.5f) * 1.0f, 0.5f))) {IgnoreLighting = false, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1.0f, 0.5f, 1.0f) * 1.0f, 0.5f))) {IgnoreLighting = false, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0.5f, 1.0f, 1.0f) * 1.0f, 0.5f))) {IgnoreLighting = false, LineWidth = 2},
            };
            lineMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Red)) { LineWidth = 3 };
            lineMaterialExternal = new StandardMaterial(new SingleColorPixelSource(0.7f * Color4.Red)) { LineWidth = 3 };
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            var springModel = new NestedSpheresStorySpringModel(coroutineService, x => ArrangeAndDecorateInternal(sg.Root, 0, x, sg), sg);
            springModel.Apply();
            var sceneComponent = sg.NodeObjects[sg.Root].Scene;
            sceneComponent.RenderStageDistribution = new NestedSpheresRenderStageDistribution(navigationServiceLazy.Value);
            return new BasicStoryLayoutInstance(worldTreeService);
        }

        private void ArrangeAndDecorateInternal(int nodeIndex, int level, NestedSpheresStorySpringModel springModel, IStoryGraph sg)
        {
            var node = sg.NodeObjects[nodeIndex];
            var aspect = sg.Aspects[nodeIndex];
            var index = node.Id;

            var dynamicParts = new StoryNodeDynamicParts();
            var scale = springModel.GetVisualRadius(index);
            var visualElems = new List<IVisualElement>
            {
                new CgModelVisualElement<IStoryComponent>(sg.Aspects[sg.Root])
                    .SetModel(mainModel)
                    .SetMaterial(sphereMaterials[level % sphereMaterials.Length])
                    .SetTransform(new Transform(scale, Quaternion.Identity, /*todo: remove this hack*/new Vector3(0, 2, 0)))
                    .SetCullFace(CgCullFace.Back)
                    //.SetTransformSpace(CgTransformSpace.ScreenAlighned)
                    .SetGetDistanceToCameraSq((o, t, c) => ((t.Offset - c.GetEye()).Length() + scale).Sq())
                    .SetHide(x => !x.ShowAux1)
            };

            node.Transform = new Transform(1, Quaternion.RotationY(springModel.GetRotation(index)), springModel.GetPosition(index));

            if (level == 0)
            {
                var edgeVisuals = sg.Edges.Select(edge => CreateEdgeVisualElement(node, sg.NodeObjects[edge.First], sg.NodeObjects[edge.Second]));
                visualElems.AddRange(edgeVisuals);
            }

            if (sg.Children[index].Any())
            {
                foreach (var childIndex in sg.Children[index])
                    ArrangeAndDecorateInternal(childIndex, level + 1, springModel, sg);
                dynamicParts.DefaultViewpointMechanism = new WallDefaultViewpointMechanism(node, new TargetedControlledCameraY.Props
                {
                    // todo: remove this hack
                    Target = new Vector3(0, 2, 0),
                    Yaw = MathHelper.PiOver4,
                    Pitch = MathHelper.PiOver4,
                    ZNear = 0.01f,
                    ZFar = 1000f,
                    FieldOfView = MathHelper.PiOver4,
                    Distance = 2f * scale,
                });
            }
            else
            {
            dynamicParts.DefaultViewpointMechanism = new WallDefaultViewpointMechanism(node, new TargetedControlledCameraY.Props
            {
                    // todo: remove this hack
                    Target = new Vector3(0, 2, 0),
                
                ZNear = 0.01f,
                ZFar = 1000f,
                FieldOfView = MathHelper.PiOver4,
                    Distance = GraphicsHelper.FrustumDistance,
            });
            }
            dynamicParts.Hittable = new SphereHittable<ISceneNode>(node, x => new Sphere(x.GlobalTransform.Offset + /*todo: remove this hack*/new Vector3(0, 2, 0), scale), true);


            dynamicParts.VisualElements = visualElems;
            
            aspect.SetDynamicParts(dynamicParts);
        }

        private IVisualElement CreateEdgeVisualElement(ISceneNode root, ISceneNode node1, ISceneNode node2)
        {
            var external = node1.ParentNode != node2.ParentNode;

            return external
                ? new CgModelVisualElement<IStoryComponent>(root.GetComponent<IStoryComponent>())
                    .SetModel(lineModel)
                    .SetMaterial(lineMaterialExternal)
                    .SetTransform(x => GetTransformBetweenNodes(root, node1, node2))
                    .SetHide(x => !x.ShowAux2)
                : new CgModelVisualElement<IStoryComponent>(root.GetComponent<IStoryComponent>())
                    .SetModel(lineModel)
                    .SetMaterial(lineMaterial)
                    .SetTransform(x => GetTransformBetweenNodes(root, node1, node2))
                    .SetHide(x => !x.ShowAux2);
        }

        private static Transform GetTransformBetweenNodes(ISceneNode root, ISceneNode node1, ISceneNode node2)
        {
            var point1 = GetTransfprmToIndirectParent(root, node1).Offset;
            var point2 = GetTransfprmToIndirectParent(root, node2).Offset;
            var dir = point2 - point1;
            var scale = dir.Length();
            var rotation = Quaternion.RotationToFrame(dir, Vector3.UnitY);

            // todo: remove this hack
            point1.Y += 2;

            return new Transform(scale, rotation, point1);
        }

        private static Transform GetTransfprmToIndirectParent(ISceneNode parent, ISceneNode node)
        {
            var immediateParent = node.ParentNode;
            var transformToImmediateParent = node.Transform;
            return immediateParent == parent
                ? transformToImmediateParent
                : transformToImmediateParent * GetTransfprmToIndirectParent(parent, immediateParent);
        }
    }
}