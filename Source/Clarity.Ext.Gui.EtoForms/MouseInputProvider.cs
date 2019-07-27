using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Eto.Drawing;
using Eto.Forms;
using EMouseEventArgs = Eto.Forms.MouseEventArgs;
using EMouseButtons = Eto.Forms.MouseButtons;
using CMouseButtons = Clarity.Engine.Interaction.Input.Mouse.MouseButtons;

namespace Clarity.Ext.Gui.EtoForms
{
    public class MouseInputProvider : IMouseInputProvider
    {
        private readonly IMainForm mainForm;
        private readonly IInputService inputService;
        private IntVector2 prevGlobalPosition;
        private int wheelPosition;
        private DateTime lastDownTime;
        private DateTime lastClickTime;
        private bool canBeClick;
        private bool canBeDoubleClick;

        public MouseInputProvider(IMainForm mainForm, IInputService inputService)
        {
            this.mainForm = mainForm;
            this.inputService = inputService;
            lastClickTime = new DateTime();
            mainForm.RenderControl.MouseMove += OnMouseMove;
            mainForm.RenderControl.MouseDown += OnMouseDown;
            mainForm.RenderControl.MouseUp += OnMouseUp;
            mainForm.Form.MouseWheel += OnMouseWheel;
        }

        private Vector2 GetCurrentRawGlobalPos() => 
            ToClarity(mainForm.RenderControl.PointFromScreen(Mouse.Position));

        private void OnMouseEvent(MouseEventType eventType, EMouseEventArgs eArgs)
        {
            var currentTime = DateTime.Now;

            if (eventType == MouseEventType.Down)
            {
                canBeClick = true;
                lastDownTime = currentTime;
            }
            else if (eventType == MouseEventType.Up)
            {
                if (canBeDoubleClick && (currentTime - lastClickTime).TotalSeconds < 0.5f)
                {
                    eventType = MouseEventType.DoubleClick;
                    canBeDoubleClick = false;
                }
                else if (canBeClick && (currentTime - lastDownTime).TotalSeconds < 0.5f)
                {
                    eventType = MouseEventType.Click;
                    canBeDoubleClick = true;
                    lastClickTime = currentTime;
                }
            }

            var wheelDelta = Round(eArgs.Delta.Height);
            wheelPosition += wheelDelta;
            var rawGlobalPos = GetCurrentRawGlobalPos();
            var currGlobalPos = Round(rawGlobalPos);
            var delta = currGlobalPos - prevGlobalPosition;

            if (eventType == MouseEventType.Move)
            {
                if (delta == IntVector2.Zero)
                    return;

                canBeClick = false;
                canBeDoubleClick = false;
            }
            
            var viewport = mainForm.RenderControl.GetViewport(rawGlobalPos);
            var relMousePosition = rawGlobalPos - new Vector2(viewport.Left, viewport.Top);
            var currState = new MouseState
            {
                Buttons = ToClarity(Mouse.Buttons),
                Position = Round(relMousePosition),
                NormalizedPosition = Normalize(relMousePosition),
                HmgnPosition = Hmgnize(relMousePosition),
                WheelPosition = wheelPosition
            };

            var cArgs = new MouseEvent
            {
                ComplexEventType = eventType,
                EventButtons = ToClarity(eArgs.Buttons),
                State = currState,
                Delta = delta,
                WheelDelta = wheelDelta,
                KeyModifiers = ToClarity(eArgs.Modifiers),
                Viewport = viewport
            };

            inputService.OnInputEvent(cArgs);

            prevGlobalPosition = currGlobalPos;
        }

        private static CMouseButtons ToClarity(EMouseButtons eButtons)
        {
            var result = CMouseButtons.None;
            if (eButtons.HasFlagFast(EMouseButtons.Primary))
                result |= CMouseButtons.Left;
            if (eButtons.HasFlagFast(EMouseButtons.Alternate))
                result |= CMouseButtons.Right;
            if (eButtons.HasFlagFast(EMouseButtons.Middle))
                result |= CMouseButtons.Middle;
            return result;
        }

        private static KeyModifiers ToClarity(Keys modifiers)
        {
            var result = KeyModifiers.None;
            if ((modifiers & Keys.Control) != 0)
                result |= KeyModifiers.Control;
            if ((modifiers & Keys.Shift) != 0)
                result |= KeyModifiers.Shift;
            if ((modifiers & Keys.Alt) != 0)
                result |= KeyModifiers.Alt;
            return result;
        }

        private static Vector2 ToClarity(PointF ePoint) { return new Vector2(ePoint.X, ePoint.Y); }
        private static Vector2 ToClarity(SizeF eSize) { return new Vector2(eSize.Width, eSize.Height); }
        private static IntVector2 Round(Vector2 v) { return new IntVector2(Round(v.X), Round(v.Y)); }
        private static int Round(float x) { return (int)Math.Round(x); }

        private Vector2 Normalize(Vector2 v)
        {
            return new Vector2(
                2f * (v.X - mainForm.RenderControl.Width / 2f) / mainForm.RenderControl.Height,
                1f - v.Y * 2f / mainForm.RenderControl.Height);
        }

        private Vector2 Hmgnize(Vector2 v)
        {
            return new Vector2(
                v.X / mainForm.RenderControl.Width * 2f - 1f,
                1f - v.Y * 2f / mainForm.RenderControl.Height);
        }

        private void OnMouseMove(object sender, EMouseEventArgs mouseEventArgs) { OnMouseEvent(MouseEventType.Move, mouseEventArgs); }
        private void OnMouseDown(object sender, EMouseEventArgs mouseEventArgs) { OnMouseEvent(MouseEventType.Down, mouseEventArgs); }
        private void OnMouseUp(object sender, EMouseEventArgs mouseEventArgs) { OnMouseEvent(MouseEventType.Up, mouseEventArgs); }
        private void OnMouseWheel(object sender, EMouseEventArgs mouseEventArgs) { OnMouseEvent(MouseEventType.Wheel, mouseEventArgs); }
    }
}