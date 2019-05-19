using Clarity.Common.Numericals;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Common
{
    public static class NumeralExtensions
    {
        public static float Sq(this float x) => x * x;
        public static float Pow(this float b, float e) => MathHelper.Pow(b, e);
    }
}