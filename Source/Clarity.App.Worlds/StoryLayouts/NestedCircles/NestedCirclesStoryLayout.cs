using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Coroutines;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.App.Worlds.StoryLayouts.NestedCircles
{
    public class NestedCirclesStoryLayout : IStoryLayout
    {
        public string UserFriendlyName => "Nested Circles";
        public Type Type => typeof(NestedCirclesStoryLayout);

        private readonly ICoroutineService coroutineService;

        private readonly IModel3D planeModel;
        private readonly IModel3D circleModel;
        private readonly IModel3D lineModel;
        private readonly IMaterial[] circleMaterials;
        private readonly IMaterial lineMaterial;
        private readonly IMaterial lineMaterialExternal;
        private readonly IRenderState circleRenderState;
        private readonly IRenderState lineRenderState;

        private const float ChildScale = 0.1f;
        private static readonly Vector2 DefaultEndYawPitch = new Vector2(MathHelper.Pi, 0);

        public NestedCirclesStoryLayout(IEmbeddedResources embeddedResources, ICoroutineService coroutineService)
        {
            this.coroutineService = coroutineService;
            planeModel = embeddedResources.SimplePlaneXyModel();
            circleModel = embeddedResources.CircleModel(64);
            lineModel = embeddedResources.LineModel();
            circleMaterials = new IMaterial[]
            {
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1f, 0f, 0f) * 0.5f, 1.0f)).SetIgnoreLighting(true).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0f, 1f, 0f) * 0.5f, 1.0f)).SetIgnoreLighting(true).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0f, 0f, 1f) * 0.5f, 1.0f)).SetIgnoreLighting(true).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1f, 1f, 0f) * 0.5f, 1.0f)).SetIgnoreLighting(true).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(1f, 0f, 1f) * 0.5f, 1.0f)).SetIgnoreLighting(true).FromGlobalCache(),
                StandardMaterial.New().SetDiffuseColor(new Color4(new Color3(0f, 1f, 1f) * 0.5f, 1.0f)).SetIgnoreLighting(true).FromGlobalCache()
            };
            lineMaterial = StandardMaterial.New().SetDiffuseColor(Color4.Blue).FromGlobalCache();
            lineMaterialExternal = StandardMaterial.New().SetDiffuseColor(0.7f * Color4.White).FromGlobalCache();
            circleRenderState = StandardRenderState.New()
                .SetLineWidth(2)
                .SetCullFace(CullFace.Front)
                .FromGlobalCache();
            lineRenderState = StandardRenderState.New().SetLineWidth(3).FromGlobalCache();
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            var springModel = new NestedCirclesStorySpringModel(coroutineService, x => ArrangeAndDecorateInternal(sg.Root, 0, x, sg), sg);
            springModel.Apply();
            return new BasicStoryLayoutInstance(sg);
        }

        private void ArrangeAndDecorateInternal(int nodeIndex, int level, NestedCirclesStorySpringModel springModel, IStoryGraph sg)
        {
            var node = sg.NodeObjects[nodeIndex];
            var aspect = sg.Aspects[nodeIndex];
            var index = node.Id;

            var dynamicParts = new StoryNodeDynamicParts();
            var scale = springModel.GetVisualRadius(index);
            var visualElems = new List<IVisualElement>
            {
                new ModelVisualElement<object>(null)
                    .SetModel(circleModel)
                    .SetMaterial(circleMaterials[level % circleMaterials.Length])
                    .SetRenderState(circleRenderState)
                    .SetTransform(Transform.Scaling(scale))
                    .SetTransformSpace(TransformSpace.ScreenAlighned)
            };

            var transform = Transform.Translation(new Vector3(springModel.GetPosition(index), 0));
            node.Transform = transform;
            
            if (level == 0)
            {
                var edgeVisuals = sg.Edges.Select(edge => CreateEdgeVisualElement(node, sg.NodeObjects[edge.First], sg.NodeObjects[edge.Second]));
                visualElems.AddRange(edgeVisuals);
            }

            if (sg.Children[index].Any())
            {
                foreach (var childIndex in sg.Children[index])
                    ArrangeAndDecorateInternal(childIndex, level + 1, springModel, sg);
            }

            dynamicParts.VisualElements = visualElems;
            dynamicParts.Hittable = new CircleHittable<ISceneNode>(node, Transform.Identity, x => new Circle2(Vector2.Zero, scale), x => -0.01f * level);
            dynamicParts.DefaultViewpointMechanism = new OrthoDefaultViewpointMechanism(node, new PlaneOrthoBoundControlledCamera.Props
            {
                Target = Vector2.Zero,
                Distance = 1.5f * scale,
                ZNear = 0.1f,
                ZFar = 1000f
            });
            aspect.SetDynamicParts(dynamicParts);
        }

        private IVisualElement CreateEdgeVisualElement(ISceneNode root, ISceneNode node1, ISceneNode node2)
        {
            var external = node1.ParentNode != node2.ParentNode;

            return external
                ? new ModelVisualElement<NestedCirclesStoryLayout>(this)
                    .SetModel(lineModel)
                    .SetMaterial(lineMaterialExternal)
                    .SetRenderState(lineRenderState)
                    .SetTransform(x => GetTransformBetweenNodes(root, node1, node2))
                : new ModelVisualElement<NestedCirclesStoryLayout>(this)
                    .SetModel(lineModel)
                    .SetMaterial(lineMaterial)
                    .SetRenderState(lineRenderState)
                    .SetTransform(x => GetTransformBetweenNodes(root, node1, node2));
        }

        private static Transform GetTransformBetweenNodes(ISceneNode root, ISceneNode node1, ISceneNode node2)
        {
            var point1 = GetTransfprmToIndirectParent(root, node1).Offset;
            var point2 = GetTransfprmToIndirectParent(root, node2).Offset;
            var dir = point2 - point1;
            var scale = dir.Length();
            var rotation = Quaternion.RotationToFrame(dir, Vector3.UnitZ);
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