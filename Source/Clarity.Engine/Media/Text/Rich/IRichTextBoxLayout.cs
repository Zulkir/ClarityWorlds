using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichTextBoxLayout
    {
        IReadOnlyList<RichTextBoxLayoutSpan> LayoutSpans { get; }
        IReadOnlyList<RichTextBoxLayoutSpan> ExternalLayoutSpans { get; }
        bool TryGetSpanAt(Vector2 point, out RichTextBoxLayoutSpan lspan);
        int GetPosition(Vector2 point);
        void GetCursorPoint(int pos, out Vector2 point, out float height);
        IRtSpanStyle GetSpanStyleAt(int pos);
        IEnumerable<AaRectangle2> GetSelectionRectangles(RtAbsRange range);

        bool TryGetDown(int pos, out int newPos);
        bool TryGetUp(int pos, out int newPos);
    }
}