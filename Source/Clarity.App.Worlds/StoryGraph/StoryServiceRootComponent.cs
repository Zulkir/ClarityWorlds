using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.StoryGraph
{
    public abstract class StoryServiceRootComponent : SceneNodeComponentBase<StoryServiceRootComponent>
    {
        public abstract IList<Pair<int>> Edges { get; }
    }
}