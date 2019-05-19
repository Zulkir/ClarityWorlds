using System;

namespace Clarity.Common.CodingUtilities
{
    public static class CodingHelper
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            var t = a;
            a = b;
            b = t;
        }

        public static bool HasFlag(int value, int flag) => (value & flag) == flag;

        public static T EnumParse<T>(string str) => (T)Enum.Parse(typeof(T), str);

        public static void UpdateIfLess(ref float variable, float newValue)
        {
            if (newValue < variable)
                variable = newValue;
        }

        public static void UpdateIfGreater(ref float variable, float newValue)
        {
            if (newValue > variable)
                variable = newValue;
        }

        public static void UpdateIfLess(ref int variable, int newValue)
        {
            if (newValue < variable)
                variable = newValue;
        }

        public static void UpdateIfGreater(ref int variable, int newValue)
        {
            if (newValue > variable)
                variable = newValue;
        }
    }
}