using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.StoryLayout.Building
{
    public class BuildingFreeCamera : IControlledCamera
    {
        private struct Props
        {
            public Vector3 Eye;
            public float Pitch;
            public float Yaw;

            public CameraFrame GetFrame()
            {
                var forwardPitched = -Vector3.UnitZ * Quaternion.RotationX(Pitch);
                var forward = (forwardPitched * Quaternion.RotationY(Yaw)).Normalize();
                var right = Vector3.Cross(forward, Vector3.UnitY).Normalize();
                var up = Vector3.Cross(right, forward);
                return new CameraFrame(Eye, forward, up, right);
            }

            public static Props FromCommonProps(CameraFrame frame)
            {
                var forward = frame.Forward;
                var yaw = MathHelper.Atan2(forward.Xz.X, -forward.Xz.Y);
                var pitch = MathHelper.Acos(Vector3.Dot(new Vector3(forward.X, 0, forward.Z).Normalize(), forward));
                return new Props
                {
                    Eye = frame.Eye,
                    Yaw = yaw,
                    Pitch = pitch
                };
            }
        }

        private Props props;
        private readonly IInputService inputService;
        private readonly ICollisionMesh collisionMesh;
        private readonly IStoryLayoutZoning zoning;
        private Vector3 horrVelocity;
        private float vertVelocity;

        private const float Speed = 10f;
        private const float Accel = 180f;
        private const float MaxVelocity = Speed;
        private const float MaxVelocitySq = MaxVelocity * MaxVelocity;
        private const float MouseSensitivity = 0.01f;
        private const float JumpVelocity = 5f;

        public BuildingFreeCamera(CameraProps props, IInputService inputService, ICollisionMesh collisionMesh, IStoryLayoutZoning zoning)
        {
            this.props = Props.FromCommonProps(props.Frame);
            this.inputService = inputService;
            this.collisionMesh = collisionMesh;
            this.zoning = zoning;
        }

        public Color4 VeilColor { get; }

        public CameraProps GetProps()
        {
            var frame = props.GetFrame();
            return new CameraProps(frame.Eye + frame.Forward, frame, new CameraProjection(CameraProjectionType.Perspective, 0.1f, 100f, MathHelper.PiOver4));
        }

        public Vector3 GetEye() => GetProps().Frame.Eye;
        public Vector3 GetTarget() => GetProps().Target;
        public Vector3 GetUp() => GetProps().Frame.Up;
        public float GetFov() => GetProps().Projection.Fov;
        public CameraFrame GetGlobalFrame() => GetProps().Frame;
        public CameraProjection GetProjectionProps() => GetProps().Projection;
        public Matrix4x4 GetViewMat() => GetGlobalFrame().GetViewMat();

        public Matrix4x4 GetProjectionMat(float aspectRatio)
        {
            var projProps = GetProjectionProps();
            return Matrix4x4.PerspectiveFovDx(projProps.Fov, aspectRatio, projProps.ZNear, projProps.ZFar);
        }

        public Matrix4x4 GetViewProjInverse(float aspectRatio)
        {
            return (GetViewMat() * GetProjectionMat(aspectRatio)).Invert();
        }

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

        public void Update(FrameTime frameTime)
        {
            var frame = GetGlobalFrame();
            var eye = frame.Eye;
            var planarForward = new Vector3(frame.Forward.X, 0, frame.Forward.Z).Normalize();
            var planarRight = Vector3.Cross(planarForward, Vector3.UnitY);

            var accel = Vector3.Zero;
            if (inputService.CurrentKeyboardState.IsKeyPressed(Key.W))
                accel += planarForward * Accel;
            if (inputService.CurrentKeyboardState.IsKeyPressed(Key.S))
                accel -= planarForward * Accel;
            if (inputService.CurrentKeyboardState.IsKeyPressed(Key.D))
                accel += planarRight * Accel;
            if (inputService.CurrentKeyboardState.IsKeyPressed(Key.A))
                accel -= planarRight * Accel;
            if (accel.LengthSquared() >= MathHelper.Eps8)
                accel = accel.Normalize() * Accel;

            if (accel.X == 0)
                horrVelocity.X *= 0.7f;
            if (accel.Z == 0)
                horrVelocity.Z *= 0.7f;

            horrVelocity += new Vector3(accel.X, 0, accel.Z) * frameTime.DeltaSeconds;
            vertVelocity += accel.Y;

            if (horrVelocity.LengthSquared() > MaxVelocitySq)
                horrVelocity = horrVelocity.Normalize() * MaxVelocity;

            // todo: check that the horizontal speed is limited by MaxVelocity

            var gravity = zoning.GetZonePropertiesAt(eye).Gravity;
            if (gravity != 0)
            {
                if (vertVelocity == 0 && inputService.CurrentKeyboardState.IsKeyPressed(Key.Space))
                    vertVelocity = JumpVelocity;
                else
                    vertVelocity += gravity * frameTime.DeltaSeconds;
            }
            else
            {
                if (inputService.CurrentKeyboardState.IsKeyPressed(Key.Space))
                    vertVelocity += Accel;
                else if (inputService.CurrentKeyboardState.IsKeyPressed(Key.C))
                    vertVelocity -= Accel;
                else
                    vertVelocity *= 0.7f;

                if (vertVelocity > MaxVelocity)
                    vertVelocity = MaxVelocity;
                if (vertVelocity < -MaxVelocity)
                    vertVelocity = -MaxVelocity;
            }

            var velocity = new Vector3(horrVelocity.X, vertVelocity, horrVelocity.Z);
            var newPos = collisionMesh.RestrictMovement(0.8f, eye, velocity * frameTime.DeltaSeconds);
            velocity = (newPos - eye) / frameTime.DeltaSeconds;
            vertVelocity = velocity.Y;
            horrVelocity = new Vector3(velocity.X, 0, velocity.Z);
            props.Eye = newPos;
        }

        public bool TryHandleInput(IInputEventArgs eventArgs)
        {
            if (eventArgs is IMouseEventArgs margs)
            {
                if (margs.IsOfType(MouseEventType.Move))
                {
                    props.Yaw += margs.Delta.X * MouseSensitivity;
                    props.Pitch += margs.Delta.Y * MouseSensitivity;
                    if (props.Pitch > 1f)
                        props.Pitch = 1f;
                    if (props.Pitch < -1f)
                        props.Pitch = -1f;
                    return true;
                }
            }
            return false;
        }
    }
}