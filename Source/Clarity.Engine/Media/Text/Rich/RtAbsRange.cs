namespace Clarity.Engine.Media.Text.Rich
{
    public struct RtAbsRange
    {
        public int FirstCharAbsPos;
        public int LastCharAbsPos;

        public RtAbsRange(int pos1, int pos2)
        {
            if (pos1 <= pos2)
            {
                FirstCharAbsPos = pos1;
                LastCharAbsPos = pos2;
            }
            else
            {
                FirstCharAbsPos = pos2;
                LastCharAbsPos = pos1;
            }
        }
    }
}