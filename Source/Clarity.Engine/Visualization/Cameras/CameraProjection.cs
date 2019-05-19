using Clarity.Common.Numericals;

namespace Clarity.Engine.Visualization.Cameras
{
    public struct CameraProjection
    {
        public float ZNear;
        public float ZFar;
        public float Fov;
        
        public CameraProjection(float zNear, float zFar, float fov)
        {
            ZNear = zNear;
            ZFar = zFar;
            Fov = fov;
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