using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 48)]
    public struct Matrix4x3 : IEquatable<Matrix4x3>
    {
        public Vector4 C0;
        public Vector4 C1;
        public Vector4 C2;

        #region Constructors
        public Matrix4x3(Vector4 c0, Vector4 c1, Vector4 c2)
        {
            C0 = c0;
            C1 = c1;
            C2 = c2;
        }

        public Matrix4x3(Matrix3x3 matrix3x3)
        {
            C0 = new Vector4(matrix3x3.C0, 0f);
            C1 = new Vector4(matrix3x3.C1, 0f);
            C2 = new Vector4(matrix3x3.C2, 0f);
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

        public Vector3 R0
        {
            get { return new Vector3(C0.X, C1.X, C2.X); }
            set { C0.X = value.X; C1.X = value.Y; C2.X = value.Z; }
        }

        public Vector3 R1
        {
            get { return new Vector3(C0.Y, C1.Y, C2.Y); }
            set { C0.Y = value.X; C1.Y = value.Y; C2.Y = value.Z; }
        }

        public Vector3 R2
        {
            get { return new Vector3(C0.Z, C1.Z, C2.Z); }
            set { C0.Z = value.X; C1.Z = value.Y; C2.Z = value.Z; }
        }

        public Vector3 R3
        {
            get { return new Vector3(C0.W, C1.W, C2.W); }
            set { C0.W = value.X; C1.W = value.Y; C2.W = value.Z; }
        }

        public float this[int row, int col]
        {
            get
            {
                switch (col)
                {
                    case 0: return C0[row];
                    case 1: return C1[row];
                    case 2: return C2[row];
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
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Matrix4x4 ToMatrix4x4()
        {
            return new Matrix4x4(C0, C1, C2, Vector4.UnitW);
        }

        #region Equality, Hash, String
        public override int GetHashCode()
        {
            return 
                C0.GetHashCode() ^
                (C1.GetHashCode() << 1) ^
                (C2.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return string.Format("{{ {0}, {1}, {2}, {3} }}",
                R0.ToString(),
                R1.ToString(),
                R2.ToString(),
                R3.ToString());
        }

        public static bool Equals(Matrix4x3 s1, Matrix4x3 s2)
        {
            return 
                s1.C0 == s2.C0 &&
                s1.C1 == s2.C1 &&
                s1.C2 == s2.C2;
        }

        public static bool operator ==(Matrix4x3 s1, Matrix4x3 s2) { return Equals(s1, s2); }
        public static bool operator !=(Matrix4x3 s1, Matrix4x3 s2) { return !Equals(s1, s2); }
        public bool Equals(Matrix4x3 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Matrix4x3 && Equals((Matrix4x3)obj); }
        #endregion

        #region Math
        #region DeterminantOptimizers
        private float D0123 { get { return M20 * M31 - M21 * M30; } }
        private float D0223 { get { return M10 * M31 - M11 * M30; } }
        private float D0303 { get { return M11 * M22 - M12 * M21; } }
        private float D0313 { get { return M10 * M22 - M12 * M20; } }
        private float D0323 { get { return M10 * M21 - M11 * M20; } }
        private float D1223 { get { return M00 * M31 - M01 * M30; } }
        private float D1303 { get { return M01 * M22 - M02 * M21; } }
        private float D1313 { get { return M00 * M22 - M02 * M20; } }
        private float D1323 { get { return M00 * M21 - M01 * M20; } }
        private float D2303 { get { return M01 * M12 - M02 * M11; } }
        private float D2313 { get { return M00 * M12 - M02 * M10; } }
        private float D2323 { get { return M00 * M11 - M01 * M10; } }

        private float D00 { get { return D0303; } }
        private float D01 { get { return D0313; } }
        private float D02 { get { return D0323; } }
        private float D03 { get { return M12 * D0123 - M22 * D0223 + M32 * D0323; } }
        private float D10 { get { return D1303; } }
        private float D11 { get { return D1313; } }
        private float D12 { get { return D1323; } }
        private float D13 { get { return M02 * D0123 - M22 * D1223 + M32 * D1323; } }
        private float D20 { get { return D2303; } }
        private float D21 { get { return D2313; } }
        private float D22 { get { return D2323; } }
        private float D23 { get { return M02 * D0223 - M12 * D1223 + M32 * D2323; } }
        private float D33 { get { return M02 * D0323 - M12 * D1323 + M22 * D2323; } }
        #endregion

        public float Determinant()
        {
            return D33;
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
                M03 = 0.0f,
                M13 = 0.0f,
                M23 = 0.0f,
                M33 = 1.0f
            };
        }

        public static Matrix4x3 operator -(Matrix4x3 m) { return m.Negate(); }
        public Matrix4x3 Negate()
        {
            return new Matrix4x3(-C0, -C1, -C2);
        }

        public static Matrix4x3 operator +(Matrix4x3 m1, Matrix4x3 m2) { return Add(m1, m2); }
        public static Matrix4x3 Add(Matrix4x3 m1, Matrix4x3 m2)
        {
            return new Matrix4x3(
                m1.C0 + m2.C0,
                m1.C1 + m2.C1,
                m1.C2 + m2.C2);
        }

        public static Matrix4x3 operator -(Matrix4x3 left, Matrix4x3 right) { return Subtract(left, right); }
        public static Matrix4x3 Subtract(Matrix4x3 left, Matrix4x3 right)
        {
            return new Matrix4x3(
                left.C0 - right.C0,
                left.C1 - right.C1,
                left.C2 - right.C2);
        }

        public static Vector4 operator *(Vector4 v, Matrix4x3 m) { return Multiply(v, m); }
        public static Vector4 Multiply(Vector4 v, Matrix4x3 m)
        {
            return new Vector4(
                Vector4.Dot(v, m.C0),
                Vector4.Dot(v, m.C1),
                Vector4.Dot(v, m.C2),
                v.W);
        }

        public static Matrix4x3 operator *(Matrix4x3 left, Matrix4x3 right) { return Multiply(left, right); }
        public static Matrix4x3 Multiply(Matrix4x3 left, Matrix4x3 right)
        {
            return new Matrix4x3
            {
                M00 = Vector3.Dot(left.R0, right.C0.Xyz),
                M10 = Vector3.Dot(left.R1, right.C0.Xyz),
                M20 = Vector3.Dot(left.R2, right.C0.Xyz),
                M30 = Vector3.Dot(left.R3, right.C0.Xyz) + right.C0.W,

                M01 = Vector3.Dot(left.R0, right.C1.Xyz),
                M11 = Vector3.Dot(left.R1, right.C1.Xyz),
                M21 = Vector3.Dot(left.R2, right.C1.Xyz),
                M31 = Vector3.Dot(left.R3, right.C1.Xyz) + right.C1.W,

                M02 = Vector3.Dot(left.R0, right.C2.Xyz),
                M12 = Vector3.Dot(left.R1, right.C2.Xyz),
                M22 = Vector3.Dot(left.R2, right.C2.Xyz),
                M32 = Vector3.Dot(left.R3, right.C2.Xyz) + right.C2.W
            };
        }
        #endregion

        #region Constant Matrices
        public static Matrix4x3 Zero
        {
            get { return new Matrix4x3(); }
        }

        public static Matrix4x3 Identity
        {
            get { return new Matrix4x3 { M00 = 1f, M11 = 1f, M22 = 1f }; }
        }
        #endregion

        #region Typical transforms
        public static Matrix4x3 Scaling(float scale) { return Scaling(scale, scale, scale); }
        public static Matrix4x3 Scaling(float sx, float sy, float sz)
        {
            return new Matrix4x3 { M00 = sx, M11 = sy, M22 = sz };
        }

        public static Matrix4x3 RotationX(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix4x3
            {
                M00 = 1f,
                M11 = cosa, M12 = -sina,
                M21 = sina, M22 = cosa
            };
        }

        public static Matrix4x3 RotationY(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix4x3
            {
                M00 = cosa, M02 = -sina,
                M11 = 1f,
                M22 = cosa, M20 = sina
            };
        }

        public static Matrix4x3 RotationZ(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix4x3
            {
                M00 = cosa, M01 = -sina,
                M11 = cosa, M10 = sina,
                M22 = 1f
            };
        }

        public static Matrix4x3 Translation(Vector3 translation)
        {
            return new Matrix4x3
            {
                M00 = 1f,
                M11 = 1f,
                M22 = 1f,
                M30 = translation.X,
                M31 = translation.Y,
                M32 = translation.Z
            };
        }
        #endregion
    }
}
