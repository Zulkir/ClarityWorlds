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
using Clarity.Engine.Interaction.Input.VRController;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Ext.StoryLayout.Building
{
    class BuildingWarpCamera : IControlledCamera
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
        private readonly BuildingStoryLayoutPlacementAlgorithm pa;
        private readonly IStoryGraph sg;
        private readonly List<BuildingWallSegment> globalWallSegments;
       
        private const float MouseSensitivity = 0.01f;
        private bool triggerWasHeld = false;

        public BuildingWarpCamera(CameraProps props, List<BuildingWallSegment> globalWallSegments, BuildingStoryLayoutPlacementAlgorithm pa)
        {
            this.props = Props.FromCommonProps(props.Frame);
            this.globalWallSegments = globalWallSegments;
            this.pa = pa;
            sg = pa.StoryGraph;
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
            

            var accel = Vector3.Zero;
            
        }

        private bool TryWarpAlong(Ray3 warpRay)
        {
            var frame = GetGlobalFrame();
            var eye = frame.Eye;
            var floorNode = InsideARoom(eye);
            if (floorNode.HasValue)
            {
                var ray = warpRay;
                var rayIntersectsWall = globalWallSegments
                                    .Where(x => IsOnSameLevel(ray.Point, x))
                                    .Any(x => Intersects(ray, x));
                if (!rayIntersectsWall)
                {

                    var floorHeight = pa.RelativeTransforms[floorNode.Value].Offset.Y;
                    var pointOnFloor = new Vector3(0, floorHeight, 0);
                    var floorPlane = new Plane(Vector3.UnitY, pointOnFloor);
                    var floorIntersection = ray.Intersect(floorPlane);
                    if (floorIntersection.HasValue)
                    {
                        var eyeHeight = eye.Y;
                        var warpPoint = floorIntersection.Value + Vector3.UnitY * eyeHeight;
                        var warpTargetInBounds = InsideARoom(warpPoint).HasValue;
                        if (warpTargetInBounds)
                        {
                            props.Eye = warpPoint;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private int? InsideARoom(Vector3 p)
        {
            return sg.Children[sg.Root]
                    .Where(x => new AaBox(pa.RelativeTransforms[x].Offset, pa.HalfSizes[x]).Contains(p))
                    .FirstOrNull();
        }

        private static bool IsOnSameLevel(Vector3 point, BuildingWallSegment wallSegment)
        {
            var height = point.Y;
            return height >= wallSegment.Basement.Point1.Y &&
                    height <= wallSegment.Basement.Point1.Y + wallSegment.Height;
        }

        private static bool Intersects(Ray3 ray, BuildingWallSegment wallSegment)
        {
            var vectorInWallPlane = wallSegment.Basement.Point1 - wallSegment.Basement.Point2;
            var wallNormal = Vector3.Cross(Vector3.UnitY, vectorInWallPlane);
            var wallPlane = new Plane(wallNormal , wallSegment.Basement.Point1);
            var rayWallIntersection = ray.Intersect(wallPlane);
            if (!rayWallIntersection.HasValue)
            {
                return false;
            }
            if (rayWallIntersection.Value.Y < wallSegment.Basement.Point1.Y ||
                rayWallIntersection.Value.Y > wallSegment.Basement.Point1.Y + wallSegment.Height)
            {
                return false;
            }
            var ray2D = new Ray2(ray.Point.Xz,ray.Direction.Xz);
            var wallSeg2D = new LineSegment2(wallSegment.Basement.Point1.Xz, wallSegment.Basement.Point2.Xz);
            var inter2D = wallSeg2D.Intersect(ray2D);
            if(inter2D.HasValue)
            {
                return true;
            }
            return false;
        }


        public bool TryHandleInput(IInputEvent eventArgs)
        {
            if (eventArgs is IMouseEvent margs)
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
            else if (eventArgs is IVrControllerEvent vrcargs)
            {
                if (vrcargs.IsLeftTriggerPress())
                {
                    Ray3 globalRay = new Ray3(props.Eye, vrcargs.State.LeftForward);
                    TryWarpAlong(globalRay);
                }
            }
            else if (eventArgs is IKeyEvent kargs)
            {
                if (kargs.EventKey == Key.W && kargs.ComplexEventType == KeyEventType.Down)
                {
                    var frame = GetGlobalFrame();
                    Ray3 globalRay = new Ray3(frame.Eye, frame.Forward);
                    TryWarpAlong(globalRay);
                }
            }
            return false;
        }
    }
}
