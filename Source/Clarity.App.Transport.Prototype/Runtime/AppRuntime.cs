using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.Runtime
{
    public class AppRuntime : IAppRuntime
    {
        public IDataSourceRuntime DataSource { get; }
        public IOriginDataLogService OriginDataLog { get; }
        public IDataQueriesService DataQueries { get; }
        public IVisualQueriesRuntime VisualQueries { get; }
        public IPlaybackService PlaybackService { get; }
        public IAppProcedures Procedures { get; }
        public IDataRetrievalService DataRetrieval { get; }

        public AppRuntime(IDataSourceRuntime dataSource, IOriginDataLogService originDataLog, IDataQueriesService dataQueries, 
                          IVisualQueriesRuntime visualQueries, IPlaybackService playbackService, IAppProcedures procedures, IDataRetrievalService dataRetrieval)
        {
            DataSource = dataSource;
            OriginDataLog = originDataLog;
            DataQueries = dataQueries;
            VisualQueries = visualQueries;
            PlaybackService = playbackService;
            Procedures = procedures;
            DataRetrieval = dataRetrieval;
        }

        public void OnNewFrame(FrameTime frameTime)
        {
            PlaybackService.OnNewFrame(frameTime, out var updated);
            if (updated)
            {
                DataQueries.OnTimestampChanged(PlaybackService.AbsoluteTime);
                VisualQueries.OnTimestampChanged(PlaybackService.AbsoluteTime);
            }
            VisualQueries.OnNewFrame(frameTime);
        }
    }
}