using System;
using System.Linq;
using Clarity.App.Worlds.Gui;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using UnityEngine;
using Vector2 = Clarity.Common.Numericals.Algebra.Vector2;

namespace Assets.Scripts.Gui
{
    public class UcMouseInputProvider : IMouseInputProvider, IUcInputProvider
    {
        private readonly IInputService inputService;
        private readonly Lazy<IGui> guiLazy;
        private MouseState prevState;
        private int wheelPos = 0;

        private const float MaxClickDelay = 0.5f;
        private bool canBeClick;
        private bool canBeDoubleClick;
        private float lastDownTime;
        private float lastClickTime;


        public UcMouseInputProvider(IInputService inputService, Lazy<IGui> guiLazy)
        {
            this.inputService = inputService;
            this.guiLazy = guiLazy;
            prevState = GetCurrentStateCopy();
        }

        public MouseState GetCurrentStateCopy()
        {
            var buttons = MouseButtons.None;
            if (Input.GetMouseButton(0))
                buttons |= MouseButtons.Left;
            if (Input.GetMouseButton(1))
                buttons |= MouseButtons.Right;
            if (Input.GetMouseButton(2))
                buttons |= MouseButtons.Middle;
            var relMousePosition = RelMousePositionToClarity(Input.mousePosition);
            wheelPos += Round(Input.mouseScrollDelta.y);

            return new MouseState
            {
                Buttons = buttons,
                Position = Round(relMousePosition),
                NormalizedPosition = Normalize(relMousePosition),
                HmgnPosition = Hmgnize(relMousePosition),
                WheelPosition = wheelPos
            };
        }

        private static Vector2 RelMousePositionToClarity(Vector3 unityPos) => new Vector2(unityPos.x, Screen.height - unityPos.y);
        private static IntVector2 Round(Vector2 v) => new IntVector2(Round(v.X), Round(v.Y));
        private static int Round(float x) => (int)Math.Round(x);

        private static Vector2 Normalize(Vector2 v)
        {
            return new Vector2(
                2f * (v.X - Screen.width / 2f) / Screen.height,
                1f - v.Y * 2f / Screen.height);
        }

        private static Vector2 Hmgnize(Vector2 v)
        {
            return new Vector2(
                v.X / Screen.width * 2f - 1f,
                1f - v.Y * 2f / Screen.height);
        }

        public void OnUpdate()
        {
            var currState = GetCurrentStateCopy();
            ProcessMove(currState);
            ProcessWheel(currState);
            ProcessButtonDown(0, MouseButtons.Left);
            ProcessButtonDown(1, MouseButtons.Right);
            ProcessButtonDown(2, MouseButtons.Middle);
            ProcessButtonUp(0, MouseButtons.Left);
            ProcessButtonUp(1, MouseButtons.Right);
            ProcessButtonUp(2, MouseButtons.Middle);
            prevState = currState;
        }

        private void ProcessMove(IMouseState currState)
        {
            if (currState.Position == prevState.Position)
                return;

            canBeClick = false;
            canBeDoubleClick = false;

            var delta = currState.Position - prevState.Position;
            var state = prevState.CloneTyped();
            state.Position = currState.Position;
            state.NormalizedPosition = currState.NormalizedPosition;
            state.HmgnPosition = currState.HmgnPosition;
            FireMove(state, delta);
            prevState = state;
        }

        private void ProcessWheel(IMouseState currState)
        {
            if (currState.WheelPosition == prevState.WheelPosition)
                return;
            var wheelDelta = currState.WheelPosition - prevState.WheelPosition;
            var state = prevState.CloneTyped();
            state.WheelPosition  = currState.WheelPosition;
            FireWheel(state, wheelDelta);
            prevState = state;
        }

        private void ProcessButtonDown(int unityIndex, MouseButtons clarityButton)
        {
            if (!Input.GetMouseButtonDown(unityIndex))
                return;

            canBeClick = true;
            lastDownTime = Time.time;

            var state = prevState.CloneTyped();
            state.Buttons |= clarityButton;
            FireButton(state, clarityButton, MouseEventType.Down);
            prevState = state;
        }

        private void ProcessButtonUp(int unityIndex, MouseButtons clarityButton)
        {
            if (!Input.GetMouseButtonUp(unityIndex))
                return;

            var eventType = MouseEventType.Up;
            if (canBeDoubleClick && Time.time - lastClickTime < MaxClickDelay)
            {
                eventType = MouseEventType.DoubleClick;
                canBeDoubleClick = false;
            }
            else if (canBeClick && Time.time - lastDownTime < MaxClickDelay)
            {
                eventType = MouseEventType.Click;
                canBeDoubleClick = true;
                lastClickTime = Time.time;
            }

            var state = prevState.CloneTyped();
            state.Buttons &= ~clarityButton;
            FireButton(state, clarityButton, eventType);
            prevState = state;
        }

