namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Common
{
    public static class StringExtensions
    {
        public static string SafeSubstring(this string s, int startIndex) => 
            startIndex >= s.Length ? "" : s.Substring(startIndex);

        public static string SafeSubstring(this string s, int startIndex, int length) => 
            startIndex + length > s.Length
                ? s.SafeSubstring(startIndex)
                : s.Substring(startIndex, length);
    }
}