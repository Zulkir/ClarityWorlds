using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Gui;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Text.Rich;

namespace Clarity.App.Worlds.Media.Media2D
{
    public class RichTextEditInteractionElement : IInteractionElement
    {
        private readonly IRichTextComponent cText;
        private readonly IInputHandler inputHandler;
        private readonly IUndoRedoService undoRedo;
        private readonly IClipboard clipboard;

        private IRichTextHeadlessEditor headlessEditor;
        
        public IRichTextHeadlessEditor HeadlessEditor => headlessEditor?.Text == cText.TextBox.Text 
            ? headlessEditor 
            : headlessEditor = new RichTextHeadlessEditor(cText.TextBox.Text);

        public RichTextEditInteractionElement(IRichTextComponent cText, 
            IInputHandler inputHandler, IUndoRedoService undoRedo, IClipboard clipboard)
        {
            this.cText = cText;
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

            var editor = HeadlessEditor;
            var textBox = cText.TextBox;
            var layout = textBox.Layout;

            if (args.ComplexEventType == KeyEventType.TextInput)
            {
                editor.InputString(args.Text);
                undoRedo.OnChange();
                return true;
            }

            if (args.ComplexEventType == KeyEventType.Down)
            {
                switch (args.EventKey)
                {
                    case Key.Left:
                        {
                            editor.MoveCursorSafe(editor.CursorPos - 1, args.KeyModifiers.HasFlag(KeyModifiers.Shift));
                            return true;
                        }
                    case Key.Right:
                        {
                            editor.MoveCursorSafe(editor.CursorPos + 1, args.KeyModifiers.HasFlag(KeyModifiers.Shift));
                            return true;
                        }
                    case Key.Up:
                        {
                            if (!layout.TryGetUp(editor.CursorPos, out var newPos))
                                newPos = editor.CursorPos;
                            editor.MoveCursor(newPos, args.KeyModifiers.HasFlag(KeyModifiers.Shift));
                            return true;
                        }
                    case Key.Down:
                        {
                            if (!layout.TryGetDown(editor.CursorPos, out var newPos))
                                newPos = editor.CursorPos;
                            editor.MoveCursor(newPos, args.KeyModifiers.HasFlag(KeyModifiers.Shift));
                            return true;
                        }
                    case Key.Enter:
                        {
                            editor.InputString("\n");
                            undoRedo.OnChange();
                            return true;
                        }
                    case Key.Backspace:
                        {
                            editor.Erase();
                            undoRedo.OnChange();
                            return true;
                        }
                    case Key.Tab:
                        if (args.KeyModifiers == KeyModifiers.None)
                            editor.Tab();
                        else if (args.KeyModifiers == KeyModifiers.Shift)
                            editor.ShiftTab();
                        return true;
                }
            }
            return false;
        }

        private bool TryHandleMouseEvent(IMouseEvent args)
        {
            var rect = cText.Node.GetComponent<IRectangleComponent>().Rectangle;
            var textBox = cText.TextBox;
            var hmgnPoint = args.RayHitResult.LocalHitPoint.Xy;
            var textBoxPoint = new Vector2(
                (1 + hmgnPoint.X) / 2 * rect.Width * textBox.PixelScaling,
                (1 - hmgnPoint.Y) / 2 * rect.Height * textBox.PixelScaling);

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
                HeadlessEditor.MoveCursor(newPos, args.KeyModifiers == KeyModifiers.Shift);
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
                var rect = cText.Node.GetComponent<IRectangleComponent>().Rectangle;
                var textBox = cText.TextBox;
                var hmgnPoint = args.RayHitResult.LocalHitPoint.Xy;
                var textBoxPoint = new Vector2(
                    (1 + hmgnPoint.X) / 2 * rect.Width * textBox.PixelScaling,
                    (1 - hmgnPoint.Y) / 2 * rect.Height * textBox.PixelScaling);
                var layout = textBox.Layout;

                var newPos = layout.GetPosition(textBoxPoint);
                HeadlessEditor.MoveCursor(newPos, true);
                return InputEventProcessResult.StopPropagating;
            }
            return InputEventProcessResult.ReleaseLock;
        }

        public bool CanCopy()
        {
            return HeadlessEditor.CanCopy();
        }

        public void Copy()
        {
            var copiedText = HeadlessEditor.Copy();
            if (!string.IsNullOrEmpty(copiedText))
                clipboard.Text = copiedText;
        }

        public void Cut()
        {
            var copiedText = HeadlessEditor.Cut();
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
            HeadlessEditor.Paste(clipboard.Text);
            undoRedo.OnChange();
        }
    }
}