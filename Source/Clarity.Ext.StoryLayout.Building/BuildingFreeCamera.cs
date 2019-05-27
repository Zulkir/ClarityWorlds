using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.StoryGraph;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
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

        private struct WallIntersection
        {
            public LineSegment3 Wall;
            public float Distance;
        }

        private Props props;
        private readonly IInputService inputService;
        private readonly BuildingStoryLayoutPlacementAlgorithm pa;
        private readonly IStoryGraph sg;
        private readonly List<BuildingWallSegment> globalWallSegments;
        private Vector3 horrVelocity;
        private float vertVelocity;

        private const float Speed = 10f;
        private const float Gravity = 15f;
        private const float Accel = 3f;
        private const float MaxVelocity = Speed;
        private const float MouseSensitivity = 0.01f;
        private const float JumpVelocity = 5f;

        public BuildingFreeCamera(CameraProps props, IInputService inputService, List<BuildingWallSegment> globalWallSegments, BuildingStoryLayoutPlacementAlgorithm pa)
        {
            this.props = Props.FromCommonProps(props.Frame);
            this.inputService = inputService;
            this.globalWallSegments = globalWallSegments;
            this.pa = pa;
            sg = pa.StoryGraph;
        }

        public Color4 VeilColor { get; }

        public CameraProps GetProps()
        {
            var frame = props.GetFrame();
            return new CameraProps(frame.Eye + frame.Forward, frame, new CameraProjection(0.1f, 100f, MathHelper.PiOver4));
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

            horrVelocity += new Vector3(accel.X, 0, accel.Z);
            vertVelocity += accel.Y;
            var horrSpeed = horrVelocity.Length();
            if (horrSpeed > MaxVelocity)
            {
                horrVelocity = horrVelocity / horrSpeed * MaxVelocity;
                horrSpeed = MaxVelocity;
            }

            var eyeLoc = eye;
            var floorNode = sg.Children[sg.Root]
                .Where(x => new AaBox(pa.RelativeTransforms[x].Offset, pa.HalfSizes[x]).Contains(eyeLoc))
                .FirstOrNull();

            if (floorNode.HasValue)
            {
                if (vertVelocity == 0 && inputService.CurrentKeyboardState.IsKeyPressed(Key.Space))
                    vertVelocity = JumpVelocity;
                else
                    vertVelocity -= Gravity * frameTime.DeltaSeconds;
                var floorHeight = pa.RelativeTransforms[floorNode.Value].Offset.Y;

                eye.Y += vertVelocity * frameTime.DeltaSeconds;
                if (eye.Y < floorHeight + BuildingConstants.EyeHeight)
                {
                    eye.Y = floorHeight + BuildingConstants.EyeHeight;
                    vertVelocity = 0;
                }
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

                eye.Y += vertVelocity * frameTime.DeltaSeconds;
                if (eye.Y < BuildingConstants.EyeHeight)
                {
                    eye.Y = BuildingConstants.EyeHeight;
                    vertVelocity = 0;
                }
            }

            var remainingT = Math.Min(frameTime.DeltaSeconds, 1f);
            while (remainingT > 0)
            {
                var ray = new Ray3(eye, horrVelocity.Normalize());
                var intersection = globalWallSegments
                    .Select(x => Intersects(ray, x))
                    .Where(x => x.HasValue)
                    .MinimalOrDefault(x => x?.Distance ?? float.MaxValue);
                if (intersection.HasValue)
                {
                    var tangent = intersection.Value.Wall.Difference.Normalize();
                    var normal = Vector3.Cross(tangent, Vector3.UnitY);
                    if (Vector3.Dot(horrVelocity, normal) > 0)
                        normal = -normal;
                    var nVelocity = Vector3.Dot(horrVelocity, -normal);
                    var toEye = -ray.Direction * intersection.Value.Distance;
                    var nDist = Vector3.Dot(toEye, normal);
                    var offsetNDist = Math.Max(nDist - 0.8f, 0);

                    if (nVelocity * remainingT < offsetNDist)
                    {
                        eye += horrVelocity * remainingT;
                        remainingT = 0;
                    }
                    else
                    {
                        var amount = MathHelper.Clamp(offsetNDist / nDist, 0, 1);
                        var move = -toEye * amount;
                        eye += move;
                        remainingT -= move.Length() / horrVelocity.Length();
                        horrVelocity = Vector3.Dot(horrVelocity, tangent) * tangent;
                    }
                }
                else
                {
                    eye += horrVelocity * remainingT;
                    remainingT = 0;
                }
            }

            props.Eye = eye;
        }

        private static WallIntersection? Intersects(Ray3 ray, BuildingWallSegment wallSegment)
        {
            var height = ray.Point.Y;
            if (height < wallSegment.Basement.Point1.Y ||
                height > wallSegment.Basement.Point1.Y + wallSegment.Height)
            {
                return null;
            }
            var ray2D = new Ray2(ray.Point.Xz, ray.Direction.Xz.Normalize());
            var wallSeg2D = new LineSegment2(wallSegment.Basement.Point1.Xz, wallSegment.Basement.Point2.Xz);
            var inter2D = wallSeg2D.Intersect(ray2D);
            if (!inter2D.HasValue)
            {
                return null;
            }
            return new WallIntersection
            {
                Wall = wallSegment.Basement,
                Distance = (inter2D.Value - ray2D.Point).Length()
            };
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