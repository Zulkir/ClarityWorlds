using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 8)]
    public struct Bool32Vector2 : IEquatable<Bool32Vector2>
    {
        public Bool32 X;
        public Bool32 Y;

        public Bool32Vector2(Bool32 x, Bool32 y)
        {
            X = x; Y = y;
        }

        public Bool32Vector2(bool x, bool y)
        {
            X = (Bool32)x; Y = (Bool32)y;
        }

        #region Equality, Hash, String

        public override int GetHashCode()
        {
            return 
                X.GetHashCode() ^
                (Y.GetHashCode() << 1);
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}",
                X.ToString(),
                Y.ToString());
        }

        public static bool Equals(Bool32Vector2 s1, Bool32Vector2 s2)
        {
            return 
                s1.X == s2.X &&
                s1.Y == s2.Y;
        }

        public static bool operator ==(Bool32Vector2 s1, Bool32Vector2 s2) { return Equals(s1, s2); }
        public static bool operator !=(Bool32Vector2 s1, Bool32Vector2 s2) { return !Equals(s1, s2); }
        public bool Equals(Bool32Vector2 other) { return Equals(this, other); }
        public override bool Equals(object obj) { return obj is Bool32Vector2 && Equals((Bool32Vector2)obj); }

        #endregion

    }
}
