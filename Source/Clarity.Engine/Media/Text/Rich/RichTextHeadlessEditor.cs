using System;
using System.Diagnostics;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public class RichTextHeadlessEditor
    {
        private readonly IRichText text;
        private readonly IRtParagraphStyle defaultParaStyle;
        private readonly IRtSpanStyle defaultSpanStyle;

        private int cursorAbsPos;
        private RtAbsRange? selectionRange;

        public RichTextHeadlessEditor(IRichText text)
        {
            this.text = text;
            defaultParaStyle = AmFactory.Create<RtParagraphStyle>();
            defaultSpanStyle = AmFactory.Create<RtSpanStyle>();
            NormalizeText(text, defaultParaStyle, defaultSpanStyle);
        }

        public int CursorAbsPosition
        {
            get => cursorAbsPos;
            set
            {
                if (value < 0 || value > text.LayoutTextLength)
                    throw new ArgumentOutOfRangeException();
                cursorAbsPos = value;
            }
        }

        public RtAbsRange? SelectionRange
        {
            get => selectionRange;
            set
            {
                if (value.HasValue && (value.Value.FirstCharAbsPos < 0 || value.Value.LastCharAbsPos >= text.LayoutTextLength))
                    throw new ArgumentOutOfRangeException();
                selectionRange = value;
            }
        }

        private RtPosition ToRelPos(int absPos, out IRtParagraph para, out IRtSpan span)
        {
            para = text.Paragraphs[0];
            span = para.Spans[0];

            var toSkip = absPos;
            for (var paraIndex = 0; paraIndex < text.Paragraphs.Count; paraIndex++)
            {
                para = text.Paragraphs[paraIndex];
                for (var spanIndex = 0; spanIndex < para.Spans.Count; spanIndex++)
                {
                    span = para.Spans[spanIndex];
                    if (span.LayoutTextLength > absPos)
                        return new RtPosition(paraIndex, spanIndex, toSkip);
                    toSkip -= span.LayoutTextLength;
                }

                if (toSkip == 0)
                {
                    var lastSpanIndex = para.Spans.Count - 1;
                    var lastSpan = para.Spans[lastSpanIndex];
                    return new RtPosition(paraIndex, lastSpanIndex, lastSpan.LayoutTextLength);
                }

                toSkip--;
            }

            throw new ArgumentOutOfRangeException();
        }

        private void GetParaAndSpan(RtPosition pos, out IRtParagraph para, out IRtSpan span)
        {
            para = text.Paragraphs[pos.ParaIndex];
            span = para.Spans[pos.SpanIndex];
        }

        #region Normalization
        private void Normalize()
        {
            var prevParaStyle = text.Paragraphs.FirstOrDefault()?.Style ?? defaultParaStyle;
            var prevSpanStyle = text.Paragraphs.SelectMany(x => x.Spans).FirstOrDefault()?.Style ?? defaultSpanStyle;
            NormalizeText(text, prevParaStyle, prevSpanStyle);
        }

        private static void NormalizeText(IRichText text, IRtParagraphStyle defaultParaStyle, IRtSpanStyle defaultSpanStyle)
        {
            if (text.Paragraphs.IsEmptyL())
                text.Paragraphs.Add(CreatePara(defaultParaStyle, defaultSpanStyle));
            NormalizePara(text.Paragraphs[0], defaultSpanStyle);
            for (var i = 1; i < text.Paragraphs.Count; i++)
                NormalizePara(text.Paragraphs[i], text.Paragraphs[i-1].Spans.Last().Style);
        }

        private static void NormalizePara(IRtParagraph para, IRtSpanStyle prevSpanStyle)
        {
            if (para.LayoutTextLength == 0)
                if (para.Spans.IsEmptyL())
                    para.Spans.Add(CreatePureSpan(prevSpanStyle));
                else
                    for (var i = para.Spans.Count - 1; i >= 1; i--)
                        para.Spans.RemoveAt(i);
            else
                for (var i = para.Spans.Count - 1; i >= 0; i--)
                    if (para.Spans[i].IsEmpty)
                        para.Spans.RemoveAt(i);
            for (var i = para.Spans.Count - 2; i >= 0; i--)
            {
                if (para.Spans[i] is IRtPureSpan span1 && 
                    para.Spans[i + 1] is IRtPureSpan span2 && 
                    span1.Style.Equals(span2.Style))
                {
                    span1.Text += span2.Text;
                    para.Spans.RemoveAt(i + 1);
                }
            }
        }
        #endregion

        #region Creation
        private static IRtParagraph CreatePara(IRtParagraphStyle paraStyleProto, IRtSpanStyle spanStyleProto)
        {
            var para = AmFactory.Create<RtParagraph>();
            para.Style = paraStyleProto.CloneTyped();
            para.Spans.Add(CreatePureSpan(spanStyleProto));
            return para;
        }

        private static IRtPureSpan CreatePureSpan(IRtSpanStyle styleProto)
        {
            var span = AmFactory.Create<RtPureSpan>();
            span.Style = styleProto.CloneTyped();
            span.Text = "";
            return span;
        }
        #endregion

        public void InsertString(string str)
        {
            if (selectionRange.HasValue)
            {
                var firstChar = selectionRange.Value.FirstCharAbsPos;
                EraseRange(selectionRange.Value);
                cursorAbsPos = firstChar;
                selectionRange = null;
            }

            InsertStringTo(cursorAbsPos, str);
            cursorAbsPos += str.Length;
        }

        private void InsertStringTo(int absPos, string str)
        {
            var relPos = ToRelPos(absPos, out var para, out var span);
            if (span is IRtPureSpan pureSpan)
            {
                pureSpan.Text = pureSpan.Text.SafeSubstring(0, relPos.CharIndex) +
                                str +
                                pureSpan.Text.SafeSubstring(relPos.CharIndex);
            }
            else
            {
                var newSpan = CreatePureSpan(span.Style);
                newSpan.Text = str;
                if (relPos.CharIndex == 0)
                    para.Spans.Insert(relPos.SpanIndex, newSpan);
                else if (relPos.CharIndex == span.LayoutTextLength)
                    para.Spans.Insert(relPos.SpanIndex + 1, newSpan);
                else
                    throw new InvalidOperationException("Trying to insert a string in the middle of a non-pure span");
                Normalize();
            }
        }

        private void EraseRange(RtAbsRange range)
        {
            Debug.Assert(range.FirstCharAbsPos >= 0, "range.FirstCharAbsPos >= 0");
            Debug.Assert(range.LastCharAbsPos < text.LayoutTextLength);
            var lastCharAbsPos = range.LastCharAbsPos;
            while (lastCharAbsPos > range.FirstCharAbsPos)
            {
                EraseChar(lastCharAbsPos);
                lastCharAbsPos--;
            }
        }

        private void EraseChar(int absPos)
        {
            var lastCharRelPos = ToRelPos(absPos, out var para, out var span);
            if (lastCharRelPos.CharIndex < span.LayoutTextLength)
            {
                if (span.LayoutTextLength == 1)
                {
                    para.Spans.RemoveAt(lastCharRelPos.SpanIndex);
                    Normalize();
                }
                else if (span is IRtPureSpan pureSpan)
                {
                    pureSpan.Text = span.LayoutText.SafeSubstring(0, lastCharRelPos.CharIndex) +
                                    span.LayoutText.SafeSubstring(lastCharRelPos.CharIndex + 1);
                }
                else
                {
                    throw new InvalidOperationException("Trying to partially delete a non-pure span");
                }
            }
            else
            {
                // char to delete is '\n'
                var nextParaIndex = lastCharRelPos.ParaIndex + 1;
                Debug.Assert(nextParaIndex < text.Paragraphs.Count, "nextParaIndex < text.Paragraphs.Count");
                var nextPara = text.Paragraphs[nextParaIndex];
                text.Paragraphs.RemoveAt(nextParaIndex);
                // todo: simplify when text is no longer ActiveModel
                while (nextPara.Spans.HasItemsL())
                {
                    var spanToMove = nextPara.Spans[0];
                    nextPara.Spans.RemoveAt(0);
                    para.Spans.Add(spanToMove);
                }
                Normalize();
            }
        }
    }
}