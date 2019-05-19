using System;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Visualization.Cameras.Embedded
{
    public class TransitionCamera : ICamera
    {
        private readonly ICamera initialCamera;
        private readonly ICamera targetCamera;
        private readonly Action onFinish;
        private readonly float targetSeconds;
        private float currentSeconds;

        public TransitionCamera(ICamera initialCamera, ICamera targetCamera, 
            float targetSeconds, Action onFinish)
        {
            this.initialCamera = initialCamera;
            this.targetCamera = targetCamera;
            this.targetSeconds = targetSeconds;
            this.onFinish = onFinish;
            currentSeconds = 0;
        }

        private float CurrentLerpAmount
        {
            get
            {
                var timeAmount = Math.Min(currentSeconds / targetSeconds, 1.0f);
                return timeAmount * timeAmount * (3f - 2f * timeAmount);
            }
        }

        public Color4 VeilColor => new Color4(0, 0, 0, 0);

        public CameraProps GetProps() => 
            new CameraProps(GetTarget(), GetGlobalFrame(), GetProjectionProps());

        public Vector3 GetEye() => 
            Vector3.Lerp(initialCamera.GetEye(), targetCamera.GetEye(), CurrentLerpAmount);

        public Vector3 GetTarget() => 
            Vector3.Lerp(initialCamera.GetTarget(), targetCamera.GetTarget(), CurrentLerpAmount);

        public Vector3 GetUp() => 
            Vector3.NLerp(initialCamera.GetUp(), targetCamera.GetUp(), CurrentLerpAmount);


        public CameraFrame GetGlobalFrame()
        {
            var eye = GetEye();
            var target = GetTarget();
            var up = GetUp();
            return new CameraFrame(eye, target - eye, up);
        }

        public CameraProjection GetProjectionProps() => 
            CameraProjection.Lerp(initialCamera.GetProjectionProps(), targetCamera.GetProjectionProps(), CurrentLerpAmount);

        public float GetFov()
        {
            return MathHelper.Lerp(initialCamera.GetFov(), targetCamera.GetFov(), CurrentLerpAmount);
        }
        
        public Matrix4x4 GetViewMat() { return GetGlobalFrame().GetViewMat(); }

        public Matrix4x4 GetProjectionMat(float aspectRatio)
        {
            //var fov = GetFov();
            //var projProps = GetProjectionProps();
            //return Matrix4x4.PerspectiveFovDx(fov, aspectRatio, projProps.ZNear, projProps.ZFar);
            var initialProj = initialCamera.GetProjectionMat(aspectRatio);
            var targetProj = targetCamera.GetProjectionMat(aspectRatio);
            return Matrix4x4.Lerp(initialProj, targetProj, CurrentLerpAmount);
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
            currentSeconds += frameTime.DeltaSeconds;
            if (currentSeconds >= targetSeconds)
                onFinish();
        }
    }
}