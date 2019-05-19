using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    public struct Bool32Vector4 : IEquatable<Bool32Vector4>
    {
        public Bool32 X;
        public Bool32 Y;
        public Bool32 Z;
        public Bool32 W;

        public Bool32Vector4(Bool32 x, Bool32 y, Bool32 z, Bool32 w)
        {
            X = x; Y = y; Z = z; W = w;
        }

        public Bool32Vector4(bool x, bool y, bool z, bool w)
        {
            X = (Bool32)x; Y = (Bool32)y; Z = (Bool32)z; W = (Bool32)w;
        }

        #region Equality, Hash, String

        public override int GetHashCode()
        {
            return 
                X.GetHashCode() ^
                (Y.GetHashCode() << 1) ^
                (Z.GetHashCode() << 2) ^
                (W.GetHashCode() << 3);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}, {3}}}",
                X.ToString(),
                Y.ToString(),
                Z.ToString(),
                W.ToString());
        }

        public static bool Equals(Bool32Vector4 s1, Bool32Vector4 s2)
        {
            return 
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z &&
                s1.W == s2.W;
        }

        public static bool operator ==(Bool32Vector4 s1, Bool32Vector4 s2) { return Equals(s1, s2); }
        public static bool operator !=(Bool32Vector4 s1, Bool32Vector4 s2) { return !Equals(s1, s2); }
        public bool Equals(Bool32Vector4 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Bool32Vector4 && Equals((Bool32Vector4)obj); }

        #endregion


    }
}
