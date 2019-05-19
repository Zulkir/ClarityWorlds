using System;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Eto.Forms;
using CKeyEventArgs = Clarity.Engine.Interaction.Input.Keyboard.KeyEventArgs;
using EKeyEventArgs = Eto.Forms.KeyEventArgs;
using KeyEventType = Clarity.Engine.Interaction.Input.Keyboard.KeyEventType;

namespace Clarity.Ext.Gui.EtoForms
{
    public class KeyboardInputProvider : IKeyboardInputProvider
    {
        private readonly IInputService inputService;
        private readonly bool[] keys = new bool[100];
        private readonly RenderControl renderControl;

        public KeyboardInputProvider(IMainForm mainForm, IInputService inputService)
        {
            this.inputService = inputService;
            renderControl = mainForm.RenderControl;
            renderControl.KeyDown += OnKeyDown;
            renderControl.KeyUp += OnKeyUp;
            renderControl.TextInput += OnTextInput;
            renderControl.GotFocus += OnFocusChanged;
            renderControl.LostFocus += OnFocusChanged;
        }

        private void OnKeyDown(object sender, EKeyEventArgs keyEventArgs)
        {
            var key = keyEventArgs.Key.ToClarity();
            keys[(int)key] = true;
            var cEventArgs = new CKeyEventArgs
            {
                ComplexEventType = KeyEventType.Down,
                KeyModifyers = GetModifyers(keyEventArgs),
                EventKey = keyEventArgs.Key.ToClarity(),
                HasFocus = renderControl.HasFocus,
                State = new KeyboardState(keys.Copy()),
                Viewport = inputService.FocusedViewport
            };
            inputService.OnInputEvent(cEventArgs);
            if (keyEventArgs.Key == Keys.Tab)
                keyEventArgs.Handled = true;
        }

        private void OnKeyUp(object sender, EKeyEventArgs keyEventArgs)
        {
            var key = keyEventArgs.Key.ToClarity();
            keys[(int)key] = false;
            var cEventArgs = new CKeyEventArgs
            {
                ComplexEventType = KeyEventType.Up,
                KeyModifyers = GetModifyers(keyEventArgs),
                EventKey = keyEventArgs.Key.ToClarity(),
                HasFocus = renderControl.HasFocus,
                State = new KeyboardState(keys.Copy()),
                Viewport = inputService.FocusedViewport
            };
            inputService.OnInputEvent(cEventArgs);
        }

        private void OnTextInput(object sender, TextInputEventArgs textInputEventArgs)
        {
            var cEventArgs = new CKeyEventArgs
            {
                ComplexEventType = KeyEventType.TextInput,
                HasFocus = renderControl.HasFocus,
                State = new KeyboardState(keys.Copy()),
                Text = textInputEventArgs.Text,
                Viewport = inputService.FocusedViewport
            };
            inputService.OnInputEvent(cEventArgs);
        }

        private void OnFocusChanged(object sender, EventArgs eventArgs)
        {
            var cEventArgs = new CKeyEventArgs
            {
                ComplexEventType = KeyEventType.FocusChanged,
                HasFocus = renderControl.HasFocus,
                State = new KeyboardState(keys.Copy()),
                Viewport = inputService.FocusedViewport
            };
            inputService.OnInputEvent(cEventArgs);
        }
        
        private static KeyModifyers GetModifyers(EKeyEventArgs keyEventArgs)
        {
            var result = KeyModifyers.None;
            if (keyEventArgs.Shift)
                result |= KeyModifyers.Shift;
            if (keyEventArgs.Control)
                result |= KeyModifyers.Control;
            if (keyEventArgs.Alt)
                result |= KeyModifyers.Alt;
            return result;
        }

        public bool IsKeyPressed(Key key) => keys[(int)key];
    }
}