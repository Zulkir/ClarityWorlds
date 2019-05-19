using System;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Core.AppCore.Views
{
    public class StoryPathCamera : ICamera
    {
        private readonly CameraProps initialCameraProps;
        private CameraProps? finalCameraProps;
        private readonly IStoryPath storyPath;
        private readonly Action onComplete;
        private float time;
        private float completeTime;
        private bool isComplete;
        private CameraProps visualProps;

        public StoryPathCamera(IStoryPath storyPath, CameraProps initialCameraProps, CameraProps? finalCameraProps, Action onComplete)
        {
            this.storyPath = storyPath;
            this.initialCameraProps = initialCameraProps;
            this.finalCameraProps = finalCameraProps;
            this.onComplete = onComplete;
            time = 0f;
        }

        public Color4 VeilColor { get; }

        public CameraProps GetProps() => visualProps;
        public Vector3 GetEye() => GetProps().Frame.Eye;
        public Vector3 GetTarget() => GetProps().Target;
        public Vector3 GetUp() => GetProps().Frame.Up;
        public float GetFov() => GetProps().Projection.Fov;
        public CameraFrame GetGlobalFrame() => GetProps().Frame;
        public CameraProjection GetProjectionProps() => GetProps().Projection;
        public Matrix4x4 GetViewMat() => GetGlobalFrame().GetViewMat();

        public Matrix4x4 GetProjectionMat(float aspectRatio)
        {
            // todo: check for ortho
            var projProps = GetProjectionProps();
            return Matrix4x4.PerspectiveFovDx(projProps.Fov, aspectRatio, projProps.ZNear, projProps.ZFar);
        }

        public Matrix4x4 GetViewProjInverse(float aspectRatio)
        {
            return (GetViewMat() * GetProjectionMat(aspectRatio)).Invert();
        }

        public Ray3 GetGlobalRayForHmgnPointer(float aspectRatio, Vector2 hmgnPointerPos)
        {
            // todo: implement for ortho
            var frame = GetGlobalFrame();
            var proj = GetProps().Projection;
            var halfHeight = proj.ZNear * MathHelper.Tan(proj.Fov / 2f);
            var halfWidth = halfHeight * aspectRatio;
            var worldPoint = frame.Eye + frame.Forward * proj.ZNear +
                             frame.Right * halfWidth * hmgnPointerPos.X +
                             frame.Up * halfHeight * hmgnPointerPos.Y;
            return new Ray3(worldPoint, worldPoint - frame.Eye);
        }

        public void Update(FrameTime frameTime)
        {
            if (isComplete)
                return;

            time += frameTime.DeltaSeconds;
            if (!storyPath.HasFinished)
            {
                storyPath.Update(frameTime);
                completeTime = Math.Max(1f, time + storyPath.MaxRemainingTime);
            }

            visualProps = storyPath.GetCurrentCameraProps();

            var acceleratingTime = MathHelper.Clamp(completeTime, 0f, 1f);
            if (time < acceleratingTime)
                visualProps = CameraProps.Lerp(initialCameraProps, visualProps, time / acceleratingTime);
            var actualFinalCameraProps = finalCameraProps ?? storyPath.GetCurrentCameraProps();
            var brakingTime = MathHelper.Clamp(completeTime - acceleratingTime, 0f, 1f);
            var remainingTime = brakingTime + completeTime - time;
            if (remainingTime < brakingTime)
                visualProps = CameraProps.Lerp(visualProps, actualFinalCameraProps, (brakingTime - remainingTime) / brakingTime);
            if (remainingTime < 0)
            {
                visualProps = actualFinalCameraProps;
                onComplete();
                isComplete = true;
            }
        }
    }
}