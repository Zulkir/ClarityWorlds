using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.App.Worlds.StoryLayouts.NestedSpheres
{
    public class NestedSpheresStoryLayout : IStoryLayout
    {
        public string UserFriendlyName => "Nested Spheres";
        public Type Type => typeof(NestedSpheresStoryLayout);

        private readonly ICoroutineService coroutineService;

        private readonly IModel3D mainModel;
        private readonly IModel3D lineModel;
        private readonly IMaterial[] sphereMaterials;
        private readonly IMaterial lineMaterial;
        private readonly IMaterial lineMaterialExternal;
        private readonly IRenderState sphereRenderState;
        private readonly IRenderState lineRenderState;

        public NestedSpheresStoryLayout(IEmbeddedResources embeddedResources, ICoroutineService coroutineService)
        {
            this.coroutineService = coroutineService;
            mainModel = embeddedResources.SphereModel(64, true);
            lineModel = embeddedResources.LineModel();
            sphereMaterials = new IMaterial[]
            {
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1.0f, 0.5f, 0.5f) * 1.0f, 0.5f)).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0.5f, 1.0f, 0.5f) * 1.0f, 0.5f)).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0.5f, 0.5f, 1.0f) * 1.0f, 0.5f)).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1.0f, 1.0f, 0.5f) * 1.0f, 0.5f)).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1.0f, 0.5f, 1.0f) * 1.0f, 0.5f)).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0.5f, 1.0f, 1.0f) * 1.0f, 0.5f)).FromGlobalCache(),
            };
            sphereRenderState = StandardRenderState.New().SetCullFace(CullFace.Back).FromGlobalCache();

            lineMaterial = StandardMaterial.New().SetDiffuseColor(Color4.Red).FromGlobalCache();
            lineMaterialExternal = StandardMaterial.New().SetDiffuseColor(0.7f * Color4.Red);
            lineRenderState = StandardRenderState.New().SetLineWidth(3).FromGlobalCache();
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            var springModel = new NestedSpheresStorySpringModel(coroutineService, x => ArrangeAndDecorateInternal(sg.Root, 0, x, sg), sg);
            springModel.Apply();
            return new BasicStoryLayoutInstance(sg);
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
                new ModelVisualElement<IStoryComponent>(sg.Aspects[sg.Root])
                    .SetModel(mainModel)
                    .SetMaterial(sphereMaterials[level % sphereMaterials.Length])
                    .SetRenderState(sphereRenderState)
                    .SetTransform(new Transform(scale, Quaternion.Identity, Vector3.Zero))
                    //.SetTransformSpace(TransformSpace.ScreenAlighned)
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
                    ZNear = 0.01f,
                    ZFar = 1000f,
                    FieldOfView = MathHelper.PiOver4,
                        Distance = GraphicsHelper.FrustumDistance,
                });
                dynamicParts.PlacementSurface2D = new PlanarPlacementSurface(node, new Transform(2f, Quaternion.Identity, new Vector3(0, 0, -MathHelper.FrustumDistance)));
                dynamicParts.PlacementSurface3D = new PlanarPlacementSurface(node, Transform.Scaling(0.1f));
            }
            dynamicParts.Hittable = new SphereHittable<ISceneNode>(node, x => new Common.Numericals.Geometry.Sphere(x.GlobalTransform.Offset, scale), true);
            dynamicParts.VisualElements = visualElems;
            
            aspect.SetDynamicParts(dynamicParts);
        }

        private IVisualElement CreateEdgeVisualElement(ISceneNode root, ISceneNode node1, ISceneNode node2)
        {
            var external = node1.ParentNode != node2.ParentNode;

            return external
                ? new ModelVisualElement<IStoryComponent>(root.GetComponent<IStoryComponent>())
                    .SetModel(lineModel)
                    .SetMaterial(lineMaterialExternal)
                    .SetRenderState(lineRenderState)
                    .SetTransform(x => GetTransformBetweenNodes(root, node1, node2))
                    .SetHide(x => !x.ShowAux2)
                : new ModelVisualElement<IStoryComponent>(root.GetComponent<IStoryComponent>())
                    .SetModel(lineModel)
                    .SetMaterial(lineMaterial)
                    .SetRenderState(lineRenderState)
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