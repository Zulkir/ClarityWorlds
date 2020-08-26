using System;
using System.Linq;
using Clarity.App.Worlds.Interaction.Placement;
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

        private const float MaxAspectRatio = 2f;
        private const float HalfHeight = 1f;
        private const float HalfWidth = MaxAspectRatio * HalfHeight;
        private const float MinVerticalDistance = 2 * (HalfHeight * 1.1f);
        private const float MinHorizontalDistance = MaxAspectRatio * MinVerticalDistance;
        private const float MinRadiusDistance = 5f;

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
            ArrangeAndDecorateRoot(sg, sg.Root);
            return new BasicStoryLayoutInstance(sg);
        }

        private void ArrangeAndDecorateRoot(IStoryGraph sg, int nodeIndex)
        {
            var cStory = sg.Aspects[nodeIndex];
            var node = sg.NodeObjects[nodeIndex];
            var children = sg.Children[nodeIndex];
            var numChildren = children.Count;

            if (numChildren < 2)
            {
                var viewpointProps = new TargetedControlledCameraY.Props
                {
                    Target = Vector3.Zero,
                    Distance = MathHelper.FrustumDistance,
                    FieldOfView = MathHelper.PiOver4,
                    ZNear = 0.1f,
                    ZFar = 100.0f
                };

                cStory.SetDynamicParts(new StoryNodeDynamicParts
                {
                    DefaultViewpointMechanism = new WallDefaultViewpointMechanism(node, viewpointProps),
                    Hittable = new DummyHittable(),
                    VisualElements = new IVisualElement[0]
                });

                foreach (var child in children)
                    ArrangeAndDecorateRoot(sg, child);
            }
            else
            {
                var distr = CalculateDistribution(numChildren);
                var radius = distr.RelativeRadius;

                var oneMore = children.SelectMany(x => sg.Children[x]).Any();

                for (int i = 0; i < numChildren; i++)
                {
                    var pitch = distr.Angles[i].Pitch;
                    var yaw = distr.Angles[i].Yaw;

                    if (oneMore)
                    {
                        //var internalRadius = radius / 2;
                        ArrangeAndDecorateRoot(sg, children[i]);

                        //var yaw = yawPitchBounds.Center.X;
                        //var pitch = yawPitchBounds.Center.Y;
                        var pos = ToCartesian(yaw, pitch, radius * 20);
                        var zAxis = (-pos).Normalize();
                        var xAxis = Vector3.Cross(Vector3.UnitY, zAxis).Normalize();
                        var yAxis = Vector3.Cross(zAxis, xAxis);
                        var rotation = Quaternion.RotationToFrame(xAxis, yAxis);
                        sg.NodeObjects[children[i]].Transform = new Transform(1, rotation, pos);
                    
                        //ArrangeAndDecorateInternal(sg, children[i],
                        //    AaRectangle2.FromCenter(new Vector2(yaw, pitch), HalfWidth / radius, HalfHeight / radius),
                        //    radius);
                    }
                    else
                    {
                        ArrangeAndDecorateInternal(sg, children[i],
                            AaRectangle2.FromCenter(new Vector2(yaw, pitch), HalfWidth / radius, HalfHeight / radius),
                            radius);
                    }
                }

                var viewpointProps = new LookAroundCamera.Props
                {
                    Distance = distr.RelativeRadius,
                    FieldOfView = 0.75f * MathHelper.Pi,
                    ZNear = 0.1f,
                    ZFar = 100.0f,
                    Pitch = 0//MathHelper.PiOver2 + 0.1f
                };

                cStory.SetDynamicParts(new StoryNodeDynamicParts
                {
                    DefaultViewpointMechanism = new SphereDefaultViewpointMechanism(node, viewpointProps),
                    Hittable = new DummyHittable(),
                    VisualElements = new IVisualElement[0]
                });
            }
        }

        private static Vector3 ToCartesian(float yaw, float pitch, float radius)
        {
            var adjustedYaw = MathHelper.Pi - yaw;
            return radius * new Vector3(
               MathHelper.Sin(adjustedYaw) * MathHelper.Cos(pitch),
               MathHelper.Sin(pitch),
               MathHelper.Cos(adjustedYaw) * MathHelper.Cos(pitch));
        }

        private static SphereDistribution CalculateDistribution(int numChildren)
        {
            // todo: estimate good radius from number
            float radius = 2f;
            var angles = new PitchYaw[numChildren];
            while (!TryCalculateDistributionForRadius(radius, angles))
                radius *= 1.1f;
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
                ? MinHorizontalDistance * MathHelper.Asin(1f / bigRadius)
                : float.PositiveInfinity;
        }

        private static float PitchStep(float radius) =>
            MinVerticalDistance * MathHelper.Asin(1f / radius);

        private static float RadiusAt(float radius, float pitch) =>
            radius * MathHelper.Cos(pitch);
        
        private void ArrangeAndDecorateInternal(IStoryGraph sg, int nodeId, AaRectangle2 yawPitchBounds, float radius)
        {
            var cStory = sg.Aspects[nodeId];
            var node = sg.NodeObjects[nodeId];
            var children = sg.Children[nodeId];
            var numChildren = children.Count;

            var yaw = yawPitchBounds.Center.X;
            var pitch = yawPitchBounds.Center.Y;
            var pos = ToCartesian(yaw, pitch, radius);
            var zAxis = (-pos).Normalize();
            var xAxis = Vector3.Cross(Vector3.UnitY, zAxis).Normalize();
            var yAxis = Vector3.Cross(zAxis, xAxis);
            var rotation = Quaternion.RotationToFrame(xAxis, yAxis);
            node.Transform = new Transform(1, rotation, pos);

            var numRows = (int)Math.Ceiling(MathHelper.Sqrt(numChildren));
            var numCols = (int)Math.Ceiling((float)numChildren / numRows);
            var totalHeightRequired = MinVerticalDistance * numRows;
            var totalWidthRequired = MinHorizontalDistance * numRows;
            var minChildrenRadius = Math.Max(totalWidthRequired / yawPitchBounds.Width, totalHeightRequired / yawPitchBounds.Height);
            var childRadius = Math.Max(radius + MinRadiusDistance, minChildrenRadius);

            for (var i = 0; i < numChildren; i++)
            {
                var child = children[i];
                var row = i / numCols;
                var col = i % numCols;
                var childYawWidth = yawPitchBounds.Width / numCols;
                var childYawHeight = yawPitchBounds.Height / numRows;
                var childYaw = yawPitchBounds.MinX + childYawWidth * (row + 0.5f);
                var childPitch = yawPitchBounds.MinY + childYawHeight * (col * 0.5f);
                var childYawPitchBounds = AaRectangle2.FromCenter(new Vector2(childYaw, childPitch), HalfWidth / childRadius, HalfHeight / childRadius);
                ArrangeAndDecorateInternal(sg, child, childYawPitchBounds, childRadius);
            }
            
            var visuals = new []
            {
                ModelVisualElement.New(sg.Aspects[sg.Root])
                    .SetModel(frustumModel)
                    .SetMaterial(frustumMaterial)
                    .SetTransform(new Transform(0.5f, Quaternion.Identity, new Vector3(0, 0, 0.5f * MathHelper.FrustumDistance)))
                    .SetHide(x => !x.ShowAux1)
            };

            var viewpointProps = new TargetedControlledCameraY.Props
            {
                Target = Vector3.Zero,
                Distance = MathHelper.FrustumDistance,
                FieldOfView = MathHelper.PiOver4,
                ZNear = 0.1f,
                ZFar = 100.0f
            };

            var transform2D = new Transform(2, Quaternion.Identity, -MathHelper.FrustumDistance * Vector3.UnitZ);
            cStory.SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new WallDefaultViewpointMechanism(node, viewpointProps),
                Hittable = GetHittableComponent(node, transform2D),
                VisualElements = visuals,
                PlacementSurface2D = new PlanarPlacementSurface(node, Transform.Identity),
                PlacementSurface3D = new PlanarPlacementSurface(node, new Transform(0.05f, Quaternion.Identity, new Vector3(0, 0, 0.5f * MathHelper.FrustumDistance)))
            });
        }

        private static IRayHittable GetHittableComponent(ISceneNode layout, Transform planeTransform)
        {
            return new RectangleHittable<ISceneNodeBound>(
                layout, planeTransform, x => new AaRectangle2(Vector2.Zero, 1, 1), x => 0.0001f);
        }

        // todo: remove

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