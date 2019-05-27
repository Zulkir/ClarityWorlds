using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public interface IAppRuntime
    {
        IDataSourceRuntime DataSource { get; }
        IOriginDataLogService OriginDataLog { get; }
        IDataQueriesService DataQueries { get; }
        IVisualQueriesRuntime VisualQueries { get; }
        IPlaybackService PlaybackService { get; }

        IAppProcedures Procedures { get; }
        IDataRetrievalService DataRetrieval { get; }

        void OnNewFrame(FrameTime frameTime);
    }
}