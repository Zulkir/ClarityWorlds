using System;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Visualization.Cameras.Embedded
{
    public class LookAroundCamera : IBoundControlledCamera
    {
        #region Props struct
        public struct Props
        {
            public Vector3 Eye;
            public float Distance;
            public float Pitch;
            public float Yaw;
            public float FieldOfView;
            public float ZNear;
            public float ZFar;

            public Vector3 GetTarget() => Eye + Vector3.UnitY * Matrix3x3.RotationX(Pitch) * Matrix3x3.RotationZ(Yaw) * Distance;

            public CameraFrame GetFrame()
            {
                var viewFrame = new CameraFrame();
                viewFrame.Forward = Vector3.UnitY * Matrix3x3.RotationX(Pitch) * Matrix3x3.RotationZ(Yaw);
                viewFrame.Right = Vector3.Cross(viewFrame.Forward, Vector3.UnitZ).Normalize();
                viewFrame.Up = Vector3.Cross(viewFrame.Right, viewFrame.Forward).Normalize();
                viewFrame.Eye = Eye;//Target - Distance * viewFrame.Forward;
                return viewFrame;
            }

            public static Props Lerp(Props left, Props right, float amount)
            {
                return new Props
                {
                    Eye = Vector3.Lerp(left.Eye, right.Eye, amount),
                    Distance = MathHelper.Lerp(left.Distance, right.Distance, amount),
                    Pitch = MathHelper.Lerp(left.Pitch, right.Pitch, amount),
                    Yaw = LerpYaw(left.Yaw, right.Yaw, amount),
                    FieldOfView = MathHelper.Lerp(left.FieldOfView, right.FieldOfView, amount),
                    ZNear = MathHelper.Lerp(left.ZNear, right.ZNear, amount),
                    ZFar = MathHelper.Lerp(left.ZFar, right.ZFar, amount),
                };
            }

            private static float LerpYaw(float left, float right, float amount)
            {
                var diff = right - left;
                if (diff > MathHelper.Pi)
                    return MathHelper.Lerp(left + MathHelper.TwoPi, right, amount);
                if (diff < -MathHelper.Pi)
                    return MathHelper.Lerp(left - MathHelper.TwoPi, right, amount);
                return MathHelper.Lerp(left, right, amount);
            }

            public static Props Default => new Props
            {
                Eye = Vector3.Zero,
                Distance = 15f,
                Pitch = MathHelper.PiOver4,
                Yaw = MathHelper.PiOver4,
                FieldOfView = MathHelper.PiOver4,
                ZNear = 0.1f,
                ZFar = 100f
            };
        }
        #endregion

        private Props realProps;
        private Props visibleProps;
        public ISceneNode Node { get; }

        public LookAroundCamera(ISceneNode node, Props initialProps)
        {
            Node = node;
            realProps = visibleProps = initialProps;
        }

        public CameraFrame GetLocalFrame() => 
            visibleProps.GetFrame();

        public CameraProjection GetProjectionProps()
        {
            var zNear = visibleProps.ZNear;
            var zFar = visibleProps.ZFar;
            return new CameraProjection(CameraProjectionType.Perspective, zNear, zFar, visibleProps.FieldOfView);
        }

        public Color4 VeilColor => new Color4(0, 0, 0, 0);
        public CameraProps GetProps() => new CameraProps(GetTarget(), GetGlobalFrame(), GetProjectionProps());
        public Vector3 GetEye() { return GetGlobalFrame().Eye; }
        public Vector3 GetTarget() { return visibleProps.GetTarget() * Node.GlobalTransform; }
        public Vector3 GetUp() => GetGlobalFrame().Up;
        public float GetFov() { return visibleProps.FieldOfView; }

        public CameraFrame GetGlobalFrame() => 
            GetLocalFrame() * Node.GlobalTransform;

        public Matrix4x4 GetViewMat()
        {
            var frame = GetGlobalFrame();
            var eye = frame.Eye;
            return Matrix4x4.LookAtRh(eye, frame.Forward, Vector3.UnitZ);
        }

        public Matrix4x4 GetProjectionMat(float aspectRatio)
        {
            var transform = Node.GlobalTransform;
            var scaledZNear = visibleProps.ZNear * transform.Scale;
            var scaledZFar = visibleProps.ZFar * transform.Scale;
            return Matrix4x4.PerspectiveFovDx(visibleProps.FieldOfView, aspectRatio, scaledZNear, scaledZFar);
        }

        public Matrix4x4 GetViewProjInverse(float aspectRatio)
        {
            return (GetViewMat() * GetProjectionMat(aspectRatio)).Invert();
        }

        public Ray3 GetGlobalRayForHmgnPointer(float aspectRatio, Vector2 hmgnPointerPos)
        {
            var frame = GetLocalFrame();
            var halfHeight = visibleProps.ZNear * MathHelper.Tan(visibleProps.FieldOfView / 2f);
            var halfWidth = halfHeight * aspectRatio;
            var worldPoint = frame.Eye + frame.Forward * visibleProps.ZNear +
                             frame.Right * halfWidth * hmgnPointerPos.X +
                             frame.Up * halfHeight * hmgnPointerPos.Y;
            return new Ray3(worldPoint, worldPoint - frame.Eye) * Node.GlobalTransform;
        }

        public bool TryHandleInput(IInputEventArgs eventArgs)
        {
            if (!(eventArgs is IMouseEventArgs mouseArgs))
                return false;
            if (mouseArgs.ComplexEventType == MouseEventType.Move)
            {
                if (mouseArgs.State.Buttons == MouseButtons.Left)
                {
                    var scale = 0.01f;
                    realProps.Yaw += scale * mouseArgs.Delta.X;
                    realProps.Pitch += scale * mouseArgs.Delta.Y;
                    realProps.Pitch = MathHelper.Clamp(realProps.Pitch, -MathHelper.PiOver2 + 1e-5f, MathHelper.PiOver2 - 1e-5f);
                    return true;
                }
                if (mouseArgs.State.Buttons == MouseButtons.Right)
                {
                    var viewFrame = realProps.GetFrame();
                    var scale = 0.00115f * realProps.Distance;
                    var offset = -viewFrame.Right * scale * mouseArgs.Delta.X + viewFrame.Up * scale * mouseArgs.Delta.Y;
                    realProps.Eye += offset;
                    return true;
                }
            }
            
            if (mouseArgs.ComplexEventType == MouseEventType.Wheel)
            {
                var scale = 0.1f;
                realProps.FieldOfView *= (1f - scale * mouseArgs.WheelDelta);
                //realProps.Distance *= (1f - scale * eventArgs.WheelDelta);
                return true;
            }

            return false;
        }

        public void Update(FrameTime frameTime)
        {
            var amount = Math.Min(20f * frameTime.DeltaSeconds, 1f);
            visibleProps = Props.Lerp(visibleProps, realProps, amount);
        }

        public static LookAroundCamera Default(ISceneNode node)
        {
            return new LookAroundCamera(node, Props.Default);
        }
    }
}