using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    public struct Bool32Vector3 : IEquatable<Bool32Vector3>
    {
        public Bool32 X;
        public Bool32 Y;
        public Bool32 Z;

        public Bool32Vector3(Bool32 x, Bool32 y, Bool32 z)
        {
            X = x; Y = y; Z = z;
        }

        public Bool32Vector3(bool x, bool y, bool z)
        {
            X = (Bool32)x; Y = (Bool32)y; Z = (Bool32)z;
        }

        #region Equality, Hash, String

        public override int GetHashCode()
        {
            return 
                X.GetHashCode() ^
                (Y.GetHashCode() << 1) ^
                (Z.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}}}",
                X.ToString(),
                Y.ToString(),
                Z.ToString());
        }

        public static bool Equals(Bool32Vector3 s1, Bool32Vector3 s2)
        {
            return 
                s1.X == s2.X &&
                s1.Y == s2.Y &&
                s1.Z == s2.Z;
        }

        public static bool operator ==(Bool32Vector3 s1, Bool32Vector3 s2) { return Equals(s1, s2); }
        public static bool operator !=(Bool32Vector3 s1, Bool32Vector3 s2) { return !Equals(s1, s2); }
        public bool Equals(Bool32Vector3 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Bool32Vector3 && Equals((Bool32Vector3)obj); }

        #endregion


    }
}
