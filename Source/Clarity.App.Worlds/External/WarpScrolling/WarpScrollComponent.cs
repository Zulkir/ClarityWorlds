using System;
using Clarity.Common.Numericals;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Special;

namespace Clarity.App.Worlds.External.WarpScrolling
{
    public abstract class WarpScrollComponent : SceneNodeComponentBase<WarpScrollComponent>, IWarpScrollComponent, IInteractionComponent
    {
        public float RealScrollAmount { get; set; }
        public float VisibleScrollAmount { get; set; }
        
        public override void Update(FrameTime frameTime)
        {
            var amount = Math.Min(20f * frameTime.DeltaSeconds, 1f);
            VisibleScrollAmount = MathHelper.Lerp(VisibleScrollAmount, RealScrollAmount, amount);
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (!(args is IMouseEventArgs margs))
                return false;
            if (margs.ComplexEventType != MouseEventType.Wheel || margs.KeyModifyers != KeyModifyers.Control)
                return false;
            RealScrollAmount += margs.WheelDelta * 0.01f;
            if (RealScrollAmount < 0)
                RealScrollAmount = 0;
            if (RealScrollAmount > 1)
                RealScrollAmount = 1;
            return true;
        }
    }
}