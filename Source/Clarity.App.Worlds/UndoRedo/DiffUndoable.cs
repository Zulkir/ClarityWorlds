using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.UndoRedo
{
    public class DiffUndoable : IUndoable
    {
        private readonly ITrwSerializationDiffApplier applier;
        private readonly IWorld world;
        private readonly ITrwDiff diff;

        public DiffUndoable(ITrwSerializationDiffApplier applier, IWorld world, ITrwDiff diff)
        {
            this.applier = applier;
            this.world = world;
            this.diff = diff;
        }

        public void Apply()
        {
            applier.ApplyDiff(world, diff, TrwDiffDirection.Forward);
        }

        public void Undo()
        {
            applier.ApplyDiff(world, diff, TrwDiffDirection.Backward);
        }
    }
}