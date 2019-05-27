namespace Clarity.Engine.Media.Models.Explicit
{
    public struct ExplicitModelIndexSubrange
    {
        public int FirstIndex;
        public int IndexCount;

        public ExplicitModelIndexSubrange(int firstIndex, int indexCount)
        {
            FirstIndex = firstIndex;
            IndexCount = indexCount;
        }
    }
}