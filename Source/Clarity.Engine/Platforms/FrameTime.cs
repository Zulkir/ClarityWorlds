namespace Clarity.Engine.Platforms
{
    public struct FrameTime
    {
        public float TotalSeconds;
        public float DeltaSeconds;

        public FrameTime(float totalSeconds, float deltaSeconds)
        {
            TotalSeconds = totalSeconds;
            DeltaSeconds = deltaSeconds;
        }
    }
}