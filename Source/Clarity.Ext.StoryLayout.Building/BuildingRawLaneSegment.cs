namespace Clarity.Ext.StoryLayout.Building
{
    public struct BuildingRawLaneSegment
    {
        public BuildingStoryLayoutLanePartType Type;
        public int ParentNode;
        public bool StartsAtView;
        public bool EndsAtView;

        public int StartPoint;
        public int NumPoints;
    }
}