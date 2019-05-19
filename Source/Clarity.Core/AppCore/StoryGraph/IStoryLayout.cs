using System;

namespace Clarity.Core.AppCore.StoryGraph
{
    public interface IStoryLayout
    {
        string UserFriendlyName { get; }
        Type Type { get; }
        IStoryLayoutInstance ArrangeAndDecorate(IStoryGraph sg);
    }
}