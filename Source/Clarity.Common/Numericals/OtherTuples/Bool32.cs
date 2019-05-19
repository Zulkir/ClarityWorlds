using System;
using System.Runtime.InteropServices;

namespace Clarity.Common.Numericals.OtherTuples
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 4)]
    public struct Bool32 : IEquatable<Bool32>
    {
        public int Raw;

        public Bool32(bool b)
        {
            Raw = b ? 1 : 0;
        }

        public Bool32(int raw)
        {
            Raw = raw;
        }
        
        public bool IsTrue { get { return Raw != 0; } set { Raw = value ? 1 : 0; } }

        public bool Equals(Bool32 other) { return Equals(this, other); }
        public bool Equals(bool other) { return Equals(this, new Bool32(other)); }
        public override int GetHashCode() { return Raw; }
        public override string ToString() { return IsTrue ? "True" : "False"; }

        public override bool Equals(object obj)
        {
            if (obj is Bool32)
                return Equals((Bool32)obj);
            if (obj is bool)
                return Equals((bool)obj);
            return false;
        }

        public static bool Equals(Bool32 b1, Bool32 b2)
        {
            return b1.IsTrue == b2.IsTrue;
        }

        public static bool operator ==(Bool32 b1, Bool32 b2) { return Equals(b1, b2); }
        public static bool operator !=(Bool32 b1, Bool32 b2) { return !Equals(b1, b2); }

        public static explicit operator bool(Bool32 bool32)
        {
            return bool32.IsTrue;
        }

        public static explicit operator Bool32(bool b)
        {
            return new Bool32(b);
        }

        public static explicit operator int(Bool32 bool32)
        {
            return bool32.Raw;
        }

        public static explicit operator Bool32(int raw)
        {
            return new Bool32(raw);
        }
    }
}
