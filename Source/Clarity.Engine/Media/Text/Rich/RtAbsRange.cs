namespace Clarity.Engine.Media.Text.Rich
{
    public struct RtAbsRange
    {
        public int FirstCharPos;
        public int LastCharPos;

        public RtAbsRange(int pos1, int pos2)
        {
            if (pos1 <= pos2)
            {
                FirstCharPos = pos1;
                LastCharPos = pos2;
            }
            else
            {
                FirstCharPos = pos2;
                LastCharPos = pos1;
            }
        }
    }
}