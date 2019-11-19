using System;

namespace Clarity.Common.Numericals
{
    public static class MathHelper
    {
        public const float Eps5 = 0.00001f;
        public const float Eps8 = 0.00000001f;
        public const double DEps8 = 0.00000001;
        public const float Pi = 3.1415926535897932384626433832795f;
        public const float TwoPi = 6.283185307179586476925286766559f;
        public const float PiOver2 = 1.5707963267948966192313216916398f;
        public const float PiOver3 = 1.0471975511965976f;
        public const float PiOver4 = 0.78539816339744830961566084581988f;
        public const float Sqrt2 = 1.4142135623730951f;
        public const float FrustumDistance = 2.414213562373095f;
        public static float Sin(float a) => (float)System.Math.Sin(a);
        public static float Cos(float a) => (float)System.Math.Cos(a);
        public static float Tan(float a) => (float)System.Math.Tan(a);
        public static float Asin(float x) => (float)System.Math.Asin(x);
        public static float AsinSafe(float x) => Asin(Clamp(x, -1f, 1f));
        public static float Acos(float x) => (float)System.Math.Acos(x);
        public static float AcosSafe(float x) => Acos(Clamp(x, -1f, 1f));
        public static float Atan2(float y, float x) => (float)System.Math.Atan2(y, x);
        public static float Pow(float b, float e) => (float)System.Math.Pow(b, e);
        public static float Sqrt(float x) => (float)System.Math.Sqrt(x);
        public static float Exp(float d) => (float)System.Math.Exp(d);
        public static float Ceiling(float f) => (float)Math.Ceiling(f);
        public static float Floor(float f) => (float)Math.Floor(f);
        public static float Lerp(float left, float right, float amount) => left - amount * (left - right);
        public static double Lerp(double left, double right, double amount) => left - amount * (left - right);
        public static int Clamp(int x, int min, int max) => x < min ? min : x > max ? max : x;
        public static float Clamp(float x, float min, float max) => x < min ? min : x > max ? max : x;
        public static float Hermite(float t) => t * t * (3f - 2f * t);
    }
}