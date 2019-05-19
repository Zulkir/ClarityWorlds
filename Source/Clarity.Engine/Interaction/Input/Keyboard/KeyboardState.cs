namespace Clarity.Engine.Interaction.Input.Keyboard
{
    public class KeyboardState : IKeyboardState
    {
        private readonly bool[] keyStates;

        public KeyboardState(bool[] keyStates)
        {
            this.keyStates = keyStates;
        }

        public bool IsKeyPressed(Key key) => 
            keyStates[(int)key];
    }
}