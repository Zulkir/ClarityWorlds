namespace Clarity.Engine.Media.Movies
{
    public struct MovieAudioFrame
    {
        public byte[] SamplesData;
        public double Timestamp;
        public int SampleRate;
        public int BitsPerSample;
        public int NumChannels;
    }
}