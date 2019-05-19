using System.Collections.Generic;
using System.Text.RegularExpressions;
using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Engine.Media.Text.Common
{
    public class TextLineBreaker : ITextLineBreaker
    {
        private static string ProhibitedStartLineSymbols { get; } = @"、。，．・：；？！゛゜ヽヾゝゞ々ー’”）〕］｝〉》」』】°‰′″℃￠％ぁぃぅぇぉっゃゅょゎァィゥェォッャュョヮヵヶ!%),\.:;?\]}｡｣､･ｧｨｩｪｫｬｭｮｯｰﾞﾟ";
        private static string ProhibitedEndLineSymbols { get; } = @"‘“（〔［｛〈《「『【￥＄$([\\{｢￡";
        private static Regex ProhibitedStartLineRegex { get; } = new Regex($@"^[^\S\t]*[{ProhibitedStartLineSymbols}]");
        private static Regex ProhibitedEndLineRegex { get; } = new Regex($@"[{ProhibitedEndLineSymbols}][^\S\t]*$");

        private readonly Dictionary<Pair<char>, bool> pairCache = new Dictionary<Pair<char>, bool>();

        public bool CanBreakAt(string paragraphText, int breakPosition)
        {
            if (breakPosition < 1 || breakPosition > paragraphText.Length - 1)
                return false;
            var ch1 = paragraphText[breakPosition - 1];
            var ch2 = paragraphText[breakPosition];
            if (pairCache.TryGetValue(Tuples.SameTypePair(ch1, ch2), out var cachedResult))
                return cachedResult;
            if (ProhibitedStartLineRegex.IsMatch(paragraphText.Substring(breakPosition)))
                return false;
            if (ProhibitedEndLineRegex.IsMatch(paragraphText.Substring(0, breakPosition)))
                return false;
            if (IsWhitespace(ch1) || IsWhitespace(ch2))
                return true;
            bool result;
            if (ch1 == '-')
                result = true;
            else if (IsCjk(ch1) || IsCjk(ch2))
                result = true;
            else
                result = false;
            pairCache.Add(Tuples.SameTypePair(ch1, ch2), result);
            return result;
        }

        private static bool IsCjk(char ch) => 0x2E80 <= ch && ch <= 0x9FFF;
        private static bool IsWhitespace(char ch) => ch == ' ' || ch == '　';
    }
}