        private void FireMove(IMouseState state, IntVector2 delta)
        {
            inputService.OnInputEvent(new MouseEventArgs
            {
                ComplexEventType = MouseEventType.Move,
                EventButtons = MouseButtons.None,
                State = state,
                Delta = delta,
                WheelDelta = 0,
                KeyModifyers = GetKeyModifyersFromInput(),
                Viewport = guiLazy.Value.RenderControl.Viewports.Single()
            });
        }

        private void FireWheel(IMouseState state, int wheelDelta)
        {
            inputService.OnInputEvent(new MouseEventArgs
            {
                ComplexEventType = MouseEventType.Wheel,
                EventButtons = MouseButtons.None,
                State = state,
                Delta = IntVector2.Zero,
                WheelDelta = wheelDelta,
                KeyModifyers = GetKeyModifyersFromInput(),
                Viewport = guiLazy.Value.RenderControl.Viewports.Single()
            });
        }

        private void FireButton(IMouseState state, MouseButtons button, MouseEventType eventType)
        {
            inputService.OnInputEvent(new MouseEventArgs
            {
                ComplexEventType = eventType,
                EventButtons = button,
                State = state,
                Delta = IntVector2.Zero,
                WheelDelta = 0,
                KeyModifyers = GetKeyModifyersFromInput(),
                Viewport = guiLazy.Value.RenderControl.Viewports.Single()
            });
        }

        public void OnGui()
        {
      
        }

        #region Obsolete OnGUI implementation
        /*
        public void OnGui()
        {
            var unityEvent = Event.current;
            if (!unityEvent.isMouse)
                return;
            var complexEventType = ToCLarityEventType(unityEvent);
            if (!complexEventType.HasValue)
                return;
            var currState = GetCurrentStateCopy();
            var args = new MouseEventArgs
            {
                ComplexEventType = complexEventType.Value,
                EventButtons = ToClarityMouseButtons(unityEvent),
                State = currState,
                Delta = currState.Position - prevState.Position,
                NormalizedDelta = currState.NormalizedPosition - prevState.NormalizedPosition,
                WheelDelta = currState.WheelPosition - prevState.WheelPosition,
                KeyModifyers = ToClarityKeyModifyers(unityEvent),
                Viewport = viewService.RenderControl.FocusedViewport
            };
            prevState = currState;
            inputService.OnInputEvent(args);
        }
        
        private static MouseEventType? ToCLarityEventType(Event unityEvent)
        {
            switch (unityEvent.type)
            {
                case EventType.MouseDown:
                    return unityEvent.clickCount == 2 ? MouseEventType.DoubleClick : MouseEventType.Down;
                case EventType.MouseUp: return MouseEventType.Up;
                case EventType.MouseDrag: return MouseEventType.Move;
                case EventType.ScrollWheel:
                    return MouseEventType.Wheel;
                case EventType.MouseMove:
                case EventType.KeyDown:
                case EventType.KeyUp:
                case EventType.Repaint:
                case EventType.Layout:
                case EventType.DragUpdated:
                case EventType.DragPerform:
                case EventType.DragExited:
                case EventType.Ignore:
                case EventType.Used:
                case EventType.ValidateCommand:
                case EventType.ExecuteCommand:
                case EventType.ContextClick:
                case EventType.MouseEnterWindow:
                case EventType.MouseLeaveWindow:
                default:
                    return null;
            }
        }

        private static MouseButtons ToClarityMouseButtons(Event unityEvent)
        {
            if (unityEvent.type != EventType.MouseDown && unityEvent.type != EventType.MouseUp)
                return MouseButtons.None;
            switch (unityEvent.button)
            {
                case 0: return MouseButtons.Left;
                case 1: return MouseButtons.Right;
                case 2: return MouseButtons.Middle;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private static KeyModifyers ToClarityKeyModifyers(Event unityEvent)
        {
            var modifyers = KeyModifyers.None;
            if (unityEvent.control)
                modifyers |= KeyModifyers.Control;
            if (unityEvent.shift)
                modifyers |= KeyModifyers.Shift;
            if (unityEvent.alt)
                modifyers |= KeyModifyers.Alt;
            return modifyers;
        }*/
        #endregion

        private static KeyModifyers GetKeyModifyersFromInput()
        {
            var modifyers = KeyModifyers.None;
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl))
                modifyers |= KeyModifyers.Control;
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift))
                modifyers |= KeyModifyers.Shift;
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.RightAlt))
                modifyers |= KeyModifyers.Alt;
            return modifyers;
        }
    }
}