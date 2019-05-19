using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.Engine.Visualization.Cameras
{
    public struct CameraFrame
    {
        public Vector3 Eye;
        public Vector3 Forward;
        public Vector3 Up;
        public Vector3 Right;

        public CameraFrame(Vector3 eye, Vector3 forward, Vector3 up, Vector3 right)
        {
            Eye = eye;
            Forward = forward;
            Up = up;
            Right = right;
        }

        public CameraFrame(Vector3 eye, Vector3 roughForward, Vector3 roughUp)
        {
            Eye = eye;
            Forward = roughForward.Normalize();
            Right = Vector3.Cross(Forward, roughUp.Normalize());
            Up = Vector3.Cross(Right, Forward);
        }

        public CameraFrame(Vector3 eye, Quaternion rotation)
        {
            Eye = eye;
            Forward = -Vector3.UnitZ * rotation;
            Right = Vector3.UnitX * rotation;
            Up = Vector3.UnitY * rotation;
        }

        public Matrix4x4 GetViewMat()
        {
            return Matrix4x4.LookAtRh(Eye, Forward, Up);
        }

        public static CameraFrame operator *(CameraFrame f, Transform tr) { return Apply(f, tr); }
        public static CameraFrame Apply(CameraFrame f, Transform tr)
        {
            var eye = f.Eye * tr;
            var forward = f.Forward * tr.Rotation;
            var up = f.Up * tr.Rotation;
            var right = Vector3.Cross(forward, up);
            return new CameraFrame(eye, forward, up, right);
        }

        public static CameraFrame Lerp(CameraFrame f1, CameraFrame f2, float amount)
        {
            var eye = Vector3.Lerp(f1.Eye, f2.Eye, amount);
            var r1 = f1.GetRotation();
            var r2 = f2.GetRotation();
            if (Vector3.Dot(r1.Xyz, r2.Xyz) < 0)
                r2 = -r2;
            var rotation = Quaternion.Slerp(r1, r2, amount);
            return new CameraFrame(eye, rotation);
        }

        public Quaternion GetRotation()
        {
            return Quaternion.RotationToFrame(Right, Up);
        }

        public static CameraFrame Default
        {
            get
            {
                //return new CameraFrame(new Vector3(0, 0, 0), Vector3.UnitY, Vector3.UnitZ);

                const float distance = 15f;
                var sin = MathHelper.Sin(MathHelper.PiOver4);
                var cos = MathHelper.Cos(MathHelper.PiOver4);
                var eye = new Vector3(-cos * cos, -sin * cos, sin) * distance;
                return new CameraFrame(eye, -eye, Vector3.UnitZ);
            }
        }
    }
}