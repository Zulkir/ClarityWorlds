using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Media.Media2D
{
    public class RichTextEditInteractionElement : IInteractionElement
    {
        private readonly IRichTextComponent cText;
        private readonly IRichTextHeadlessEditor headlessEditor;
        private readonly IInputHandler inputHandler;
        private readonly IUndoRedoService undoRedo;
        private readonly IClipboard clipboard;

        public RichTextEditInteractionElement(IRichTextComponent cText, IRichTextHeadlessEditor headlessEditor, 
            IInputHandler inputHandler, IUndoRedoService undoRedo, IClipboard clipboard)
        {
            this.cText = cText;
            this.headlessEditor = headlessEditor;
            this.undoRedo = undoRedo;
            this.clipboard = clipboard;
            this.inputHandler = inputHandler;
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            switch (args) 
            {
                case IMouseEvent mouseArgs: return TryHandleMouseEvent(mouseArgs);
                case IKeyEvent keyboardArgs: return TryHandleKeyEvent(keyboardArgs);
                default: return false;
            }
        }

        private bool TryHandleKeyEvent(IKeyEvent args)
        {
            if (!args.HasFocus)
                return false;

            var textBox = cText.TextBox;
            var layout = textBox.Layout;

            if (args.ComplexEventType == KeyEventType.TextInput)
            {
                headlessEditor.InputString(args.Text);
                undoRedo.OnChange();
                return true;
            }

            if (args.ComplexEventType == KeyEventType.Down)
            {
                switch (args.EventKey)
                {
                    case Key.Left:
                        {
                            headlessEditor.MoveCursorSafe(headlessEditor.CursorPos - 1, args.KeyModifiers.HasFlag(KeyModifiers.Shift));
                            return true;
                        }
                    case Key.Right:
                        {
                            headlessEditor.MoveCursorSafe(headlessEditor.CursorPos + 1, args.KeyModifiers.HasFlag(KeyModifiers.Shift));
                            return true;
                        }
                    case Key.Up:
                        {
                            if (!layout.TryGetUp(headlessEditor.CursorPos, out var newPos))
                                newPos = headlessEditor.CursorPos;
                            headlessEditor.MoveCursor(newPos, args.KeyModifiers.HasFlag(KeyModifiers.Shift));
                            return true;
                        }
                    case Key.Down:
                        {
                            if (!layout.TryGetDown(headlessEditor.CursorPos, out var newPos))
                                newPos = headlessEditor.CursorPos;
                            headlessEditor.MoveCursor(newPos, args.KeyModifiers.HasFlag(KeyModifiers.Shift));
                            return true;
                        }
                    case Key.Enter:
                        {
                            headlessEditor.InputString("\n");
                            undoRedo.OnChange();
                            return true;
                        }
                    case Key.Backspace:
                        {
                            headlessEditor.Erase();
                            undoRedo.OnChange();
                            return true;
                        }
                    case Key.Tab:
                        if (args.KeyModifiers == KeyModifiers.None)
                            headlessEditor.Tab();
                        else if (args.KeyModifiers == KeyModifiers.Shift)
                            headlessEditor.ShiftTab();
                        return true;
                }
            }
            return false;
        }

        private bool TryHandleMouseEvent(IMouseEvent args)
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

            if (layout.TryGetSpanAt(textBoxPoint, out var lspan) && 
                lspan.Embedding != null &&
                lspan.EmbeddingHandler != null &&
                lspan.EmbeddingHandler.TryHandleMouseEvent(lspan.Embedding, args))
            {
                return true;
            }

            if (args.IsLeftDownEvent())
            {
                var newPos = layout.GetPosition(textBoxPoint);
                headlessEditor.MoveCursor(newPos, args.KeyModifiers == KeyModifiers.Shift);
                inputHandler.AddLock(new InputLock<object>(null, MouseDownLockProc));
                return true;
            }

            return false;
        }

        private InputEventProcessResult MouseDownLockProc(object o, IInputEvent inputEvent)
        {
            if (!(inputEvent is IMouseEvent args))
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

                var newPos = layout.GetPosition(textBoxPoint);
                headlessEditor.MoveCursor(newPos, true);
                return InputEventProcessResult.StopPropagating;
            }
            return InputEventProcessResult.ReleaseLock;
        }

        public bool CanCopy()
        {
            return headlessEditor.CanCopy();
        }

        public void Copy()
        {
            var copiedText = headlessEditor.Copy();
            if (!string.IsNullOrEmpty(copiedText))
                clipboard.Text = copiedText;
        }

        public void Cut()
        {
            var copiedText = headlessEditor.Cut();
            if (!string.IsNullOrEmpty(copiedText))
                clipboard.Text = copiedText;
            undoRedo.OnChange();
        }

        public bool CanPaste()
        {
            return !string.IsNullOrEmpty(clipboard.Text);
        }

        public void Paste()
        {
            headlessEditor.Paste(clipboard.Text);
            undoRedo.OnChange();
        }
    }
}