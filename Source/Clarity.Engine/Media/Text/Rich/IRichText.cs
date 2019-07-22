using System;
using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichText : IAmObject
    {
        [NotNull]
        IList<IRtParagraph> Paragraphs { get; }
        [NotNull]
        IRtOverallStyle Style { get; set; }

        int LayoutTextLength { get; }

        string LayoutText { get; }
        string RawText { get; }
        string DebugText { get; }

        void Normalize();

        IRtParagraph GetPara(RtPosition pos);
        IRtSpan GetSpan(RtPosition pos);
        void GetParaAndSpan(RtPosition pos, out IRtParagraph para, out IRtSpan span);

        RtRange GetRange(int firstChar, int charLength);
        
        bool TryGetCommonParagraphProperty<T>(RtRange range, Func<IRtParagraph, T> getProp, out T common) where T : IEquatable<T>;
        bool TryGetCommonSpanProperty<T>(RtRange range, Func<IRtSpan, T> getProp, out T common) where T : IEquatable<T>;

        void SplitSpan(RtPosition pos, out int rightSpanIndex);
        RtRange SplitRange(RtRange range);
        //void ApplySimpleDiff(RtSimpleDiff diff);
        IEnumerable<IRtSpan> EnumerateSpans(RtRange range);

        int GetGlobalIndex(RtPosition pos);
        RtPosition GetPositionForCharIndex(int globalCharIndex, RichTextPositionPreference preference);
    }
}