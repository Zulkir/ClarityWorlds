using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Visualization.Cameras.Embedded
{
    // todo: remove?
    public class FixedCamera : IBoundControlledCamera
    {
        private readonly ISceneNode node;
        private readonly CameraFrame frame;
        private readonly float fov;
        private readonly float zNear;
        private readonly float zFar;

        public FixedCamera(ISceneNode node, Vector3 eye, Vector3 roughForward, Vector3 roughUp, float fov, float zNear, float zFar)
        {
            this.node = node;
            frame = new CameraFrame(eye, roughForward, roughUp);
            this.fov = fov;
            this.zNear = zNear;
            this.zFar = zFar;
        }

        public ISceneNode Node { get { return node; } }
        public CameraFrame GetLocalFrame() { return frame; }

        public CameraProjection GetProjectionProps()
        {
            return new CameraProjection(CameraProjectionType.Perspective, zNear, zFar, fov);
        }

        public Color4 VeilColor => new Color4(0, 0, 0, 0);
        public CameraProps GetProps() => new CameraProps(GetTarget(), GetGlobalFrame(), GetProjectionProps());
        public Vector3 GetEye() => GetGlobalFrame().Eye;
        public Vector3 GetTarget() => (frame.Eye + frame.Forward) * Node.GlobalTransform;
        public Vector3 GetUp() => GetGlobalFrame().Up;

        public CameraFrame GetGlobalFrame() { return frame * node.GlobalTransform; }
        public float GetFov() => fov;
        public Matrix4x4 GetViewMat() { return GetGlobalFrame().GetViewMat(); }

        public Matrix4x4 GetProjectionMat(float aspectRatio)
        {
            var scale = node.GlobalTransform.Scale;
            return Matrix4x4.PerspectiveFovDx(fov, aspectRatio, zNear * scale, zFar * scale);
        }

        public Matrix4x4 GetViewProjInverse(float aspectRatio) { return (GetViewMat() * GetProjectionMat(aspectRatio)).Invert();  }

        public Ray3 GetGlobalRayForHmgnPointer(float aspectRatio, Vector2 hmgnPointerPos)
        {
            var viewFrame = GetGlobalFrame();

            var halfHeight = zNear * MathHelper.Tan(fov / 2f);
            var halfWidth = halfHeight * aspectRatio;
            var worldPoint = viewFrame.Eye + viewFrame.Forward * zNear +
                viewFrame.Right * halfWidth * hmgnPointerPos.X +
                viewFrame.Up * halfHeight * hmgnPointerPos.Y;
            return new Ray3(worldPoint, worldPoint - viewFrame.Eye);
        }

        public void Update(FrameTime frameTime) { }
        public bool TryHandleInput(IInputEventArgs eventArgs) => false;

        public static FixedCamera Default(ISceneNode node)
        {
            var defaultViewFrame = CameraFrame.Default;
            return new FixedCamera(node, defaultViewFrame.Eye, defaultViewFrame.Forward, defaultViewFrame.Up, MathHelper.PiOver4, 0.1f, 100f);
        }
    }
}