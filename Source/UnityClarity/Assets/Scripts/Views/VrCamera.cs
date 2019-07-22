using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Assets.Scripts.Rendering;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using Matrix4x4 = Clarity.Common.Numericals.Algebra.Matrix4x4;
using Vector2 = Clarity.Common.Numericals.Algebra.Vector2;
using Vector3 = Clarity.Common.Numericals.Algebra.Vector3;

namespace Assets.Scripts.Views
{
    public class VrCamera : IControlledCamera, IFromUnityCamera
    {
        private readonly IGlobalObjectService globalObjectService;
        private CameraProps props;

        public VrCamera(IGlobalObjectService globalObjectService)
        {
            this.globalObjectService = globalObjectService;
        }

        public Color4 VeilColor { get; }

        public CameraProps GetProps() => props;
        public Vector3 GetEye() => props.Frame.Eye;
        public Vector3 GetTarget() => props.Target;
        public Vector3 GetUp() => props.Frame.Up;
        public float GetFov() => props.Projection.Fov;
        public CameraFrame GetGlobalFrame() => props.Frame;
        public CameraProjection GetProjectionProps() => props.Projection;
        public Matrix4x4 GetViewMat() => props.Frame.GetViewMat();
        public Matrix4x4 GetProjectionMat(float aspectRatio) =>
            Matrix4x4.PerspectiveFovDx(props.Projection.Fov, aspectRatio, props.Projection.ZNear, props.Projection.ZFar);
        public Matrix4x4 GetViewProjInverse(float aspectRatio) => (GetViewMat() * GetProjectionMat(aspectRatio)).Invert();

        public Ray3 GetGlobalRayForHmgnPointer(float aspectRatio, Vector2 hmgnPointerPos)
        {
            var frame = GetGlobalFrame();
            var proj = GetProps().Projection;
            var halfHeight = proj.ZNear * MathHelper.Tan(proj.Fov / 2f);
            var halfWidth = halfHeight * aspectRatio;
            var worldPoint = frame.Eye + frame.Forward * proj.ZNear +
                             frame.Right * halfWidth * hmgnPointerPos.X +
                             frame.Up * halfHeight * hmgnPointerPos.Y;
            return new Ray3(worldPoint, worldPoint - frame.Eye);
        }

        public bool TryHandleInput(IInputEventArgs eventArgs)
        {
            return false;
        }

        public void Update(FrameTime frameTime)
        {
            var camera = globalObjectService.MainCamera;
            var frame = new CameraFrame(camera.transform.position.ToClarity(), camera.transform.rotation.ToClarity());
            var proj = new CameraProjection(CameraProjectionType.Perspective, camera.nearClipPlane, camera.farClipPlane, camera.fieldOfView);
            props = new CameraProps(frame.Eye + frame.Forward, frame, proj);

        }

        public void TeleportTo(CameraProps newProps, float? floorHeight)
        {
            var player = globalObjectService.VrPlayerCarrier;
            var eye = newProps.Frame.Eye;
            if (floorHeight.HasValue)
                eye.Y = floorHeight.Value;
            player.transform.position = eye.ToUnity();
            player.transform.rotation = newProps.Frame.GetRotation().ToUnity();
        }
    }
}