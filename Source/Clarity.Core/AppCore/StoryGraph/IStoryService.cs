using System;
using Clarity.Engine.Objects.WorldTree;
using JetBrains.Annotations;

namespace Clarity.Core.AppCore.StoryGraph
{
    public interface IStoryService
    {
        [NotNull]
        IScene EditingScene { get; }
        [NotNull]
        IStoryGraph GlobalGraph { get; }
        // todo: to nested instances
        [NotNull]
        IStoryLayoutInstance RootLayoutInstance { get; }

        void AddEdge(int from, int to);
        void RemoveEdge(int from, int to);

        void OnBeginTransaction(object obj);
        void OnEndTransaction(object obj);

        event Action GraphChanged;
    }
}