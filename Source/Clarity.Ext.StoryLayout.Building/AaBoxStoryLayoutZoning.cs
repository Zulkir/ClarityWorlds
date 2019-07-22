using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.StoryLayout.Building
{
    public class AaBoxStoryLayoutZoning : IStoryLayoutZoning
    {
        private readonly IReadOnlyList<Pair<AaBox, StoryLayoutZoneProperties>> zonesWithProperties;
        private readonly StoryLayoutZoneProperties defaultZoneProperties;

        public AaBoxStoryLayoutZoning(IReadOnlyList<Pair<AaBox, StoryLayoutZoneProperties>> zonesWithProperties, StoryLayoutZoneProperties defaultZoneProperties)
        {
            this.zonesWithProperties = zonesWithProperties;
            this.defaultZoneProperties = defaultZoneProperties;
        }

        public StoryLayoutZoneProperties GetZonePropertiesAt(Vector3 position)
        {
            return zonesWithProperties.Where(x => x.First.Contains(position)).FirstOrNull()?.Second ?? defaultZoneProperties;
        }
    }
}