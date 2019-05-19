using System.Collections.Generic;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public abstract class ModelLayerLimitComponent : SceneNodeComponentBase<ModelLayerLimitComponent>
    {
        public abstract int InitialLimit { get; set; }
        public int LayerLimit { get; set; }
        public List<int> Exclusions { get; }

        public ModelLayerLimitComponent()
        {
            Exclusions = new List<int>();
        }

        public override void AmOnAttached()
        {
            LayerLimit = InitialLimit;
        }
    }
}