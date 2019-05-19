namespace Clarity.Engine.Media.Text.Rich
{
    public class RtImage : IRtImage
    {
        public int Width { get; }
        public int Height { get; }
        public byte[] Data { get; }

        public RtImage(int width, int height, byte[] data)
        {
            Width = width;
            Height = height;
            Data = data;
        }
    }
}