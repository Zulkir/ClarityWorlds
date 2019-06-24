using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Engine.Interaction.Input
{
    public class InputService : IInputService
    {
        private readonly IEventRoutingService eventRoutingService;

        // todo: refactor to actual querying or clear on defocus
        public IViewport FocusedViewport { get; private set; }
        public IKeyboardState CurrentKeyboardState { get; private set; }
        public KeyModifyers CurrentKeyModifiers { get; private set; }

        public InputService(IEventRoutingService eventRoutingService)
        {
            this.eventRoutingService = eventRoutingService;
            CurrentKeyboardState = new KeyboardState(new bool[100]);
        }

        public void OnInputEvent(IInputEventArgs args)
        {
            if (args is IKeyEventArgs keyArgs)
            {
                CurrentKeyboardState = keyArgs.State;
                CurrentKeyModifiers = keyArgs.KeyModifyers;
            }
            else if (args is IMouseEventArgs margs && margs.IsOfType(MouseEventType.Down))
            {
                FocusedViewport = args.Viewport;
            }
            eventRoutingService.FireEvent<IInteractionEventArgs>(args);
        }

        public void OnFocusedViewportChanged(IViewport viewport)
        {
            FocusedViewport = viewport;
        }
    }
}