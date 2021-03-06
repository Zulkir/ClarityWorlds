﻿using System;
using System.Diagnostics;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public class RichTextHeadlessEditor : IRichTextHeadlessEditor
    {
        private readonly IRichText text;
        private readonly IRtParagraphStyle defaultParaStyle;
        private readonly IRtSpanStyle defaultSpanStyle;
        private readonly IRtSpanStyle inputSpanStyle;

        private int cursorPos;
        private int? selectionStartPos;

        public RichTextHeadlessEditor(IRichText text)
        {
            this.text = text;
            defaultParaStyle = AmFactory.Create<RtParagraphStyle>();
            defaultSpanStyle = AmFactory.Create<RtSpanStyle>();
            inputSpanStyle = AmFactory.Create<RtSpanStyle>();
            NormalizeText(text, defaultParaStyle, defaultSpanStyle);
            MoveCursor(0, false);
        }

        public IRichText Text => text;
        public int CursorPos => cursorPos;
        public int? SelectionStartPos => selectionStartPos;

        public RtAbsRange? SelectionRange => selectionStartPos.HasValue 
            ? new RtAbsRange(Math.Min(selectionStartPos.Value, cursorPos), Math.Max(selectionStartPos.Value, cursorPos) - 1) 
            : (RtAbsRange?)null;

        public void MoveCursor(int newPos, bool selecting)
        {
            if (newPos < 0 || newPos > text.LayoutTextLength)
                throw new ArgumentOutOfRangeException();
            selectionStartPos = selecting
                ? selectionStartPos ?? cursorPos
                : (int?)null;
            cursorPos = newPos;
            if (cursorPos == selectionStartPos)
                selectionStartPos = null;
            ToRelPos(cursorPos, out _, out var span);
            inputSpanStyle.CopyFrom(span.Style);
        }

        public void MoveCursorSafe(int newPos, bool selecting)
        {
            MoveCursor(MathHelper.Clamp(newPos, 0, text.LayoutTextLength), selecting);
        }

        public void ClearSelection()
        {
            selectionStartPos = null;
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
                    if (span.LayoutTextLength > toSkip)
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

        private static IRtPureSpan CreatePureSpan(IRtSpanStyle styleProto, string initialText = "")
        {
            var span = AmFactory.Create<RtPureSpan>();
            span.Style = styleProto.CloneTyped();
            span.Text = initialText;
            return span;
        }

        private static IRtEmbeddingSpan CreateEmbeddingSpan(IRtSpanStyle styleProto, string embeddingType, string initialSource = "")
        {
            var span = AmFactory.Create<RtEmbeddingSpan>();
            span.Style = styleProto.CloneTyped();
            span.EmbeddingType = embeddingType;
            span.SourceCode = initialSource;
            return span;
        }
        #endregion

        #region Public Methods
        public void InputString(string str)
        {
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                var firstChar = range.FirstCharPos;
                EraseRange(range);
                cursorPos = firstChar;
                ClearSelection();
            }

            var untokenizedStr = str;
            while (untokenizedStr.Length > 0)
            {
                var nextNewLineIndex = untokenizedStr.IndexOf('\n');
                if (nextNewLineIndex == 0)
                {
                    InsertNewLineAt(cursorPos);
                    cursorPos++;
                    untokenizedStr = untokenizedStr.Substring(1);
                }
                else if (nextNewLineIndex != -1)
                {
                    InsertSpanAt(cursorPos, CreatePureSpan(inputSpanStyle, untokenizedStr.Substring(0, nextNewLineIndex)));
                    cursorPos += nextNewLineIndex;
                    untokenizedStr = untokenizedStr.Substring(nextNewLineIndex);
                }
                else
                {
                    InsertSpanAt(cursorPos, CreatePureSpan(inputSpanStyle, untokenizedStr));
                    cursorPos += untokenizedStr.Length;
                    untokenizedStr = "";
                }
            }
        }

        public void InsertEmbedding(string embeddingType, string str)
        {
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                var firstChar = range.FirstCharPos;
                EraseRange(range);
                cursorPos = firstChar;
                ClearSelection();
            }

            var newSpan = CreateEmbeddingSpan(inputSpanStyle, embeddingType, str);
            InsertSpanAt(cursorPos, newSpan);
            cursorPos += newSpan.LayoutTextLength;
        }

        public void Erase()
        {
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                EraseRange(range);
                cursorPos = range.FirstCharPos;
                ClearSelection();
            }
            else if (cursorPos > 0)
            {
                EraseChar(cursorPos - 1);
                cursorPos--;
            }
        }

        public void Tab()
        {
            int firstParaIndex, lastParaIndex;
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                firstParaIndex = ToRelPos(range.FirstCharPos, out _, out _).ParaIndex;
                lastParaIndex = ToRelPos(range.LastCharPos, out _, out _).ParaIndex;
            }
            else
            {
                firstParaIndex = lastParaIndex = ToRelPos(cursorPos, out _, out _).ParaIndex;
            }

            for (var i = firstParaIndex; i <= lastParaIndex; i++)
                text.Paragraphs[i].Style.TabCount++;
        }

        public void ShiftTab()
        {
            int firstParaIndex, lastParaIndex;
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                firstParaIndex = ToRelPos(range.FirstCharPos, out _, out _).ParaIndex;
                lastParaIndex = ToRelPos(range.LastCharPos, out _, out _).ParaIndex;
            }
            else
            {
                firstParaIndex = lastParaIndex = ToRelPos(cursorPos, out _, out _).ParaIndex;
            }

            for (var i = firstParaIndex; i <= lastParaIndex; i++)
            {
                var style = text.Paragraphs[i].Style;
                if (style.TabCount > 0)
                    style.TabCount--;
            }
        }

        public bool CanCopy()
        {
            return SelectionRange.HasValue;
        }

        public string Copy()
        {
            if (!SelectionRange.HasValue) 
                return "";
            var range = SelectionRange.Value;
            return text.LayoutText.Substring(range.FirstCharPos, range.LastCharPos - range.FirstCharPos + 1);
        }

        public string Cut()
        {
            if (!SelectionRange.HasValue) 
                return "";
            var range = SelectionRange.Value;
            var resultText = text.LayoutText.Substring(range.FirstCharPos, range.LastCharPos - range.FirstCharPos + 1);
            Erase();
            return resultText;

        }

        public void Paste(string str)
        {
            InputString(str);
        }

        public bool TryGetParaStyleProp<T>(Func<IRtParagraphStyle, T> getProp, out T prop)
            where T : IEquatable<T>
        {
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                var startRelPos = ToRelPos(range.FirstCharPos, out _, out _);
                var endRelPos = ToRelPos(range.LastCharPos, out _, out _);
                prop = getProp(text.Paragraphs[startRelPos.ParaIndex].Style);
                for (var i = startRelPos.ParaIndex + 1; i <= endRelPos.ParaIndex; i++)
                    if (!getProp(text.Paragraphs[i].Style).Equals(prop))
                        return false;
                return true;
            }
            else
            {
                ToRelPos(cursorPos, out var para, out _);
                prop = getProp(para.Style);
                return true;
            }
        }

        public bool TryGetSpanStyleProp<T>(Func<IRtSpanStyle, T> getProp, out T prop)
            where T : IEquatable<T>
        {
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                var startRelPos = ToRelPos(range.FirstCharPos, out _, out _);
                var endRelPos = ToRelPos(range.LastCharPos, out _, out _);
                prop = getProp(text.Paragraphs[startRelPos.ParaIndex].Spans[startRelPos.SpanIndex].Style);
                foreach (var span in text.EnumerateSpans(new RtRange(startRelPos, endRelPos)).Skip(1))
                    if (!getProp(span.Style).Equals(prop))
                        return false;
                return true;
            }
            else
            {
                prop = getProp(inputSpanStyle);
                return true;
            }
        }

        public void SetParaStyleProp<T>(T prop, Action<IRtParagraphStyle, T> setProp)
        {
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                var startRelPos = ToRelPos(range.FirstCharPos, out _, out _);
                var endRelPos = ToRelPos(range.LastCharPos, out _, out _);
                for (var i = startRelPos.ParaIndex; i <= endRelPos.ParaIndex; i++)
                    setProp(text.Paragraphs[i].Style, prop);
            }
            else
            {
                ToRelPos(cursorPos, out var para, out _);
                setProp(para.Style, prop);
            }
        }

        public void SetSpanStyleProp<T>(T prop, Action<IRtSpanStyle, T> setProp)
        {
            if (SelectionRange.HasValue)
            {
                var range = SelectionRange.Value;
                var startRelPos = ToRelPos(range.FirstCharPos, out _, out _);
                text.SplitSpan(startRelPos, out _);
                var endRelPos = ToRelPos(range.LastCharPos + 1, out _, out _);
                text.SplitSpan(endRelPos, out _);
                var newStartRelPos = ToRelPos(range.FirstCharPos, out _, out _);
                var newEndRelPos = ToRelPos(range.LastCharPos, out _, out _);
                foreach (var span in text.EnumerateSpans(new RtRange(newStartRelPos, newEndRelPos)))
                    setProp(span.Style, prop);
            }
            else
            {
                setProp(inputSpanStyle, prop);
            }
        }
        #endregion

        private void InsertSpanAt(int absPos, IRtSpan span)
        {
            var relPos = ToRelPos(absPos, out var para, out _);
            text.SplitSpan(relPos, out var nextSpanIndex);
            para.Spans.Insert(nextSpanIndex, span);
            Normalize();
        }

        private void InsertNewLineAt(int absPos)
        {
            var relPos = ToRelPos(absPos, out var para, out var span);
            
            int firstSpanIndexToNewPara;
            if (relPos.CharIndex == 0)
                firstSpanIndexToNewPara = relPos.SpanIndex;
            else if (relPos.CharIndex == span.LayoutTextLength)
                firstSpanIndexToNewPara = relPos.SpanIndex + 1;
            else if (span is IRtPureSpan pureSpan)
            {
                var newSpan = CreatePureSpan(span.Style);
                newSpan.Text = pureSpan.Text.SafeSubstring(relPos.CharIndex);
                pureSpan.Text = pureSpan.Text.SafeSubstring(0, relPos.CharIndex);
                para.Spans.Insert(relPos.SpanIndex + 1, newSpan);
                firstSpanIndexToNewPara = relPos.SpanIndex + 1;
            }
            else
                throw new InvalidOperationException("Trying to insert a new line in the middle of a non-pure span");

            var newPara = CreatePara(para.Style, span.Style);
            while (firstSpanIndexToNewPara < para.Spans.Count)
            {
                var spanToMove = para.Spans[firstSpanIndexToNewPara];
                para.Spans.RemoveAt(firstSpanIndexToNewPara);
                newPara.Spans.Add(spanToMove);
            }
            text.Paragraphs.Insert(relPos.ParaIndex + 1, newPara);
            Normalize();
        }

        private void EraseRange(RtAbsRange range)
        {
            Debug.Assert(range.FirstCharPos >= 0, "range.FirstCharPos >= 0");
            Debug.Assert(range.LastCharPos < text.LayoutTextLength);

            var lastCharAbsPos = range.LastCharPos;
            while (lastCharAbsPos >= range.FirstCharPos)
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