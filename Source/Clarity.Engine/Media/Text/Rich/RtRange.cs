namespace Clarity.Engine.Media.Text.Rich
{
    public struct RtRange
    {
        public RtPosition FirstCharPos;
        public RtPosition LastCharPos;
        
        public RtRange(RtPosition pos1, RtPosition pos2)
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