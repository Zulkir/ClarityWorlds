using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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

        public static void Swap<T>(List<T> list, int index1, int index2)
        {
            var t = list[index1];
            list[index1] = list[index2];
            list[index2] = t;
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

        public static PropertyInfo GetPropertyInfo(LambdaExpression propertyExpression) =>
            (PropertyInfo)((MemberExpression)propertyExpression.Body).Member;

    }
}