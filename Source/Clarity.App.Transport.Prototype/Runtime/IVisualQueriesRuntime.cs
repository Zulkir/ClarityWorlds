using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Queries.Visual;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IVisualQueriesRuntime
    {
        ISceneNode RootNode { get; }

        IReadOnlyList<IVisualQuery> Queries { get; }
        void AddQuery(IVisualQuery query);
        void RemoveQuery(int index);
        void OnNewFrame(FrameTime frameTime);
        void OnTimestampChanged(double newTimestamp);
    }
}