using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Engine.Media.Text.Rich
{
    public class RichTextBoxLayout : IRichTextBoxLayout
    {
        private IRichText Text { get; }
        public IReadOnlyList<RichTextBoxLayoutSpan> LayoutSpans { get; }
        public IReadOnlyList<RichTextBoxLayoutSpan> ExternalLayoutSpans { get; }

        public RichTextBoxLayout(IRichText text, IReadOnlyList<RichTextBoxLayoutSpan> lspans, IReadOnlyList<RichTextBoxLayoutSpan> externalLayoutSpans)
        {
            LayoutSpans = lspans;
            ExternalLayoutSpans = externalLayoutSpans;
            Text = text;
        }

        public RtPosition GetPosition(Vector2 point, RichTextPositionPreference preference)
        {
            if (LayoutSpans.Count == 0)
                return new RtPosition(0, 0, 0);
            var closestRect = LayoutSpans.Minimal(x => x.DistanceFrom(point));
            var pos = GetPositionInLspan(point, closestRect);
            // todo: use preference
            return pos;
        }

        private static RtPosition GetPositionInLspan(Vector2 point, RichTextBoxLayoutSpan lspan)
        {
            var lspanOffset = lspan.ClosestIndexFor(point.X);
            var lspanPos = lspan.TextPosition;
            lspanPos.CharIndex += lspanOffset;
            return lspanPos;
        }

        public void GetCursorPoint(RtPosition pos, out Vector2 point, out float height)
        {
            GetLspanFor(pos, out var lspan, out _, out var charIndexInLspan);
            var xOffset = charIndexInLspan < lspan.CharOffsets.Count
                ? lspan.CharOffsets[charIndexInLspan]
                : lspan.Bounds.Width;
            point = lspan.Bounds.MinMax + new Vector2(xOffset, 0);
            height = lspan.Bounds.Height;
        }

        public IRtSpanStyle GetSpanStyleAt(RtPosition pos)
        {
            GetLspanFor(pos, out var lspan, out _, out _);
            return lspan.Style;
        }

        public IEnumerable<AaRectangle2> GetSelectionRectangles(RtRange range)
        {
            GetLspanFor(range.FirstCharPos, out var lspan1, out var lspanIndex1, out var charIndex1);
            GetLspanFor(range.LastCharPos, out var lspan2, out var lspanIndex2, out var charIndex2);
            var startCorner = lspan1.Bounds.MinMin + new Vector2(lspan1.GetCharOffset(charIndex1), 0);
            var endCorner = lspan2.Bounds.MinMax + new Vector2(lspan2.GetCharOffset(charIndex2), 0);
            if (lspanIndex1 == lspanIndex2)
            {
                yield return new AaRectangle2(startCorner, endCorner);
                yield break;
            }
            yield return new AaRectangle2(startCorner, lspan1.Bounds.MaxMax);
            for (var i = lspanIndex1 + 1; i < lspanIndex2; i++)
                yield return LayoutSpans[i].Bounds;
            yield return new AaRectangle2(lspan2.Bounds.MinMin, endCorner);
        }

        public bool TryGetRight(RtPosition pos, RichTextPositionPreference preference, out RtPosition newPos)
        {
            var globalIndex = Text.GetGlobalIndex(pos);

            if (globalIndex == Text.Length)
            {
                newPos =  pos;
                return false;
            }

            newPos = Text.GetPositionForCharIndex(globalIndex + 1, preference);
            return true;
        }

        public bool TryGetLeft(RtPosition pos, RichTextPositionPreference preference, out RtPosition newPos)
        {
            var globalIndex = Text.GetGlobalIndex(pos);

            if (globalIndex == 0)
            {
                newPos = pos;
                return false;
            }

            newPos = Text.GetPositionForCharIndex(globalIndex - 1, preference);
            return true;
        }

        public bool TryGetDown(RtPosition pos, RichTextPositionPreference preference, out RtPosition newPos)
        {
            GetCursorPoint(pos, out var point, out _);
            var newPoint = point + new Vector2(0, 2);
            newPos = GetPosition(newPoint, preference);
            return pos != newPos;
        }

        public bool TryGetUp(RtPosition pos, RichTextPositionPreference preference, out RtPosition newPos)
        {
            GetLspanFor(pos, out var lspan, out _, out _);
            GetCursorPoint(pos, out var point, out _);
            var newPoint = point - new Vector2(0, lspan.Strip.Height + 2);
            newPos = GetPosition(newPoint, preference);
            return pos != newPos;
        }

        private void GetLspanFor(RtPosition pos, out RichTextBoxLayoutSpan lspan, out int lspanIndex, out int charIndexInLspan)
        {
            var nextLspanIndex = Enumerable.Range(0, LayoutSpans.Count).Where(x => pos < LayoutSpans[x].TextPosition).FirstOrNull() ?? LayoutSpans.Count;
            lspanIndex = nextLspanIndex - 1;
            lspan = LayoutSpans[lspanIndex];
            charIndexInLspan = pos.ParaIndex == lspan.TextPosition.ParaIndex && pos.SpanIndex == lspan.TextPosition.SpanIndex
                ? pos.CharIndex - lspan.TextPosition.CharIndex
                : lspan.Text.Length;
        }
    }
}