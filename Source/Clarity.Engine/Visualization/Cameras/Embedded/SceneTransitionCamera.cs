using System;
using System.Diagnostics;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Visualization.Cameras.Embedded
{
    public class SceneTransitionCamera : ICamera
    {
        private readonly ICamera initialCamera;
        private readonly ICamera targetCamera;
        private readonly Action onMiddle;
        private readonly Action onFinish;
        private readonly float duration;
        private float currentSeconds;

        public SceneTransitionCamera(ICamera initialCamera, ICamera targetCamera, 
                                        float duration, Action onMiddle, Action onFinish)
        {
            Debug.Assert(initialCamera != null, "initialCamera != null");
            Debug.Assert(targetCamera != null, "targetCamera != null");
            this.initialCamera = initialCamera;
            this.targetCamera = targetCamera;
            this.duration = duration;
            this.onMiddle = onMiddle;
            this.onFinish = onFinish;
            currentSeconds = 0;
        }

        private float CurrentLerpAmount
        {
            get
            {
                var timeAmount = Math.Min(currentSeconds / duration, 1.0f);
                return timeAmount * timeAmount * (3f - 2f * timeAmount);
            }
        }

        private float CurrentVeilAmount => Math.Abs(0.5f - CurrentLerpAmount) * 2;

        private ICamera CurrentOtherCamera => CurrentLerpAmount < 0.5f ? initialCamera : targetCamera;

        public Color4 VeilColor => new Color4(1f, 1f, 1f, MathHelper.Clamp((1f - CurrentVeilAmount) * 1.25f, 0, 1));
        public CameraProps GetProps() => new CameraProps(GetTarget(), GetGlobalFrame(), GetProjectionProps());
        public Vector3 GetEye() => CurrentOtherCamera.GetEye();
        public Vector3 GetTarget() => CurrentOtherCamera.GetTarget();
        public Vector3 GetUp() => CurrentOtherCamera.GetUp();
        
        public CameraFrame GetGlobalFrame()
        {
            var eye = GetEye();
            var target = GetTarget();
            var up = GetUp();
            return new CameraFrame(eye, target - eye, up);
        }

        public CameraProjection GetProjectionProps() => CurrentOtherCamera.GetProjectionProps();

        public float GetFov() => 
            MathHelper.Lerp(MathHelper.Pi * 0.85f, CurrentOtherCamera.GetFov(), CurrentVeilAmount);

        public Matrix4x4 GetViewMat() => GetGlobalFrame().GetViewMat();

        public Matrix4x4 GetProjectionMat(float aspectRatio)
        {
            var fov = GetFov();
            var projProps = GetProjectionProps();
            return Matrix4x4.PerspectiveFovDx(fov, aspectRatio, projProps.ZNear, projProps.ZFar);
        }

        public Matrix4x4 GetViewProjInverse(float aspectRatio)
        {
            return (GetViewMat() * GetProjectionMat(aspectRatio)).Invert();
        }

        public Ray3 GetGlobalRayForHmgnPointer(float aspectRatio, Vector2 hmgnPointerPos)
        {
            var viewProjInv = GetViewProjInverse(aspectRatio);
            var rayStartW = new Vector4(hmgnPointerPos, 0, 1) * viewProjInv;
            var rayStart = (rayStartW / rayStartW.W).Xyz;
            var rayEndW = new Vector4(hmgnPointerPos, 1, 1) * viewProjInv;
            var rayEnd = (rayEndW / rayEndW.W).Xyz;
            return new Ray3(rayStart, rayEnd - rayStart);
        }

        public void Update(FrameTime frameTime)
        {
            var prevAmount = CurrentLerpAmount;
            currentSeconds += frameTime.DeltaSeconds;
            var newLerpAmount = CurrentLerpAmount;
            if (prevAmount < 0.5f && newLerpAmount >= 0.5f)
                onMiddle();
            if (currentSeconds >= duration)
                onFinish();
        }
    }
}