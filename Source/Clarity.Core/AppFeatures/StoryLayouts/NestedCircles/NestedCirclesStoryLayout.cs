using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.Coroutines;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppFeatures.StoryLayouts.NestedCircles
{
    public class NestedCirclesStoryLayout : IStoryLayout
    {
        public string UserFriendlyName => "Nested Circles";
        public Type Type => typeof(NestedCirclesStoryLayout);

        private readonly IInputService inputService;
        private readonly ICoroutineService coroutineService;
        private readonly IWorldTreeService worldTreeService;

        private readonly IFlexibleModel planeModel;
        private readonly IFlexibleModel circleModel;
        private readonly IFlexibleModel lineModel;
        private readonly IMaterial[] circleMaterials;
        private readonly IMaterial lineMaterial;
        private readonly IMaterial lineMaterialExternal;

        private const float ChildScale = 0.1f;
        private static readonly Vector2 DefaultEndYawPitch = new Vector2(MathHelper.Pi, 0);

        public NestedCirclesStoryLayout(IEmbeddedResources embeddedResources, IInputService inputService, ICoroutineService coroutineService, IWorldTreeService worldTreeService)
        {
            this.inputService = inputService;
            this.coroutineService = coroutineService;
            this.worldTreeService = worldTreeService;
            planeModel = embeddedResources.SimplePlaneXyModel();
            circleModel = embeddedResources.CircleModel(64);
            lineModel = embeddedResources.LineModel();
            circleMaterials = new IMaterial[]
            {
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1f, 0f, 0f) * 0.5f, 1.0f))) {IgnoreLighting = true, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0f, 1f, 0f) * 0.5f, 1.0f))) {IgnoreLighting = true, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0f, 0f, 1f) * 0.5f, 1.0f))) {IgnoreLighting = true, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1f, 1f, 0f) * 0.5f, 1.0f))) {IgnoreLighting = true, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(1f, 0f, 1f) * 0.5f, 1.0f))) {IgnoreLighting = true, LineWidth = 2},
                new StandardMaterial(new SingleColorPixelSource(new Color4(new Color3(0f, 1f, 1f) * 0.5f, 1.0f))) {IgnoreLighting = true, LineWidth = 2},
            };
            lineMaterial = new StandardMaterial(new SingleColorPixelSource(Color4.Blue)) {LineWidth = 3};
            lineMaterialExternal = new StandardMaterial(new SingleColorPixelSource(0.7f * Color4.White)) {LineWidth = 3};
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            var springModel = new NestedCirclesStorySpringModel(coroutineService, x => ArrangeAndDecorateInternal(sg.Root, 0, x, sg), sg);
            springModel.Apply();
            return new BasicStoryLayoutInstance(worldTreeService);
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
                new CgModelVisualElement<object>(null)
                    .SetModel(circleModel)
                    .SetMaterial(circleMaterials[level % circleMaterials.Length])
                    .SetTransform(Transform.Scaling(scale))
                    .SetCullFace(CgCullFace.Front)
                    .SetTransformSpace(CgTransformSpace.ScreenAlighned)
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
            else
            {
                visualElems.Add(CreateNumberRect(index, scale));
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
                ? new CgModelVisualElement<NestedCirclesStoryLayout>(this)
                    .SetModel(lineModel)
                    .SetMaterial(lineMaterialExternal)
                    .SetTransform(x => GetTransformBetweenNodes(root, node1, node2))
                : new CgModelVisualElement<NestedCirclesStoryLayout>(this)
                    .SetModel(lineModel)
                    .SetMaterial(lineMaterial)
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

        private IVisualElement CreateNumberRect(int number, float scale)
        {
            // todo: RichTextHelper
            var spanStyle = AmFactory.Create<RtSpanStyle>();
            spanStyle.Size = 30;
            spanStyle.FontFamily = "Arial";
            spanStyle.FontDecoration = FontDecoration.Bold;
            spanStyle.TextColor = Color4.Blue;
            var span = AmFactory.Create<RtSpan>();
            span.Text = number.ToString();
            span.Style = spanStyle;

            var paraStyle = AmFactory.Create<RtParagraphStyle>();
            paraStyle.Alignment = RtParagraphAlignment.Center;
            var para = AmFactory.Create<RtParagraph>();
            para.Spans.Add(span);
            para.Style = paraStyle;

            var textStyle = AmFactory.Create<RtOverallStyle>();
            textStyle.BackgroundColor = Color4.White;
            textStyle.TransparencyMode = RtTransparencyMode.Opaque;
            var text = AmFactory.Create<RichText>();
            text.Paragraphs.Add(para);
            text.Style = textStyle;

            var textBox = AmFactory.Create<RichTextBox>();
            textBox.Size = new IntSize2(96, 64);
            textBox.Text = text;
            
            var material = new StandardMaterial(new RichTextPixelSource(textBox))
            {
                IgnoreLighting = true
            };
            return new CgModelVisualElement<object>(null)
                .SetModel(planeModel)
                .SetMaterial(material)
                .SetNonUniformScale(new Vector3(scale / MathHelper.Sqrt2, scale / MathHelper.Sqrt2 / 96 * 64, 1));
        }
    }
}