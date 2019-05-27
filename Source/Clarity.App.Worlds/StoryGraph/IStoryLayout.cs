using System;

namespace Clarity.App.Worlds.StoryGraph
{
    public interface IStoryLayout
    {
        string UserFriendlyName { get; }
        Type Type { get; }
        IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg);
    }
}