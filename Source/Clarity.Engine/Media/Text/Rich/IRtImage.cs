namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtImage
    {
        int Width { get; }
        int Height { get; }
        byte[] Data { get; }
    }
}