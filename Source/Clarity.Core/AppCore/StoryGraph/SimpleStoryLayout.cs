using System;
using System.Linq;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;

namespace Clarity.Core.AppCore.StoryGraph
{
    public class SimpleStoryLayout : IStoryLayout
    {
        public string UserFriendlyName => "Simple";
        public Type Type => typeof(SimpleStoryLayout);

        private readonly IWorldTreeService worldTreeService;
        private readonly IKeyboardInputProvider keyboardInputProvider;
        private readonly IFlexibleModel planeModel;
        private readonly IStandardMaterial[] planeMaterials;

        public SimpleStoryLayout(IKeyboardInputProvider keyboardInputProvider, IEmbeddedResources embeddedResources, IWorldTreeService worldTreeService)
        {
            this.keyboardInputProvider = keyboardInputProvider;
            this.worldTreeService = worldTreeService;

            planeModel = embeddedResources.SimplePlaneXyModel();
            planeMaterials = new[]
                {
                    new Color4(120, 222, 44),
                    new Color4(222, 120, 44),
                    new Color4(44, 120, 222),
                    new Color4(222, 44, 120),
                    new Color4(64, 222, 160),
                    new Color4(120, 44, 222),
                }.Select(x => new StandardMaterial(new SingleColorPixelSource(x)))
                .Select(x => (IStandardMaterial)x)
                .ToArray();
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            ArrangeAndDecorateInternal(sg, sg.Root, sg.Depth);
            return new BasicStoryLayoutInstance(worldTreeService);
        }

        private void ArrangeAndDecorateInternal(IStoryGraph sg, int nodeIndex, int scaleLevel)
        {
            var aStory = sg.Aspects[nodeIndex];
            var node = sg.NodeObjects[nodeIndex];
            var abstractChildren = sg.Children[nodeIndex];
            var numChildren = abstractChildren.Count;

            var scale = MathHelper.Pow(4, scaleLevel);
            
            var visuals = new [] 
            {
                new CgModelVisualElement()
                    .SetModel(planeModel)
                    .SetMaterial(planeMaterials[scaleLevel % planeMaterials.Length])
                    .SetTransform(Transform.Scaling(10 * scale))
            };

            var viewpointProps = new FreeControlledCamera.Props
            {
                Eye = Vector3.UnitZ * 10f * scale,
                Yaw = MathHelper.PiOver4,
                Pitch = MathHelper.PiOver4,
                FieldOfView = MathHelper.PiOver4,
                ZNear = 0.1f * scale,
                ZFar = 100.0f * scale
            };

            for (int i = 0; i < numChildren; i++)
            {
                var adaptiveChild = abstractChildren[i];
                var angle = MathHelper.TwoPi * i / numChildren - MathHelper.PiOver4;
                var rotation = Quaternion.RotationZ(angle);
                var radius = scale * GetBestRadius(numChildren);

                var offset = new Vector3(radius * MathHelper.Sin(angle), radius * MathHelper.Cos(angle), 0.1f * scale);
                sg.NodeObjects[adaptiveChild].Transform = new Transform(1, rotation, offset);
                ArrangeAndDecorateInternal(sg, adaptiveChild, scaleLevel - 1);
            }

            aStory.SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new FreeLandDefaultViewpointMechanism(node, viewpointProps, keyboardInputProvider),
                Hittable = GetHittableComponent(node),
                VisualElements = visuals
            });
        }

        private static IRayHittable GetHittableComponent(ISceneNode layout)
        {
            return new RectangleHittable<ISceneNodeBound>(
                layout, Transform.Identity, x => new AaRectangle2(Vector2.Zero, 1, 1), x => 0);
        }

        private static Color4 ColorForScale(int scaleLevel)
        {
            switch (scaleLevel)
            {
                case 0: return new Color4(120, 222, 44);
                case 1: return new Color4(222, 120, 44);
                case 2: return new Color4(44, 120, 222);
                case 3: return new Color4(222, 44, 120);
                case 4: return new Color4(64, 222, 160);
                case 5: return new Color4(120, 44, 222);
                default: return ColorForScale(scaleLevel % 6);
            }
        }

        private static float GetBestRadius(int numChildren)
        {
            switch (numChildren)
            {
                case 1: return 0f;
                case 2: return 5f;
                case 3: return 5.5f;
                case 4: return 6f;
                case 5: return 6.5f;
                case 6: return 7.0f;
                case 7: return 7.5f;
                case 8: return 8f;
                default: return 8.5f;
            }
        }
    }
}