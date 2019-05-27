using System;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Models;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Materials;

namespace Clarity.App.Worlds.StoryLayouts.Sphere
{
    public class SphereStoryLayout : IStoryLayout
    {
        public string UserFriendlyName => "Sphere";
        public Type Type => typeof(SphereStoryLayout);

        private struct PitchYaw
        {
            public float Pitch;
            public float Yaw;

            public PitchYaw(float pitch, float yaw)
            {
                Pitch = pitch;
                Yaw = yaw;
            }
        }

        private struct SphereDistribution
        {
            public float RelativeRadius;
            public PitchYaw[] Angles;

            public SphereDistribution(float relativeRadius, PitchYaw[] angles)
            {
                RelativeRadius = relativeRadius;
                Angles = angles;
            }
        }

        private readonly IModel3D frustumModel;
        private readonly IStandardMaterial frustumMaterial;

        public SphereStoryLayout(IEmbeddedResources embeddedResources)
        {
            frustumModel = embeddedResources.SimpleFrustumModel();
            frustumMaterial = StandardMaterial.New()
                .SetDiffuseColor(Color4.Green)
                .SetIgnoreLighting(true)
                .FromGlobalCache();
        }

        public IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg)
        {
            ArrangeAndDecorateRoot(sg, sg.Root, sg.Depth);
            return new BasicStoryLayoutInstance(sg);
        }

