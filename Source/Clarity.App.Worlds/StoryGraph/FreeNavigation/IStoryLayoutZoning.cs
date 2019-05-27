using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Worlds.StoryGraph.FreeNavigation
{
    public interface IStoryLayoutZoning
    {
        //ZoneProperties GetZonePropertiesAt(Vector3 position);
        StoryLayoutZoneProperties GetZonePropertiesAt(Vector3 position);
    }
}