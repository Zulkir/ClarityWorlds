using System;
using Clarity.Common.Numericals;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Special
{
    public abstract class WarpScrollComponent : SceneNodeComponentBase<WarpScrollComponent>
    {
        public float RealScrollAmount { get; set; }
        public float VisibleScrollAmount { get; set; }
        
        public override void Update(FrameTime frameTime)
        {
            var amount = Math.Min(20f * frameTime.DeltaSeconds, 1f);
            VisibleScrollAmount = MathHelper.Lerp(VisibleScrollAmount, RealScrollAmount, amount);
        }
    }
}