        private void ArrangeAndDecorateRoot(IStoryGraph sg, int nodeIndex, int scaleLevel)
        {
            var aStory = sg.Aspects[nodeIndex];
            var node = sg.NodeObjects[nodeIndex];
            var abstractChildren = sg.Children[nodeIndex];
            var numChildren = abstractChildren.Count;

            var scale = MathHelper.Pow(4, scaleLevel - 1);

            var distr = CalculateDistribution(numChildren);
            var radius = distr.RelativeRadius * scale;
            
            for (int i = 0; i < numChildren; i++)
            {
                var pitch = distr.Angles[i].Pitch;
                var yaw = -distr.Angles[i].Yaw;
                //var internalRadius = radius / 2;

                var pos = radius * new Vector3(
                    MathHelper.Cos(yaw) * MathHelper.Cos(pitch),
                    MathHelper.Sin(yaw) * MathHelper.Cos(pitch),
                    MathHelper.Sin(pitch));
                var zAxis = (-pos).Normalize();
                var xAxis = Vector3.Cross(Vector3.UnitZ, zAxis).Normalize();
                var yAxis = Vector3.Cross(zAxis, xAxis);

                var rotation = Quaternion.RotationToFrame(xAxis, yAxis);

                var adaptiveChild = abstractChildren[i];
                sg.NodeObjects[adaptiveChild].Transform = new Transform(scale / MathHelper.Sqrt(2), rotation, pos);
                ArrangeAndDecorateInternal(sg, adaptiveChild, scaleLevel - 1);
            }

            var viewpointProps = new LookAroundCamera.Props
            {
                Distance = 24 * scale,
                FieldOfView = 0.75f * MathHelper.Pi,
                ZNear = 0.1f * scale,
                ZFar = 100.0f * scale,
                Pitch = 0//MathHelper.PiOver2 + 0.1f
            };

            aStory.SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new SphereDefaultViewpointMechanism(node, viewpointProps),
                Hittable = new DummyHittable(),
                VisualElements = new IVisualElement[0]
            });
        }

        private static SphereDistribution CalculateDistribution(int numChildren)
        {
            float radius = 2f;
            var angles = new PitchYaw[numChildren];
            while (!TryCalculateDistributionForRadius(radius, angles))
                radius += 0.1f;
            return new SphereDistribution(radius, angles);
        }

        private static bool TryCalculateDistributionForRadius(float radius, PitchYaw[] anglesToFill)
        {
            var numChildren = anglesToFill.Length;
            if (numChildren == 0)
                return true;

            var middleIndex = numChildren / 2 - 1 + numChildren % 2;

            var currentIndex = middleIndex;
            var pitch = 0f;

            while (currentIndex < numChildren)
            {
                var horizontalCapacity = HorizontalCapacity(radius, pitch);
                var pitchStep = PitchStep(radius);
                var nextPitch = pitch + pitchStep;
                if (nextPitch + pitchStep / 2 > 0.66f * MathHelper.PiOver2)
                    return false;
                
                var localPitchStep = pitchStep / horizontalCapacity;
                var yawStep = YawStep(radius, pitch);
                var localBorder = Math.Min(currentIndex + horizontalCapacity, numChildren);

                if (currentIndex == middleIndex)
                {
                    anglesToFill[currentIndex] = new PitchYaw(0, 0);
                    currentIndex++;
                }

                for (int i = currentIndex; i < localBorder; i++)
                    anglesToFill[i] = new PitchYaw(anglesToFill[i - 1].Pitch + localPitchStep, anglesToFill[i - 1].Yaw + yawStep);
                currentIndex = localBorder;
                pitch = nextPitch;
            }

            for (int i = middleIndex - 1; i >= 0; i--)
            {
                var opposite = anglesToFill[middleIndex + (middleIndex - i)];
                anglesToFill[i] = new PitchYaw(-opposite.Pitch, -opposite.Yaw);
            }

            return true;
        }

        private static int HorizontalCapacity(float radius, float pitch) => 
            (int)(MathHelper.TwoPi / YawStep(radius, pitch));

        private static float YawStep(float radius, float pitch)
        {
            var bigRadius = RadiusAt(radius, pitch + PitchStep(radius));
            return bigRadius > 1 
                ? 1.8f * 2 * MathHelper.Asin(1f / bigRadius)
                : float.PositiveInfinity;
        }

        private static float PitchStep(float radius) =>
            1.2f * 2 * MathHelper.Asin(1f / radius);

        private static float RadiusAt(float radius, float pitch) =>
            radius * MathHelper.Cos(pitch);
        
        private void ArrangeAndDecorateInternal(IStoryGraph sg, int nodeIndex, int scaleLevel)
        {
            var aStory = sg.Aspects[nodeIndex];
            var node = sg.NodeObjects[nodeIndex];
            var abstractChildren = sg.Children[nodeIndex];
            var numChildren = abstractChildren.Count;

            var halfSize = 1f;
            var fov = MathHelper.PiOver4;
            var distance = halfSize / MathHelper.Tan(fov / 2);

            for (int i = 0; i < numChildren; i++)
            {
                var adaptiveChild = abstractChildren[i];
                //var angle = MathHelper.TwoPi * i / abstractChildren.Length - MathHelper.PiOver4;
                //var rotation = Quaternion.Identity;
                //var radius = halfSize * GetBestRadius(abstractChildren.Length);
                //var offset = new Vector3(radius * MathHelper.Sin(angle), radius * MathHelper.Cos(angle), 0);

                var rotation = Quaternion.Identity;
                var middleIndex = (numChildren - 1) / 2f;
                var offset = new Vector3(20 * (i - middleIndex) / numChildren, -8, 0);

                sg.NodeObjects[adaptiveChild].Transform = new Transform(1f, rotation, offset);
                ArrangeAndDecorateInternal(sg, adaptiveChild, scaleLevel - 1);
            }
            
            var visuals = new []
            {
                ModelVisualElement.New().SetModel(frustumModel).SetMaterial(frustumMaterial)
            };

            var viewpointProps = new TargetedControlledCameraY.Props
            {
                Target = Vector3.Zero,
                Distance = distance,
                FieldOfView = fov,
                ZNear = 0.1f * distance,
                ZFar = 100.0f * distance
            };

            var transform2D = new Transform(2, Quaternion.Identity, -distance * Vector3.UnitZ);
            aStory.SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new WallDefaultViewpointMechanism(node, viewpointProps),
                Hittable = GetHittableComponent(node, transform2D),
                VisualElements = visuals
            });
        }

        private static IRayHittable GetHittableComponent(ISceneNode layout, Transform planeTransform)
        {
            return new RectangleHittable<ISceneNodeBound>(
                layout, planeTransform, x => new AaRectangle2(Vector2.Zero, 1, 1), x => 0.0001f);
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
            return numChildren <= 1
                ? 0f
                : 5f + 0.5f * (numChildren - 2);
        }
    }
}