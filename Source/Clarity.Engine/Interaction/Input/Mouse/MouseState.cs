using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.OtherTuples;

namespace Clarity.Engine.Interaction.Input.Mouse
{
    public class MouseState : IMouseState, ICloneable
    {
        public MouseButtons Buttons { get; set; }
        public IntVector2 Position { get; set; }
        public Vector2 NormalizedPosition { get; set; }
        public Vector2 HmgnPosition { get; set; }
        public int WheelPosition { get; set; }

        public MouseState() {  }
        
        public MouseState(MouseButtons buttons, IntVector2 position, Vector2 normalizedPosition, Vector2 hmgnPosition, int wheelPosition)
        {
            Buttons = buttons;
            Position = position;
            NormalizedPosition = normalizedPosition;
            HmgnPosition = hmgnPosition;
            WheelPosition = wheelPosition;
        }

        public object Clone()
        {
            return new MouseState
            {
                Buttons = Buttons,
                Position = Position,
                NormalizedPosition = NormalizedPosition,
                HmgnPosition = HmgnPosition,
                WheelPosition = WheelPosition
            };
        }
    }
}