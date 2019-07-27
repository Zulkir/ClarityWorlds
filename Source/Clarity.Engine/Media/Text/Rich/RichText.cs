using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RichText : AmObjectBase<RichText>, IRichText
    {
        public abstract IList<IRtParagraph> Paragraphs { get; }
        public abstract IRtOverallStyle Style { get; set; }

        public int LayoutTextLength => Paragraphs.Sum(x => x.LayoutTextLength + 1) - 1;
        public string LayoutText => string.Join("\n", Paragraphs.Select(x => x.LayoutText));
        public string RawText => string.Join("\n", Paragraphs.Select(x => x.RawText));
        public string DebugText => string.Join("\n", Paragraphs.Select(x => x.DebugText));

        protected RichText()
        {
            Style = AmFactory.Create<RtOverallStyle>();
            Style.BackgroundColor = new Color4(0.9f, 0.9f, 0.9f);
            //Normalize();
        }

        public IRtParagraph GetPara(RtPosition pos) => Paragraphs[pos.ParaIndex];
        public IRtSpan GetSpan(RtPosition pos) => Paragraphs[pos.ParaIndex].Spans[pos.SpanIndex];

        public void GetParaAndSpan(RtPosition pos, out IRtParagraph para, out IRtSpan span)
        {
            para = Paragraphs[pos.ParaIndex];
            span = para.Spans[pos.SpanIndex];
        }

        public RtRange GetRange(int charOffset, int charLength)
        {
            var firstCharIndex = charOffset;
            var lastCharIndex = charLength > 0 
                ? charOffset + charLength - 1 
                : firstCharIndex;

            var firstCharPos = GetPositionForCharIndex(firstCharIndex, RichTextPositionPreference.NextSpan);
            var lastCharPos = GetPositionForCharIndex(lastCharIndex, RichTextPositionPreference.PreviousSpan);

            return new RtRange(firstCharPos, lastCharPos);
        }


        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public bool TryGetCommonSpanProperty<T>(RtRange range, Func<IRtSpan, T> getProp, out T common)
            where T : IEquatable<T>
        {
            var spans = EnumerateSpans(range);
            var firstSpan = spans.First();
            common = getProp(firstSpan);
            foreach (var span in spans.Skip(1))
            {
                var other = getProp(span);
                if (!common.Equals(other))
                    return false;
            }
            return true;
        }

        public bool TryGetCommonParagraphProperty<T>(RtRange range, Func<IRtParagraph, T> getProp, out T common) 
            where T : IEquatable<T>
        {
            var firstPara = Paragraphs[range.FirstCharPos.ParaIndex];
            common = getProp(firstPara);
            foreach (var para in Paragraphs.Skip(range.FirstCharPos.ParaIndex + 1).Take(range.LastCharPos.ParaIndex - range.FirstCharPos.ParaIndex))
            {
                var other = getProp(para);
                if (!common.Equals(other))
                    return false;
            }
            return true;
        }

        public RtRange SplitRange(RtRange range)
        {
            // todo: bug: need +1 on the end?
            var firstGlobalIndex = GetGlobalIndex(range.FirstCharPos);
            var lastGlobalIndex = GetGlobalIndex(range.LastCharPos);

            SplitSpan(GetPositionForCharIndex(firstGlobalIndex, RichTextPositionPreference.NextSpan), out _);
            SplitSpan(GetPositionForCharIndex(lastGlobalIndex, RichTextPositionPreference.PreviousSpan), out _);

            return new RtRange(
                GetPositionForCharIndex(firstGlobalIndex, RichTextPositionPreference.NextSpan), 
                GetPositionForCharIndex(lastGlobalIndex, RichTextPositionPreference.PreviousSpan));
        }

        public void SplitSpan(RtPosition pos, out int nextSpanIndex)
        {
            GetParaAndSpan(pos, out var para, out var span);
            if (pos.CharIndex == 0)
            {
                nextSpanIndex = pos.SpanIndex;
                return;
            }
            if (pos.CharIndex == span.LayoutTextLength)
            {
                nextSpanIndex = pos.SpanIndex + 1;
                return;
            }
            if (!(span is IRtPureSpan pureSpan))
                throw new InvalidOperationException("Cannot split non-pure spans in the middle");
            var secondSpan = AmFactory.Create<RtPureSpan>();
            secondSpan.Style = span.Style.CloneTyped();
            secondSpan.Text = pureSpan.Text.Substring(pos.CharIndex);
            pureSpan.Text = pureSpan.Text.Substring(0, pos.CharIndex);
            para.Spans.Insert(pos.SpanIndex + 1, secondSpan);
            nextSpanIndex = pos.SpanIndex + 1;
        }

        private static IRtPureSpan CreateSpan(string text, IRtSpanStyle style)
        {
            var span = AmFactory.Create<RtPureSpan>();
            span.Text = text;
            span.Style = style;
            return span;
        }

        public int GetGlobalIndex(RtPosition pos)
        {
            int globalSpanOffset = 0;
            foreach (var para in Paragraphs.Take(pos.ParaIndex))
            {
                foreach (var span in para.Spans)
                    globalSpanOffset += span.LayoutTextLength;
                globalSpanOffset++;
            }
            var exactPara = Paragraphs[pos.ParaIndex];
            foreach (var span in exactPara.Spans.Take(pos.SpanIndex))
                globalSpanOffset += span.LayoutTextLength;

            globalSpanOffset += pos.CharIndex;
            return globalSpanOffset;
        }

        public RtPosition GetPositionForCharIndex(int globalCharIndex, RichTextPositionPreference preference)
        {
            var remainingChars = globalCharIndex;
            for (var paraIndex = 0; paraIndex < Paragraphs.Count; paraIndex++)
            {
                var para = Paragraphs[paraIndex];
                for (var spanIndex = 0; spanIndex < para.Spans.Count; spanIndex++)
                {
                    var span = para.Spans[spanIndex];
                    if (remainingChars > span.LayoutTextLength)
                    {
                        remainingChars -= span.LayoutTextLength;
                        continue;
                    }
                    if (remainingChars < span.LayoutTextLength || spanIndex == para.Spans.Count - 1)
                        return new RtPosition(paraIndex, spanIndex, remainingChars);
                    switch (preference)
                    {
                        case RichTextPositionPreference.ClosestWord:
                            var leftSpanChar = para.Spans[spanIndex].LayoutText.Last();
                            var rightSpanChar = para.Spans[spanIndex + 1].LayoutText.First();
                            if (IsWhitespace(leftSpanChar) && !IsWhitespace(rightSpanChar))
                                return new RtPosition(paraIndex, spanIndex + 1, 0);
                            return new RtPosition(paraIndex, spanIndex, span.LayoutTextLength);
                        case RichTextPositionPreference.PreviousSpan:
                            return new RtPosition(paraIndex, spanIndex, span.LayoutTextLength);
                        case RichTextPositionPreference.NextSpan:
                            return new RtPosition(paraIndex, spanIndex + 1, 0);
                        default:
                            throw new ArgumentOutOfRangeException(nameof(preference), preference, null);
                    }
                }
                remainingChars--;
            }
            throw new ArgumentOutOfRangeException(nameof(globalCharIndex));
        }

        private static bool IsWhitespace(char ch)
        {
            switch (ch)
            {
                case ' ':
                case '　':
                case '\t':
                    return true;
                default:
                    return false;
            }
        }

        public void Normalize()
        {
            if (!Paragraphs.Any())
                Paragraphs.Add(AmFactory.Create<RtParagraph>());
            foreach (var para in Paragraphs)
                para.Normalize();
        }

        public IEnumerable<IRtSpan> EnumerateSpans(RtRange range)
        {
            var firstPara = Paragraphs[range.FirstCharPos.ParaIndex];
            var firstSpan = firstPara.Spans[range.FirstCharPos.SpanIndex];

            var lastPara = Paragraphs[range.LastCharPos.ParaIndex];
            var lastSpan = lastPara.Spans[range.LastCharPos.SpanIndex];

            if (firstSpan == lastSpan)
            {
                yield return firstSpan;
            }
            else if (firstPara == lastPara)
            {
                for (int i = range.FirstCharPos.SpanIndex; i <= range.LastCharPos.SpanIndex; i++)
                    yield return firstPara.Spans[i];
            }
            else
            {
                for (int i = range.FirstCharPos.SpanIndex; i < firstPara.Spans.Count; i++)
                    yield return firstPara.Spans[i];
                for (int i = range.FirstCharPos.ParaIndex + 1; i < range.LastCharPos.ParaIndex; i++)
                    foreach (var span in Paragraphs[i].Spans)
                        yield return span;
                for (int i = 0; i <= range.LastCharPos.SpanIndex; i++)
                    yield return lastPara.Spans[i];
            }
        }
    }
}