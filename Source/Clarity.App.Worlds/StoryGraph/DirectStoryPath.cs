using Clarity.Common.Numericals;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.App.Worlds.StoryGraph 
{
    public class DirectStoryPath : IStoryPath
    {
        public bool HasFinished { get; private set; }

        private CameraProps StartCameraProps { get; }
        private CameraProps EndCameraProps { get; }
        private float DefaultDuration { get; }
        private float currentTimestamp;

        public float MaxRemainingTime => DefaultDuration - currentTimestamp;

        public DirectStoryPath(CameraProps startCameraProps, CameraProps endCameraProps, float defaultDuration)
        {
            StartCameraProps = startCameraProps;
            EndCameraProps = endCameraProps;
            DefaultDuration = defaultDuration;
        }

        public CameraProps GetCurrentCameraProps() => 
            GetCameraAt(currentTimestamp);

        private CameraProps GetCameraAt(float timestamp)
        {
            var amount = GetLerpAmount(timestamp);
            return CameraProps.Lerp(StartCameraProps, EndCameraProps, amount);
        }

        private float GetLerpAmount(float timestamp) => 
            MathHelper.Hermite(timestamp / DefaultDuration);

        public void Update(FrameTime frameTime)
        {
            currentTimestamp += frameTime.DeltaSeconds;
            if (currentTimestamp > DefaultDuration)
            {
                currentTimestamp = DefaultDuration;
                HasFinished = true;
    }
        }
    }
}