using System;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Visualization.Viewports;
using JetBrains.Annotations;

namespace Clarity.Engine.Interaction.Input
{
    public interface IInputService
    {
        event Action<IInputEventArgs> Input;

        // todo: to IKeyboardDevice Keyboard { get; }
        [CanBeNull]
        IViewport FocusedViewport { get; }
        IKeyboardState CurrentKeyboardState { get; }
        KeyModifyers CurrentKeyModifiers { get; }
        
        void OnInputEvent(IInputEventArgs args);
        void OnFocusedViewportChanged(IViewport viewport);
    }
}