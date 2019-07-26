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

        public bool TryGetSpanAt(Vector2 point, out RichTextBoxLayoutSpan lspan)
        {
            return TryGetClosestSpanAt(point, out lspan) && lspan.Bounds.ContainsPoint(point);
        }

        public int GetPosition(Vector2 point)
        {
            return TryGetClosestSpanAt(point, out var lspan)
                ? GetPositionInLspan(point, lspan) // todo: use preference
                : 0;
        }

        private bool TryGetClosestSpanAt(Vector2 point, out RichTextBoxLayoutSpan lspan)
        {
            if (LayoutSpans.Count == 0)
            {
                lspan = default(RichTextBoxLayoutSpan);
                return false;
            }
            lspan = LayoutSpans.Minimal(x => x.DistanceFrom(point));
            return true;
        }

        private static int GetPositionInLspan(Vector2 point, RichTextBoxLayoutSpan lspan)
        {
            return lspan.TextAbsPosition + lspan.ClosestIndexFor(point.X);
        }

        public void GetCursorPoint(int pos, out Vector2 point, out float height)
        {
            GetLspanFor(pos, out var lspan, out _, out var charIndexInLspan);
            var xOffset = charIndexInLspan < lspan.CharOffsets.Count
                ? lspan.CharOffsets[charIndexInLspan]
                : lspan.Bounds.Width;
            point = lspan.Bounds.MinMax + new Vector2(xOffset, 0);
            height = lspan.Bounds.Height;
        }

        public IRtSpanStyle GetSpanStyleAt(int pos)
        {
            GetLspanFor(pos, out var lspan, out _, out _);
            return lspan.Style;
        }

        public IEnumerable<AaRectangle2> GetSelectionRectangles(RtAbsRange range)
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
        
        public bool TryGetDown(int pos, out int newPos)
        {
            GetCursorPoint(pos, out var point, out _);
            var newPoint = point + new Vector2(0, 2);
            newPos = GetPosition(newPoint);
            return pos != newPos;
        }

        public bool TryGetUp(int pos, out int newPos)
        {
            GetLspanFor(pos, out var lspan, out _, out _);
            GetCursorPoint(pos, out var point, out _);
            var newPoint = point - new Vector2(0, lspan.Strip.Height + 2);
            newPos = GetPosition(newPoint);
            return pos != newPos;
        }

        private void GetLspanFor(int pos, out RichTextBoxLayoutSpan lspan, out int lspanIndex, out int charIndexInLspan)
        {
            var nextLspanIndex = Enumerable.Range(0, LayoutSpans.Count).Where(x => pos < LayoutSpans[x].TextAbsPosition).FirstOrNull() ?? LayoutSpans.Count;
            lspanIndex = nextLspanIndex - 1;
            lspan = LayoutSpans[lspanIndex];
            charIndexInLspan = pos - lspan.Text.Length;
        }
    }
}