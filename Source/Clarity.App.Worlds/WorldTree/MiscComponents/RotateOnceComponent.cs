using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.App.Worlds.WorldTree.MiscComponents
{
    public abstract class RotateOnceComponent : SceneNodeComponentBase<RotateOnceComponent>, IInteractionComponent
    {
        private const float Duration = 1f;

        private bool active;
        private Transform originalTransform;
        private float startTime;
        private float lastTime;
        
        public override void Update(FrameTime frameTime)
        {
            lastTime = frameTime.TotalSeconds;
            if (!active)
                return;
            var rawAmount = (lastTime - startTime) / Duration;
            if (rawAmount >= 1)
            {
                active = false;
                Node.Transform = originalTransform;
                return;
            }
            var amount = MathHelper.Hermite(rawAmount);
            var roatation = Quaternion.RotationY(MathHelper.TwoPi * amount);
            Node.Transform = new Transform(originalTransform.Scale, originalTransform.Rotation * roatation, originalTransform.Offset);
        }

        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            if (active)
                return true;

            if (!(args is IMouseEvent margs))
                return false;

            if (!margs.IsLeftDoubleClickEvent() || margs.KeyModifiers != KeyModifiers.None)
                return false;

            originalTransform = Node.Transform;
            startTime = lastTime;
            active = true;
            return true;
        }
    }
}