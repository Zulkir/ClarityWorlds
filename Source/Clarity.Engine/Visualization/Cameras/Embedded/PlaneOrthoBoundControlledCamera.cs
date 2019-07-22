using System;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Visualization.Cameras.Embedded
{
    public class PlaneOrthoBoundControlledCamera : IBoundControlledCamera
    {
        #region Props struct
        public struct Props
        {
            public Vector2 Target;
            public float Distance;
            public float ZNear;
            public float ZFar;

            public Vector3 GetTarget3() => new Vector3(Target, 0);
            public float GetScale() => Distance * MathHelper.Tan(MathHelper.PiOver4 / 2);

            public Vector3 GetEye()
            {
                return GetTarget3() + Distance * Vector3.UnitZ;
            }

            public CameraFrame GetFrame()
            {
                return new CameraFrame
                {
                    Forward = -Vector3.UnitZ,
                    Right = Vector3.UnitX,
                    Up = Vector3.UnitY,
                    Eye = GetEye()
                };
            }

            public static Props Lerp(Props left, Props right, float amount)
            {
                return new Props
                {
                    Target = Vector2.Lerp(left.Target, right.Target, amount),
                    Distance = MathHelper.Lerp(left.Distance, right.Distance, amount),
                    ZNear = MathHelper.Lerp(left.ZNear, right.ZNear, amount),
                    ZFar = MathHelper.Lerp(left.ZFar, right.ZFar, amount),
                };
            }

            public static Props Default => new Props
            {
                Target = Vector2.Zero,
                Distance = 1f / MathHelper.Tan(MathHelper.Pi / 8),
                ZNear = 0.1f,
                ZFar = 1000f
            };
        }
        #endregion

        public struct CameraBounds
        {
            public AaRectangle2 PlaneBounds;
            public float MinDistance;
            public float MaxDistance;
        }

        private readonly ISceneNode node;
        private Props realProps;
        private Props visibleProps;

        public ISceneNode Node { get { return node; } }
        public bool AcceptsInput { get; }
        public CameraBounds Bounds { get; private set; }

        public PlaneOrthoBoundControlledCamera(ISceneNode node, Props initialProps, bool acceptsInput, CameraBounds? bounds = null)
        {
            this.node = node;
            AcceptsInput = acceptsInput;
            realProps = visibleProps = initialProps;
            Bounds = bounds ?? new CameraBounds
            {
                PlaneBounds = new AaRectangle2(Vector2.Zero, float.MaxValue / 2, float.MaxValue / 2),
                MaxDistance = float.MaxValue,
                MinDistance = 0
            };
        }

        public CameraFrame GetLocalFrame() { return visibleProps.GetFrame(); }

        public CameraProjection GetProjectionProps()
        {
            var zNear = visibleProps.ZNear;
            var zFar = visibleProps.ZFar;
            var scale = visibleProps.GetScale();
            return new CameraProjection(CameraProjectionType.Orthographic, zNear, zFar, scale);
        }

        public Color4 VeilColor => new Color4(0, 0, 0, 0);
        public CameraProps GetProps() => new CameraProps(GetTarget(), GetGlobalFrame(), GetProjectionProps());
        public Vector3 GetEye() => GetGlobalFrame().Eye;
        public Vector3 GetTarget() => visibleProps.GetTarget3() * Node.GlobalTransform;
        public Vector3 GetUp() => GetGlobalFrame().Up;
        public CameraFrame GetGlobalFrame() => GetLocalFrame() * Node.GlobalTransform;
        public float GetFov() { return MathHelper.PiOver4; }

        public Matrix4x4 GetViewMat()
        {
            return GetGlobalFrame().GetViewMat();
        }

        public Matrix4x4 GetProjectionMat(float aspectRatio)
        {
            var transform = Node.GlobalTransform;
            var scaledZNear = visibleProps.ZNear * transform.Scale;
            var scaledZFar = visibleProps.ZFar * transform.Scale;
            return Matrix4x4.OrthoScaleDx(visibleProps.GetScale(), aspectRatio, scaledZNear, scaledZFar);
        }

        public Matrix4x4 GetViewProjInverse(float aspectRatio)
        {
            return (GetViewMat() * GetProjectionMat(aspectRatio)).Invert();
        }

        public Ray3 GetGlobalRayForHmgnPointer(float aspectRatio, Vector2 hmgnPointerPos)
        {
            var frame = GetLocalFrame();
            var halfHeight = visibleProps.GetScale();
            var halfWidth = halfHeight * aspectRatio;
            var worldPoint = frame.Eye + frame.Forward * visibleProps.ZNear +
                             frame.Right * halfWidth * hmgnPointerPos.X +
                             frame.Up * halfHeight * hmgnPointerPos.Y;
            return new Ray3(worldPoint, frame.Forward) * Node.GlobalTransform;
        }

        public bool TryHandleInput(IInputEvent eventArgs)
        {
            if (!AcceptsInput)
                return false;
            if (eventArgs is IMouseEvent mouseArgs)
                return TryHandleMouse(mouseArgs);
            return false;
        }

        private bool TryHandleMouse(IMouseEvent eventArgs)
        {
            if (eventArgs.ComplexEventType == MouseEventType.Move)
            {
                if (eventArgs.State.Buttons == MouseButtons.Right && eventArgs.KeyModifyers == KeyModifyers.None)
                {
                    var scale = 0.00925f * realProps.GetScale();
                    var offset = -Vector2.UnitX * scale * eventArgs.Delta.X + Vector2.UnitY * scale * eventArgs.Delta.Y;
                    realProps.Target += offset;
                    EnforceBounds();
                    return true;
                }
            }
            else if (eventArgs.ComplexEventType == MouseEventType.Wheel)
            {
                var scale = 0.1f;
                var distanceScale = 1f - scale * eventArgs.WheelDelta;
                if (eventArgs.KeyModifyers == KeyModifyers.None)
                    realProps.Distance *= distanceScale;
                EnforceBounds();
                return true;
            }

            EnforceBounds();
            return false;
        }

        public void Update(FrameTime frameTime)
        {
            var amount = Math.Min(20f * frameTime.DeltaSeconds, 1f);
            visibleProps = Props.Lerp(visibleProps, realProps, amount);
        }

        public void SetProperties(Props props) => realProps = props;
        public void SetVisualToReal() => visibleProps = realProps;
        public void SetBounds(CameraBounds bounds) => Bounds = bounds;

        private void EnforceBounds()
        {
            CodingHelper.ForceIntoBounds(ref realProps.Target.X, Bounds.PlaneBounds.MinX, Bounds.PlaneBounds.MaxX);
            CodingHelper.ForceIntoBounds(ref realProps.Target.Y, Bounds.PlaneBounds.MinY, Bounds.PlaneBounds.MaxY);
            CodingHelper.ForceIntoBounds(ref realProps.Distance, Bounds.MinDistance, Bounds.MaxDistance);
        }

        public static PlaneOrthoBoundControlledCamera Default(ISceneNode node)
        {
            return new PlaneOrthoBoundControlledCamera(node, Props.Default, true);
        }
    }
}