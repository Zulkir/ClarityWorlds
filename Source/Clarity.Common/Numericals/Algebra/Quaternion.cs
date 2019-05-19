using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        public Vector4 Raw;

        #region Constructors
        public Quaternion(Vector4 raw)
        {
            Raw = raw;
        }

        public Quaternion(Vector3 xyz, float w)
        {
            Raw = new Vector4(xyz, w);
        }

        public Quaternion(float x, float y, float z, float w)
        {
            Raw = new Vector4(x, y, z, w);
        }
        #endregion

        public float X { get { return Raw.X; } set { Raw.X = value; } }
        public float Y { get { return Raw.Y; } set { Raw.Y = value; } }
        public float Z { get { return Raw.Z; } set { Raw.Z = value; } }
        public float W { get { return Raw.W; } set { Raw.W = value; } }

        public float this[int index] { get { return Raw[index]; } set { Raw[index] = value; } }
        public Vector3 Xyz { get { return Raw.Xyz; } set { Raw.Xyz = value; } }

        #region Equality, Hash, String
        public override int GetHashCode() { return Raw.GetHashCode(); }
        public override string ToString() { return Raw.ToString(); }
        public static bool Equals(Quaternion s1, Quaternion s2) { return s1.Raw == s2.Raw; }
        public static bool operator ==(Quaternion s1, Quaternion s2) { return Equals(s1, s2); }
        public static bool operator !=(Quaternion s1, Quaternion s2) { return !Equals(s1, s2); }
        public bool Equals(Quaternion other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Quaternion && Equals((Quaternion)obj); }
        #endregion
        
        #region Math
        public Quaternion Conjugate() { return new Quaternion(-X, -Y, -Z, W); }

        public float Norm() { return MathHelper.Sqrt(NormSquared()); }
        public float NormSquared() { return Raw.LengthSquared(); }

        public Quaternion Normalize() { return new Quaternion(Raw.Normalize()); }

        public static Quaternion operator -(Quaternion q) { return q.Negate(); }
        public Quaternion Negate() { return new Quaternion(-Raw); }

        public static Quaternion operator +(Quaternion q1, Quaternion q2) { return Add(q1, q2); }
        public static Quaternion Add(Quaternion q1, Quaternion q2) { return new Quaternion(q1.Raw + q2.Raw); }

        public static Quaternion operator -(Quaternion left, Quaternion right) { return Subtract(left, right); }
        public static Quaternion Subtract(Quaternion left, Quaternion right) { return new Quaternion(left.Raw - right.Raw); }

        public static Quaternion operator *(float scale, Quaternion q) { return q.ScaleBy(scale); }
        public static Quaternion operator *(Quaternion q, float scale) { return q.ScaleBy(scale); }
        public static Quaternion operator /(Quaternion q, float scale) { return q.ScaleBy(1f / scale); }
        public Quaternion ScaleBy(float scale) { return new Quaternion(scale * Raw); }

        public static Quaternion operator *(Quaternion left, Quaternion right) { return Multiply(left, right); }
        public static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            var a = (left.W + left.X) * (right.W + right.X);
            var b = (left.Z - left.Y) * (right.Y - right.Z);
            var c = (left.X - left.W) * (right.Y + right.Z);
            var d = (left.Y + left.Z) * (right.X - right.W);
            var e = (left.X + left.Z) * (right.X + right.Y);
            var f = (left.X - left.Z) * (right.X - right.Y);
            var g = (left.W + left.Y) * (right.W - right.Z);
            var h = (left.W - left.Y) * (right.W + right.Z);
            
            return new Quaternion(
                a - (e + f + g + h) * 0.5f,
                -c + (e - f + g - h) * 0.5f,
                -d + (e - f - g + h) * 0.5f,
                b + (-e - f + g + h) * 0.5f);
        }

        public static Vector4 operator *(Vector4 v, Quaternion q) { return Apply(v, q); }
        public static Vector3 operator *(Vector3 v, Quaternion q) { return Apply(v, q); }
        public static Vector4 Apply(Vector4 v, Quaternion q) { return new Vector4(Apply(v.Xyz, q), v.W); }
        public static Vector3 Apply(Vector3 v, Quaternion q)
        {
            q = q.Conjugate();
            Vector3 a, b;

            a.X = q.Y * v.Z - q.Z * v.Y + q.W * v.X;
            a.Y = q.Z * v.X - q.X * v.Z + q.W * v.Y;
            a.Z = q.X * v.Y - q.Y * v.X + q.W * v.Z;

            b.X = q.Y * a.Z - q.Z * a.Y;
            b.Y = q.Z * a.X - q.X * a.Z;
            b.Z = q.X * a.Y - q.Y * a.X;

            return new Vector3(
                v.X + 2.0f * b.X,
                v.Y + 2.0f * b.Y,
                v.Z + 2.0f * b.Z);
        }

        public static float Dot(Quaternion q1, Quaternion q2) { return Vector4.Dot(q1.Raw, q2.Raw); }

        public static Quaternion Lerp(Quaternion left, Quaternion right, float amount)
        {
            return new Quaternion(Vector4.Lerp(left.Raw, right.Raw, amount));
        }

        public static Quaternion NLerp(Quaternion left, Quaternion right, float amount)
        {
            return Lerp(left, right, amount).Normalize();
        }

        public static Quaternion Slerp(Quaternion left, Quaternion right, float amount)
        {
            var cosTheta = MathHelper.Clamp(Dot(left, right), -1f, 1f);
            var theta = MathHelper.Acos(cosTheta);
            var sinTheta = MathHelper.Sin(theta);
            var invSinTheta = 1.0f / sinTheta;
            var wLeft = MathHelper.Sin((1.0f - amount) * theta) * invSinTheta;
            var wRight = MathHelper.Sin(amount * theta) * invSinTheta;

            if (cosTheta < 0f) 
                wRight = -wRight;

            return new Quaternion(
                wLeft * left.X + wRight * right.X,
                wLeft * left.Y + wRight * right.Y,
                wLeft * left.Z + wRight * right.Z,
                wLeft * left.W + wRight * right.W);
        }
        #endregion

        #region Convertions
        public Matrix3x3 ToMatrix3x3()
        {
            Matrix3x3 result = new Matrix3x3();
            float x2 = X + X;
            float y2 = Y + Y;
            float z2 = Z + Z;
            float xx2 = X * x2;
            float yy2 = Y * y2;
            float zz2 = Z * z2;
            result.M00 = 1.0f - yy2 - zz2;
            result.M11 = 1.0f - xx2 - zz2;
            result.M22 = 1.0f - xx2 - yy2;
            float yz2 = Y * z2;
            float wx2 = W * x2;
            result.M21 = yz2 - wx2;
            result.M12 = yz2 + wx2;
            float xy2 = X * y2;
            float wz2 = W * z2;
            result.M10 = xy2 - wz2;
            result.M01 = xy2 + wz2;
            float xz2 = X * z2;
            float wy2 = W * y2;
            result.M02 = xz2 - wy2;
            result.M20 = xz2 + wy2;
            return result.Transpose();
        }

        public Matrix4x3 ToMatrix4x3()
        {
            return new Matrix4x3(ToMatrix3x3());
        }

        public Matrix4x4 ToMatrix4x4()
        {
            return new Matrix4x4(ToMatrix3x3());
        }
        #endregion
        
        #region Typical Rotations
        public static Quaternion RotationX(float angle)
        {
            var halfAngle = angle * 0.5f;
            float sina = MathHelper.Sin(halfAngle);
            float cosa = MathHelper.Cos(halfAngle);
            return new Quaternion { X = sina, W = cosa };
        }

        public static Quaternion RotationY(float angle)
        {
            var halfAngle = angle * 0.5f;
            float sina = MathHelper.Sin(halfAngle);
            float cosa = MathHelper.Cos(halfAngle);
            return new Quaternion { Y = sina, W = cosa };
        }
        public static Quaternion RotationZ(float angle)
        {
            var halfAngle = angle * 0.5f;
            float sina = MathHelper.Sin(halfAngle);
            float cosa = MathHelper.Cos(halfAngle);
            return new Quaternion { Z = sina, W = cosa };
        }

        public static Quaternion RotationAxis(Vector3 axis, float angle)
        {
            var nAxis = axis.Normalize();
            var halfAngle = angle * 0.5f;
            float sina = MathHelper.Sin(halfAngle);
            float cosa = MathHelper.Cos(halfAngle);

            return new Quaternion(
                nAxis.X * sina,
                nAxis.Y * sina,
                nAxis.Z * sina, 
                cosa);
        }

        public static Quaternion RotationToFrame(Vector3 roughXAxis, Vector3 roughYAxis)
        {
            var xAxis = roughXAxis.Normalize();
            var zAxis = Vector3.Cross(xAxis, roughYAxis).Normalize();
            var yAxis = Vector3.Cross(zAxis, xAxis);

            var trace = xAxis.X + yAxis.Y + zAxis.Z;
            if (trace > 0.00001f)
            {
                var s = MathHelper.Sqrt(trace + 1) * 2;
                return new Quaternion(
                    (zAxis.Y - yAxis.Z) / s,
                    (xAxis.Z - zAxis.X) / s,
                    (yAxis.X - xAxis.Y) / s,
                    0.25f * s);
            } 
            else if ((xAxis.X > yAxis.Y) && (xAxis.X > zAxis.Z))
            {
                var s = MathHelper.Sqrt(1 + xAxis.X - yAxis.Y - zAxis.Z) * 2;
                return new Quaternion(
                    0.25f * s,
                    (xAxis.Y + yAxis.X) / s,
                    (xAxis.Z + zAxis.X) / s,
                    (zAxis.Y - yAxis.Z) / s);
            }
            else if (yAxis.Y > zAxis.Z)
            {
                var s = MathHelper.Sqrt(1 + yAxis.Y - xAxis.X - zAxis.Z) * 2;
                return new Quaternion(
                    (xAxis.Y + yAxis.X) / s,
                    0.25f * s,
                    (yAxis.Z + zAxis.Y) / s,
                    (xAxis.Z - zAxis.X) / s);
            }
            else
            {
                var s = MathHelper.Sqrt(1 + zAxis.Z - xAxis.X - yAxis.Y) * 2;
                return new Quaternion(
                    (xAxis.Z + zAxis.X) / s,
                    (yAxis.Z + zAxis.Y) / s,
                    0.25f * s,
                    (yAxis.X - xAxis.Y) / s);
            }
        }
        #endregion

        #region Constant Quaternions
        public static Quaternion Zero { get { return new Quaternion(); } }
        public static Quaternion Identity { get { return new Quaternion {W = 1.0f}; } }
        #endregion
    }
}
