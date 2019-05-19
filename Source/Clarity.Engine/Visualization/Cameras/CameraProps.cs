using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Visualization.Cameras
{
    public struct CameraProps
    {
        public Vector3 Target;
        public CameraFrame Frame;
        public CameraProjection Projection;

        public CameraProps(Vector3 target, CameraFrame frame, CameraProjection projection)
        {
            Target = target;
            Frame = frame;
            Projection = projection;
        }

        public static CameraProps Lerp(CameraProps cam1, CameraProps cam2, float amount)
        {
            var target = Vector3.Lerp(cam1.Target, cam2.Target, amount);
            var eye = Vector3.Lerp(cam1.Frame.Eye, cam2.Frame.Eye, amount);
            var up = Vector3.Lerp(cam1.Frame.Up, cam2.Frame.Up, amount);
            var frame = new CameraFrame(eye, target - eye, up);
            var proj = CameraProjection.Lerp(cam1.Projection, cam2.Projection, amount);
            return new CameraProps(target, frame, proj);
        }
    }
}