using Clarity.Common.Numericals;
using System;

namespace Clarity.Engine.Visualization.Cameras
{
    public enum CameraProjectionType
    {
        Perspective,
        Orthographic
    }

    public struct CameraProjection
    {
        public CameraProjectionType Type;
        public float ZNear;
        public float ZFar;
        public float Fov;
        public float Scale;

        public CameraProjection(CameraProjectionType type, float zNear, float zFar, float fovOrScale)
        {
            Type = type;
            ZNear = zNear;
            ZFar = zFar;
            switch (type)
            {
                case CameraProjectionType.Perspective:
                    {
                        Fov = fovOrScale;
                        Scale = 1;
                        break;
                    }
                case CameraProjectionType.Orthographic:
                    {
                        Fov = 0;
                        Scale = fovOrScale;
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static CameraProjection Lerp(CameraProjection left, CameraProjection right, float amount)
        {
            return new CameraProjection
            {
                ZNear = MathHelper.Lerp(left.ZNear, right.ZNear, amount),
                ZFar = MathHelper.Lerp(left.ZFar, right.ZFar, amount),
                Fov = MathHelper.Lerp(left.Fov, right.Fov, amount)
            };
        }
    }
}