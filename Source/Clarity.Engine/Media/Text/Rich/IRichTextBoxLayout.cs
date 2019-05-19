using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichTextBoxLayout
    {
        IReadOnlyList<RichTextBoxLayoutSpan> LayoutSpans { get; }
        IReadOnlyList<RichTextBoxLayoutSpan> ExternalLayoutSpans { get; }
        RtPosition GetPosition(Vector2 point, RichTextPositionPreference preference);
        void GetCursorPoint(RtPosition pos, out Vector2 point, out float height);
        IRtSpanStyle GetSpanStyleAt(RtPosition pos);
        IEnumerable<AaRectangle2> GetSelectionRectangles(RtRange range);
        bool TryGetRight(RtPosition pos, RichTextPositionPreference preference, out RtPosition newPos);
        bool TryGetLeft(RtPosition pos, RichTextPositionPreference preference, out RtPosition newPos);
        bool TryGetDown(RtPosition pos, RichTextPositionPreference preference, out RtPosition newPos);
        bool TryGetUp(RtPosition pos, RichTextPositionPreference preference, out RtPosition newPos);
    }
}