using System;
using System.Linq;
using System.Text;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Media.Media2D
{
    public class RichTextEditInteractionElement : IInteractionElement
    {
        private readonly IRichTextComponent cText;
        private readonly IInputHandler inputHandler;
        private readonly IUndoRedoService undoRedo;
        private readonly IClipboard clipboard;

        public RichTextEditInteractionElement(IRichTextComponent cText, IInputHandler inputHandler, IUndoRedoService undoRedo, IClipboard clipboard)
        {
            this.cText = cText;
            this.undoRedo = undoRedo;
            this.clipboard = clipboard;
            this.inputHandler = inputHandler;
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (args is IMouseEventArgs mouseArgs)
                return TryHandleMouseEvent(mouseArgs);
            if (args is IKeyEventArgs keyboardArgs)
                return TryHandleKeyEvent(keyboardArgs);
            return false;
        }

        private bool TryHandleKeyEvent(IKeyEventArgs args)
        {
            if (!args.HasFocus)
                return false;

            var textBox = cText.TextBox;
            var text = textBox.Text;
            var layout = textBox.Layout;

            if (args.ComplexEventType == KeyEventType.TextInput)
            {
                InputStr(args.Text);
                undoRedo.OnChange();
                return true;
            }

            if (args.ComplexEventType == KeyEventType.Down)
            {
                switch (args.EventKey)
                {
                    case Key.Left:
                        {
                            if (layout.TryGetLeft(cText.CursorPosition, RichTextPositionPreference.ClosestWord, out var newPos))
                            {
                                cText.SelectionStart = args.KeyModifyers.HasFlag(KeyModifyers.Shift) 
                                    ? cText.SelectionStart ?? cText.CursorPosition 
                                    : (RtPosition?)null;
                                MoveCursorUpdatingInputStyle(newPos);
                            }
                            
                            return true;
                        }
                    case Key.Right:
                        {
                            if (layout.TryGetRight(cText.CursorPosition, RichTextPositionPreference.ClosestWord, out var newPos))
                            {
                                cText.SelectionStart = args.KeyModifyers.HasFlag(KeyModifyers.Shift) 
                                    ? cText.SelectionStart ?? cText.CursorPosition 
                                    : (RtPosition?)null;
                                MoveCursorUpdatingInputStyle(newPos);
                            }
                            return true;
                        }
                    case Key.Up:
                        {
                            if (layout.TryGetUp(cText.CursorPosition, RichTextPositionPreference.ClosestWord, out var newPos))
                            {
                                cText.SelectionStart = args.KeyModifyers.HasFlag(KeyModifyers.Shift) 
                                    ? cText.SelectionStart ?? cText.CursorPosition 
                                    : (RtPosition?)null;
                                MoveCursorUpdatingInputStyle(newPos);
                            }
                            return true;
                        }
                    case Key.Down:
                        {
                            if (layout.TryGetDown(cText.CursorPosition, RichTextPositionPreference.ClosestWord, out var newPos))
                            {
                                cText.SelectionStart = args.KeyModifyers.HasFlag(KeyModifyers.Shift) 
                                    ? cText.SelectionStart ?? cText.CursorPosition 
                                    : (RtPosition?)null;
                                MoveCursorUpdatingInputStyle(newPos);
                            }
                            return true;
                        }
                    case Key.Enter:
                        {
                            EnterNewLine();
                            return true;
                        }
                    case Key.Backspace:
                        {
                            if (cText.SelectionRange.HasValue)
                            {
                                EraseRange(cText.SelectionRange.Value);
                                MoveCursorUpdatingInputStyle(cText.SelectionRange.Value.FirstCharPos);
                                cText.SelectionStart = null;
                            }
                            else
                            {
                                var cp = cText.CursorPosition;
                                EraseSingleChar(ref cp);
                                MoveCursorUpdatingInputStyle(cp);
                            }
                            return true;
                        }
                    case Key.Tab:
                        if (cText.CursorPosition.SpanIndex != 0 || cText.CursorPosition.CharIndex != 0)
                            return true;
                        var paraStyle = text.Paragraphs[cText.CursorPosition.ParaIndex].Style;
                        if (args.KeyModifyers == KeyModifyers.None)
                            paraStyle.TabCount++;
                        else if (args.KeyModifyers == KeyModifyers.Shift && paraStyle.TabCount > 0)
                            paraStyle.TabCount--;
                        return true;
                }
            }
            return false;
        }

        private bool TryHandleMouseEvent(IMouseEventArgs args)
        {
            // todo: refactor for better Rectangle modification code
            var placementSurface = cText.Node.PresentationInfra().Placement;
            if (placementSurface == null)
                return false;
            var globalRay = args.Viewport.GetGlobalRayForPixelPos(args.State.Position);
            if (!placementSurface.PlacementSurface2D.TryFindPoint2D(globalRay, out var point2D))
                return false;
            var cRect = cText.Node.GetComponent<IRectangleComponent>();
            var rect = cRect.Rectangle;
            var textBox = cText.TextBox;
            var textBoxPointYswapped = (point2D - rect.MinMax) * textBox.PixelScaling;
            var textBoxPoint = new Vector2(textBoxPointYswapped.X, -textBoxPointYswapped.Y);
            var layout = textBox.Layout;

            if (args.IsLeftDownEvent())
            {
                cText.SelectionStart = args.KeyModifyers.HasFlag(KeyModifyers.Shift) 
                    ? cText.SelectionStart ?? cText.CursorPosition 
                    : (RtPosition?)null;
                MoveCursorUpdatingInputStyle(layout.GetPosition(textBoxPoint, RichTextPositionPreference.ClosestWord));
                inputHandler.AddLock(new InputLock<object>(null, MouseDownLockProc));
                return true;
            }

            return false;
        }

        private InputEventProcessResult MouseDownLockProc(object o, IInputEventArgs inputEventArgs)
        {
            if (!(inputEventArgs is IMouseEventArgs args))
                return InputEventProcessResult.DontCare;
            if (args.IsOfType(MouseEventType.Move) && args.State.Buttons == MouseButtons.Left)
            {
                // todo: refactor for better Rectangle modification code
                var placementSurface = cText.Node.PresentationInfra().Placement;
                if (placementSurface == null)
                    return InputEventProcessResult.StopPropagating;
                var globalRay = args.Viewport.GetGlobalRayForPixelPos(args.State.Position);
                if (!placementSurface.PlacementSurface2D.TryFindPoint2D(globalRay, out var point2D))
                    return InputEventProcessResult.StopPropagating;
                var cRect = cText.Node.GetComponent<IRectangleComponent>();
                var rect = cRect.Rectangle;
                var textBox = cText.TextBox;
                var textBoxPointYswapped = (point2D - rect.MinMax) * textBox.PixelScaling;
                var textBoxPoint = new Vector2(textBoxPointYswapped.X, -textBoxPointYswapped.Y);
                var layout = textBox.Layout;

                cText.SelectionStart = cText.SelectionStart ?? cText.CursorPosition;
                var preference = cText.CursorPosition == cText.SelectionStart 
                    ? RichTextPositionPreference.ClosestWord 
                    : cText.CursorPosition > cText.SelectionStart 
                        ? RichTextPositionPreference.PreviousSpan 
                        : RichTextPositionPreference.NextSpan;
                MoveCursorUpdatingInputStyle(layout.GetPosition(textBoxPoint, preference));
                return InputEventProcessResult.StopPropagating;
            }
            return InputEventProcessResult.ReleaseLock;
        }

        private void InputStr(string str)
        {
            var textBox = cText.TextBox;
            var text = textBox.Text;

            if (cText.SelectionRange.HasValue)
            {
                var rangeStartPos = cText.SelectionRange.Value.FirstCharPos;
                cText.InputTextStyle = text.Paragraphs[rangeStartPos.ParaIndex].Spans[rangeStartPos.SpanIndex].Style.CloneTyped();
                EraseRange(cText.SelectionRange.Value);
                cText.CursorPosition = cText.SelectionRange.Value.FirstCharPos;
                cText.SelectionStart = null;
            }
            var span = text.GetSpan(cText.CursorPosition);
            if (span.Style.Equals(cText.InputTextStyle))
            {
                span.Text = span.Text.Substring(0, cText.CursorPosition.CharIndex) +
                            str +
                            span.Text.Substring(cText.CursorPosition.CharIndex);
                cText.CursorPosition = cText.CursorPosition.WithCharPlus(str.Length);
            }
            else
            {
                var newSpan = AmFactory.Create<RtSpan>();
                newSpan.Style = cText.InputTextStyle.CloneTyped();
                newSpan.Text = str;
                text.SplitSpan(cText.CursorPosition, out var insertSpanIndex);
                text.GetPara(cText.CursorPosition).Spans.Insert(insertSpanIndex, newSpan);
                cText.CursorPosition = cText.CursorPosition.WithSpan(insertSpanIndex).WithChar(newSpan.Length);
            }
        }

        private void EnterNewLine()
        {
            var text = cText.TextBox.Text;

            text.SplitSpan(cText.CursorPosition, out var nextSpanIndex);
            var paraIndex = cText.CursorPosition.ParaIndex;
            var para = text.Paragraphs[paraIndex];
            var defaultPrevStyle = para.Spans[0].Style;
            var defaultNextStyle = para.Spans[Math.Min(nextSpanIndex, para.Spans.Count - 1)].Style;
            var newPara = AmFactory.Create<RtParagraph>();
            newPara.Style = para.Style.CloneTyped();
            while (nextSpanIndex < para.Spans.Count)
            {
                var span = para.Spans[nextSpanIndex];
                para.Spans.RemoveAt(nextSpanIndex);
                newPara.Spans.Add(span);
            }
            if (para.Spans.Count == 0)
            {
                var newSpan = AmFactory.Create<RtSpan>();
                newSpan.Style = defaultPrevStyle.CloneTyped();
                newPara.Spans.Add(newSpan);
            }
            if (newPara.Spans.Count == 0)
            {
                var newSpan = AmFactory.Create<RtSpan>();
                newSpan.Style = defaultNextStyle.CloneTyped();
                newPara.Spans.Add(newSpan);
            }
            text.Paragraphs.Insert(paraIndex + 1, newPara);
            cText.CursorPosition = new RtPosition(paraIndex + 1, 0, 0);
        }

        public bool CanCopy() => cText.SelectionRange.HasValue;

        public void Copy()
        {
            var range = cText.SelectionRange.Value;
            var text = cText.TextBox.Text;

            var builder = new StringBuilder();
            var first = range.FirstCharPos;
            var last = range.LastCharPos;
            for (var p = first.ParaIndex; p <= last.ParaIndex; p++)
            {
                var para = text.Paragraphs[p];
                var isFirstPara = p == first.ParaIndex;
                var isLastPara = p == last.ParaIndex;
                for (var s = (isFirstPara ? first.SpanIndex : 0); s <= (isLastPara ? last.SpanIndex : para.Spans.Count - 1); s++)
                {
                    var span = para.Spans[s];
                    var isFirstSpan = isFirstPara && s == first.SpanIndex;
                    var isLastSpan = isLastPara && s == last.SpanIndex;
                    var firstChar = isFirstSpan ? first.CharIndex : 0;
                    var lastChar = isLastSpan ? last.CharIndex : span.Length - 1;
                    builder.Append(span.Text.SafeSubstring(firstChar, lastChar - firstChar));
                }
                if (!isLastPara)
                    builder.AppendLine();
            }
            clipboard.Text = builder.ToString();
        }

        public void Cut()
        {
            Copy();
            EraseRange(cText.SelectionRange.Value);
        }

        public bool CanPaste() => !string.IsNullOrEmpty(clipboard.Text);

        public void Paste()
        {
            var lines = clipboard.Text.Replace("\r", "").Split('\n');
            if (lines.Length == 0)
                return;
            InputStr(lines.First());
            foreach (var line in lines.Skip(1))
            {
                EnterNewLine();
                InputStr(line);
            }
        }

        private void EraseRange(RtRange range)
        {
            var start = range.FirstCharPos;
            var end = range.LastCharPos;
            while (end != start)
                EraseSingleChar(ref end);
        }

        private void EraseSingleChar(ref RtPosition cp)
        {
            var textBox = cText.TextBox;
            var text = textBox.Text;
            var para = text.Paragraphs[cp.ParaIndex];

            if (cp.CharIndex > 0)
            {
                cp.CharIndex--;
                var span = para.Spans[cp.SpanIndex];
                span.Text = span.Text.SafeSubstring(0, cp.CharIndex) + span.Text.SafeSubstring(cp.CharIndex + 1);
                para.Normalize();
            }
            else if (para.Spans.Take(cp.SpanIndex).Sum(x => x.Length) > 0)
            {
                cp.SpanIndex--;
                while (para.Spans[cp.SpanIndex].Length == 0)
                    cp.SpanIndex--;
                var span = para.Spans[cp.SpanIndex];
                cp.CharIndex = span.Text.Length - 1;
                span.Text = span.Text.Substring(0, span.Text.Length - 1);
                para.Normalize();
            }
            else if (cp.ParaIndex > 0)
            {
                cp.ParaIndex--;
                var prevPara = text.Paragraphs[cp.ParaIndex];
                cp.SpanIndex = prevPara.Spans.Count - 1;
                var lastSpan = prevPara.Spans[cp.SpanIndex];
                cp.CharIndex = lastSpan.Length;
                text.Paragraphs.RemoveAt(cp.ParaIndex + 1);
                while (para.Spans.Count > 0)
                {
                    var span = para.Spans[0];
                    para.Spans.RemoveAt(0);
                    prevPara.Spans.Add(span);
                }
                text.Paragraphs[cp.ParaIndex].Normalize();
            }
        }

        private void MoveCursorUpdatingInputStyle(RtPosition newValue)
        {
            cText.CursorPosition = newValue;
            cText.InputTextStyle = cText.TextBox.Text.Paragraphs[newValue.ParaIndex].Spans[newValue.SpanIndex].Style.CloneTyped();
        }
    }
}