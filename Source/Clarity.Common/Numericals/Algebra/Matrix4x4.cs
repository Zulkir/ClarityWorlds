using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 64)]
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        public Vector4 C0;
        public Vector4 C1;
        public Vector4 C2;
        public Vector4 C3;

        #region Constructors
        public Matrix4x4(Vector4 c0, Vector4 c1, Vector4 c2, Vector4 c3)
        {
            C0 = c0;
            C1 = c1;
            C2 = c2;
            C3 = c3;
        }

        public Matrix4x4(Matrix4x3 matrix4x3)
        {
            C0 = matrix4x3.C0;
            C1 = matrix4x3.C1;
            C2 = matrix4x3.C2;
            C3 = Vector4.UnitW;
        }

        public Matrix4x4(Matrix3x3 matrix3x3)
        {
            C0 = new Vector4(matrix3x3.C0, 0f);
            C1 = new Vector4(matrix3x3.C1, 0f);
            C2 = new Vector4(matrix3x3.C2, 0f);
            C3 = Vector4.UnitW;
        }
        #endregion

        public float M00 { get { return C0.X; } set { C0.X = value; } }
        public float M10 { get { return C0.Y; } set { C0.Y = value; } }
        public float M20 { get { return C0.Z; } set { C0.Z = value; } }
        public float M30 { get { return C0.W; } set { C0.W = value; } }

        public float M01 { get { return C1.X; } set { C1.X = value; } }
        public float M11 { get { return C1.Y; } set { C1.Y = value; } }
        public float M21 { get { return C1.Z; } set { C1.Z = value; } }
        public float M31 { get { return C1.W; } set { C1.W = value; } }

        public float M02 { get { return C2.X; } set { C2.X = value; } }
        public float M12 { get { return C2.Y; } set { C2.Y = value; } }
        public float M22 { get { return C2.Z; } set { C2.Z = value; } }
        public float M32 { get { return C2.W; } set { C2.W = value; } }

        public float M03 { get { return C3.X; } set { C3.X = value; } }
        public float M13 { get { return C3.Y; } set { C3.Y = value; } }
        public float M23 { get { return C3.Z; } set { C3.Z = value; } }
        public float M33 { get { return C3.W; } set { C3.W = value; } }

        public Vector4 R0
        {
            get { return new Vector4(C0.X, C1.X, C2.X, C3.X); }
            set { C0.X = value.X; C1.X = value.Y; C2.X = value.Z; C3.X = value.W; }
        }

        public Vector4 R1
        {
            get { return new Vector4(C0.Y, C1.Y, C2.Y, C3.Y); }
            set { C0.Y = value.X; C1.Y = value.Y; C2.Y = value.Z; C3.Y = value.W; }
        }

        public Vector4 R2
        {
            get { return new Vector4(C0.Z, C1.Z, C2.Z, C3.Z); }
            set { C0.Z = value.X; C1.Z = value.Y; C2.Z = value.Z; C3.Z = value.W; }
        }

        public Vector4 R3
        {
            get { return new Vector4(C0.W, C1.W, C2.W, C3.W); }
            set { C0.W = value.X; C1.W = value.Y; C2.W = value.Z; C3.W = value.W; }
        }

        public Matrix3x3 Sub3x3 => new Matrix3x3(C0.Xyz, C1.Xyz, C2.Xyz);
        public Matrix4x3 Sub4x3 => new Matrix4x3(C0, C1, C2);

        public float this[int row, int col]
        {
            get
            {
                switch (col)
                {
                    case 0: return C0[row];
                    case 1: return C1[row];
                    case 2: return C2[row];
                    case 3: return C3[row];
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (col)
                {
                    case 0: C0[row] = value; break;
                    case 1: C1[row] = value; break;
                    case 2: C2[row] = value; break;
                    case 3: C3[row] = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return
                C0.GetHashCode() ^
                (C1.GetHashCode() << 1) ^
                (C2.GetHashCode() << 2) ^
                (C3.GetHashCode() << 3);
        }

        public override string ToString()
        {
            return string.Format("{{ {0}, {1}, {2}, {3} }}",
                R0.ToString(),
                R1.ToString(),
                R2.ToString(),
                R3.ToString());
        }

        public static bool Equals(Matrix4x4 s1, Matrix4x4 s2)
        {
            return
                s1.C0 == s2.C0 &&
                s1.C1 == s2.C1 &&
                s1.C2 == s2.C2 &&
                s1.C3 == s2.C3;
        }

        public static bool operator ==(Matrix4x4 s1, Matrix4x4 s2) { return Equals(s1, s2); }
        public static bool operator !=(Matrix4x4 s1, Matrix4x4 s2) { return !Equals(s1, s2); }
        public bool Equals(Matrix4x4 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Matrix4x4 && Equals((Matrix4x4)obj); }
        #endregion

        #region Math
        public Matrix4x4 Transpose()
        {
            return new Matrix4x4(R0, R1, R2, R3);
        }

        #region DeterminantOptimizers
        private float D0101 { get { return M22 * M33 - M23 * M32; } }
        private float D0102 { get { return M21 * M33 - M23 * M31; } }
        private float D0103 { get { return M21 * M32 - M22 * M31; } }
        private float D0112 { get { return M20 * M33 - M23 * M30; } }
        private float D0113 { get { return M20 * M32 - M22 * M30; } }
        private float D0123 { get { return M20 * M31 - M21 * M30; } }
        private float D0201 { get { return M12 * M33 - M13 * M32; } }
        private float D0202 { get { return M11 * M33 - M13 * M31; } }
        private float D0203 { get { return M11 * M32 - M12 * M31; } }
        private float D0212 { get { return M10 * M33 - M13 * M30; } }
        private float D0213 { get { return M10 * M32 - M12 * M30; } }
        private float D0223 { get { return M10 * M31 - M11 * M30; } }
        private float D0301 { get { return M12 * M23 - M13 * M22; } }
        private float D0302 { get { return M11 * M23 - M13 * M21; } }
        private float D0303 { get { return M11 * M22 - M12 * M21; } }
        private float D0312 { get { return M10 * M23 - M13 * M20; } }
        private float D0313 { get { return M10 * M22 - M12 * M20; } }
        private float D0323 { get { return M10 * M21 - M11 * M20; } }

        private float D00 { get { return M11 * D0101 - M12 * D0102 + M13 * D0103; } }
        private float D01 { get { return M10 * D0101 - M12 * D0112 + M13 * D0113; } }
        private float D02 { get { return M10 * D0102 - M11 * D0112 + M13 * D0123; } }
        private float D03 { get { return M10 * D0103 - M11 * D0113 + M12 * D0123; } }
        private float D10 { get { return M01 * D0101 - M02 * D0102 + M03 * D0103; } }
        private float D11 { get { return M00 * D0101 - M02 * D0112 + M03 * D0113; } }
        private float D12 { get { return M00 * D0102 - M01 * D0112 + M03 * D0123; } }
        private float D13 { get { return M00 * D0103 - M01 * D0113 + M02 * D0123; } }
        private float D20 { get { return M01 * D0201 - M02 * D0202 + M03 * D0203; } }
        private float D21 { get { return M00 * D0201 - M02 * D0212 + M03 * D0213; } }
        private float D22 { get { return M00 * D0202 - M01 * D0212 + M03 * D0223; } }
        private float D23 { get { return M00 * D0203 - M01 * D0213 + M02 * D0223; } }
        private float D30 { get { return M01 * D0301 - M02 * D0302 + M03 * D0303; } }
        private float D31 { get { return M00 * D0301 - M02 * D0312 + M03 * D0313; } }
        private float D32 { get { return M00 * D0302 - M01 * D0312 + M03 * D0323; } }
        private float D33 { get { return M00 * D0303 - M01 * D0313 + M02 * D0323; } }
        #endregion

        public float Determinant()
        {
            return M00 * D00 - M01 * D01 + M02 * D02 - M03 * D03;
        }

        public Matrix4x4 Invert()
        {
            float invDet = 1.0f / Determinant();
            return new Matrix4x4
            {
                M00 = D00 * invDet,
                M10 = -D01 * invDet,
                M20 = D02 * invDet,
                M30 = -D03 * invDet,
                M01 = -D10 * invDet,
                M11 = D11 * invDet,
                M21 = -D12 * invDet,
                M31 = D13 * invDet,
                M02 = D20 * invDet,
                M12 = -D21 * invDet,
                M22 = D22 * invDet,
                M32 = -D23 * invDet,
                M03 = -D30 * invDet,
                M13 = D31 * invDet,
                M23 = -D32 * invDet,
                M33 = D33 * invDet
            };
        }

        public static Matrix4x4 operator -(Matrix4x4 m) { return m.Negate(); }
        public Matrix4x4 Negate()
        {
            return new Matrix4x4(-C0, -C1, -C2, -C3);
        }

        public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2) { return Add(m1, m2); }
        public static Matrix4x4 Add(Matrix4x4 m1, Matrix4x4 m2)
        {
            return new Matrix4x4(
                m1.C0 + m2.C0,
                m1.C1 + m2.C1,
                m1.C2 + m2.C2,
                m1.C3 + m2.C3);
        }

        public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right) { return Subtract(left, right); }
        public static Matrix4x4 Subtract(Matrix4x4 left, Matrix4x4 right)
        {
            return new Matrix4x4(
                left.C0 - right.C0,
                left.C1 - right.C1,
                left.C2 - right.C2,
                left.C3 - right.C3);
        }

        public static Matrix4x4 operator *(float scale, Matrix4x4 m) { return m.ScaleBy(scale); }
        public static Matrix4x4 operator *(Matrix4x4 m, float scale) { return m.ScaleBy(scale); }
        public Matrix4x4 ScaleBy(float scale)
        {
            return new Matrix4x4(C0 * scale, C1 * scale, C2 * scale, C3 * scale);
        }

        public static Vector4 operator *(Vector4 v, Matrix4x4 m) { return Multiply(v, m); }
        public static Vector4 Multiply(Vector4 v, Matrix4x4 m)
        {
            return new Vector4(
                Vector4.Dot(v, m.C0),
                Vector4.Dot(v, m.C1),
                Vector4.Dot(v, m.C2),
                Vector4.Dot(v, m.C3));
        }

        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right) { return Multiply(left, right); }
        public static Matrix4x4 Multiply(Matrix4x4 left, Matrix4x4 right)
        {
            return new Matrix4x4
            {
                R0 = left.R0 * right,
                R1 = left.R1 * right,
                R2 = left.R2 * right,
                R3 = left.R3 * right,
            };
        }

        public static Matrix4x4 Lerp(Matrix4x4 left, Matrix4x4 right, float amount)
        {
            return new Matrix4x4(
                Vector4.Lerp(left.C0, right.C0, amount),
                Vector4.Lerp(left.C1, right.C1, amount),
                Vector4.Lerp(left.C2, right.C2, amount),
                Vector4.Lerp(left.C3, right.C3, amount));
        }
        #endregion

        #region Constant Matrices
        public static Matrix4x4 Zero
        {
            get { return new Matrix4x4(); }
        }

        public static Matrix4x4 Identity
        {
            get { return new Matrix4x4 {M00 = 1f, M11 = 1f, M22 = 1f, M33 = 1f}; }
        }
        #endregion

        #region Typical transforms
        public static Matrix4x4 Scaling(float scale) { return Scaling(scale, scale, scale); }
        public static Matrix4x4 Scaling(Vector3 scale) { return Scaling(scale.X, scale.Y, scale.Z); }
        public static Matrix4x4 Scaling(float sx, float sy, float sz)
        {
            return new Matrix4x4 { M00 = sx, M11 = sy, M22 = sz, M33 = 1f };
        }

        public static Matrix4x4 RotationX(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix4x4
            {
                M00 = 1f,
                M11 = cosa, M12 = -sina,
                M21 = sina, M22 = cosa,
                M33 = 1f
            };
        }

        public static Matrix4x4 RotationY(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix4x4
            {
                M00 = cosa, M02 = -sina,
                M11 = 1f,
                M22 = cosa, M20 = sina,
                M33 = 1f
            };
        }

        public static Matrix4x4 RotationZ(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix4x4
            {
                M00 = cosa, M01 = -sina,
                M11 = cosa, M10 = sina,
                M22 = 1f,
                M33 = 1f
            };
        }

        public static Matrix4x4 Translation(Vector3 translation)
        {
            return new Matrix4x4
            {
                M00 = 1f, M11 = 1f, M22 = 1f,
                M30 = translation.X,
                M31 = translation.Y,
                M32 = translation.Z,
                M33 = 1f
            };
        }

        public static Matrix4x4 LookAtRh(Vector3 eye, Vector3 dir, Vector3 up)
        {
            var back = -dir.Normalize();
            var left = Vector3.Cross(up, back).Normalize();
            var exactUp = Vector3.Cross(back, left);

            return new Matrix4x4(
                new Vector4(left, -Vector3.Dot(left, eye)),
                new Vector4(exactUp, -Vector3.Dot(exactUp, eye)),
                new Vector4(back, -Vector3.Dot(back, eye)),
                Vector4.UnitW);
        }

        public static Matrix4x4 LookAtLh(Vector3 eye, Vector3 dir, Vector3 up)
        {
            var result = LookAtRh(eye, dir, up);
            result.R2 = -result.R2;
            return result;
        }

        public static Matrix4x4 PerspectiveFovDx(float fieldOfView, float aspectRatio, float zNear, float zFar)
        {
            float yScale = 1f / MathHelper.Tan(fieldOfView * 0.5f);
            float xScale = yScale / aspectRatio;

            return new Matrix4x4
            {
                M00 = xScale,
                M11 = yScale,
                M22 = (zFar + zNear) / (zNear - zFar),
                M23 = -1f,
                M32 = 2.0f * zFar * zNear / (zNear - zFar)
            };
        }

        public static Matrix4x4 OrthoFromPerspective(float targetDistance, float fieldOfView, float aspectRatio, float zNear, float zFar)
        {
            // todo: adjust znear/zfar
            var scale = MathHelper.Tan(fieldOfView / 2) * targetDistance;
            return OrthoScaleDx(scale, aspectRatio, zNear, zFar);
        }

        public static Matrix4x4 OrthoScaleDx(float scale, float aspectRatio, float zNear, float zFar)
        {
            float yScale = 1f / scale;
            float xScale = yScale / aspectRatio;

            return new Matrix4x4
            {
                M00 = xScale,
                M11 = yScale,
                M22 = 1f / (zNear - zFar),
                M32 = zNear / (zNear - zFar),
                M33 = 1f
            };
        }
        #endregion
    }
}
