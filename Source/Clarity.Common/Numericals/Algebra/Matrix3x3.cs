using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.Algebra
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 36)]
    public struct Matrix3x3 : IEquatable<Matrix3x3>
    {
        public Vector3 C0;
        public Vector3 C1;
        public Vector3 C2;

        #region Constructors
        public Matrix3x3(Vector3 c0, Vector3 c1, Vector3 c2)
        {
            C0 = c0;
            C1 = c1;
            C2 = c2;
        }
        #endregion

        public float M00 { get { return C0.X; } set { C0.X = value; } }
        public float M10 { get { return C0.Y; } set { C0.Y = value; } }
        public float M20 { get { return C0.Z; } set { C0.Z = value; } }

        public float M01 { get { return C1.X; } set { C1.X = value; } }
        public float M11 { get { return C1.Y; } set { C1.Y = value; } }
        public float M21 { get { return C1.Z; } set { C1.Z = value; } }

        public float M02 { get { return C2.X; } set { C2.X = value; } }
        public float M12 { get { return C2.Y; } set { C2.Y = value; } }
        public float M22 { get { return C2.Z; } set { C2.Z = value; } }

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
            return string.Format("{{ {0}, {1}, {2} }}",
                R0.ToString(),
                R1.ToString(),
                R2.ToString());
        }

        public static bool Equals(Matrix3x3 s1, Matrix3x3 s2)
        {
            return 
                s1.C0 == s2.C0 &&
                s1.C1 == s2.C1 &&
                s1.C2 == s2.C2;
        }

        public static bool operator ==(Matrix3x3 s1, Matrix3x3 s2) { return Equals(s1, s2); }
        public static bool operator !=(Matrix3x3 s1, Matrix3x3 s2) { return !Equals(s1, s2); }
        public bool Equals(Matrix3x3 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Matrix3x3 && Equals((Matrix3x3)obj); }
        #endregion

        #region Math
        public Matrix3x3 Transpose()
        {
            return new Matrix3x3(R0, R1, R2);
        }

        #region Determinant Optimizers
        private float D00 { get { return M11 * M22 - M12 * M21; } }
        private float D01 { get { return M10 * M22 - M12 * M20; } }
        private float D02 { get { return M10 * M21 - M11 * M20; } }
        private float D10 { get { return M01 * M22 - M02 * M21; } }
        private float D11 { get { return M00 * M22 - M02 * M20; } }
        private float D12 { get { return M00 * M21 - M01 * M20; } }
        private float D20 { get { return M01 * M12 - M02 * M11; } }
        private float D21 { get { return M00 * M12 - M02 * M10; } }
        private float D22 { get { return M00 * M11 - M01 * M10; } }
        #endregion

        public float Determinant()
        {
            return M00 * D00 - M01 * D01 + M02 * D02;
        }

        public Matrix3x3 Invert()
        {
            float invDet = 1f / Determinant();
            return new Matrix3x3
            {
                M00 = D00 * invDet,
                M10 = -D01 * invDet,
                M20 = D02 * invDet,
                M01 = -D10 * invDet,
                M11 = D11 * invDet,
                M21 = -D12 * invDet,
                M02 = D20 * invDet,
                M12 = -D21 * invDet,
                M22 = D22 * invDet
            };
        }

        public static Matrix3x3 operator -(Matrix3x3 m) { return m.Negate(); }
        public Matrix3x3 Negate()
        {
            return new Matrix3x3(-C0, -C1, -C2);
        }

        public static Matrix3x3 operator +(Matrix3x3 m1, Matrix3x3 m2) { return Add(m1, m2); }
        public static Matrix3x3 Add(Matrix3x3 m1, Matrix3x3 m2)
        {
            return new Matrix3x3(
                m1.C0 + m2.C0,
                m1.C1 + m2.C1,
                m1.C2 + m2.C2);
        }

        public static Matrix3x3 operator -(Matrix3x3 m1, Matrix3x3 m2) { return Subtract(m1, m2); }
        public static Matrix3x3 Subtract(Matrix3x3 left, Matrix3x3 right)
        {
            return new Matrix3x3(
                left.C0 - right.C0,
                left.C1 - right.C1,
                left.C2 - right.C2);
        }

        public static Matrix3x3 operator *(float scale, Matrix3x3 m) { return m.ScaleBy(scale); }
        public static Matrix3x3 operator *(Matrix3x3 m, float scale) { return m.ScaleBy(scale); }
        public Matrix3x3 ScaleBy(float scale)
        {
            return new Matrix3x3(
                C0.ScaleBy(scale),
                C1.ScaleBy(scale),
                C2.ScaleBy(scale));
        }

        public static Vector3 operator *(Vector3 v, Matrix3x3 m) { return Multiply(v, m); }
        public static Vector3 Multiply(Vector3 v, Matrix3x3 m)
        {
            return new Vector3(
                Vector3.Dot(v, m.C0),
                Vector3.Dot(v, m.C1),
                Vector3.Dot(v, m.C2));
        }

        public static Matrix3x3 operator *(Matrix3x3 left, Matrix3x3 right) { return Multiply(left, right); }
        public static Matrix3x3 Multiply(Matrix3x3 left, Matrix3x3 right)
        {
            return new Matrix3x3
            {
                R0 = left.R0 * right,
                R1 = left.R1 * right,
                R2 = left.R2 * right
            };
        }
        
        #endregion

        #region Constant Matrices
        public static Matrix3x3 Zero
        {
            get { return new Matrix3x3(); }
        }

        public static Matrix3x3 Identity
        {
            get { return new Matrix3x3 { M00 = 1f, M11 = 1f, M22 = 1f }; }
        }
        #endregion

        #region Typical transforms
        public static Matrix3x3 Scaling(float scale) { return Scaling(scale, scale, scale); }
        public static Matrix3x3 Scaling(float sx, float sy, float sz)
        {
            return new Matrix3x3 { M00 = sx, M11 = sy, M22 = sz };
        }

        public static Matrix3x3 RotationX(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix3x3
            {
                M00 = 1f,
                M11 = cosa, M12 = -sina,
                M21 = sina, M22 = cosa
            };
        }

        public static Matrix3x3 RotationY(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix3x3
            {
                M00 = cosa, M02 = -sina,
                M11 = 1f,
                M22 = cosa, M20 = sina
            };
        }

        public static Matrix3x3 RotationZ(float angle)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            return new Matrix3x3
            {
                M00 = cosa, M01 = -sina,
                M11 = cosa, M10 = sina,
                M22 = 1f
            };
        }

        public static Matrix3x3 RotationAxis(Vector3 axis, float angle)
        {
            axis = axis.Normalize();
            
            var sin = MathHelper.Sin(angle);
            var cos = MathHelper.Cos(angle);
            var ic = 1.0f - cos;

            return new Matrix3x3
            {
                M00 = axis.X * axis.X * ic + cos,
                M01 = axis.Y * axis.X * ic + axis.Z * sin,
                M02 = axis.Z * axis.X * ic - axis.Y * sin,
                M10 = axis.X * axis.Y * ic - axis.Z * sin,
                M11 = axis.Y * axis.Y * ic + cos,
                M12 = axis.Z * axis.Y * ic + axis.X * sin,
                M20 = axis.X * axis.Z * ic + axis.Y * sin,
                M21 = axis.Y * axis.Z * ic - axis.X * sin,
                M22 = axis.Z * axis.Z * ic + cos
            };
        }
        #endregion
    }
}
