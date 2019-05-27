using System;
using System.Linq;
using Assets.Scripts.Helpers;
using Clarity.App.Worlds.Gui;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using UnityEngine;


namespace Assets.Scripts.Gui
{
    public class UcKeyboardInputProvider : IKeyboardInputProvider, IUcInputProvider
    {
        private readonly IInputService inputService;
        private readonly Lazy<IGui> guiLazy;
        private readonly bool[] keys = new bool[100];
        

        public UcKeyboardInputProvider(IInputService inputService, Lazy<IGui> guiLazy)
        {
            this.inputService = inputService;
            this.guiLazy = guiLazy;
        }

        public bool IsKeyPressed(Key key)
        {
            return keys[(int)key];
        }

        public void OnGui()
        {
            var unityEvent = Event.current;
            if (!unityEvent.isKey)
                return;
            var complexEventType = ToClarityEventType(unityEvent);
            if (!complexEventType.HasValue)
                return;
            var key = unityEvent.keyCode.ToClarity();
            UpdateKeyState(complexEventType.Value, key);
            NotifyInputService(complexEventType.Value, key);
        }

        private void UpdateKeyState(KeyEventType complexEventType, Key key)
        {
            switch (complexEventType)
            {
                case KeyEventType.Down:
                    keys[(int)key] = true;
                    break;
                case KeyEventType.Up:
                    keys[(int)key] = false;
                    break;
            }
        }

        private void NotifyInputService(KeyEventType complexEventType, Key key)
        { 
            inputService.OnInputEvent(new KeyEventArgs
            {
                ComplexEventType = complexEventType,
                KeyModifyers = GetKeyModifyersFromInput(),
                EventKey = key,
                HasFocus = true,
                State = new KeyboardState(keys.Copy()),
                Viewport = guiLazy.Value.RenderControl.Viewports.Single()
            });
        }

        private static KeyEventType? ToClarityEventType(Event unityEvent)
        {
            switch (unityEvent.type)
            {
                case EventType.KeyDown: return KeyEventType.Down;
                case EventType.KeyUp: return KeyEventType.Up;
                default: return null;
            }
        }

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
        
        public void OnUpdate()
        {
            
        }
    }
}
