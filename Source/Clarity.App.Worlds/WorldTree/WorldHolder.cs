using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.WorldTree
{
    public abstract class WorldHolder : AmObjectBase<WorldHolder>
    {
        public abstract IWorld World { get; set; }

        public event Action<IAmEventMessage> Updated;

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            Updated?.Invoke(message);
        }
    }
}