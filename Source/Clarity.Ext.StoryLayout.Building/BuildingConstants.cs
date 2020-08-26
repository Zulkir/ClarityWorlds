using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.StoryLayout.Building
{
    public static class BuildingConstants
    {
        public const float WidthMargin = 16f;
        public const float CorridorBranchingOffset = 4f;
        public const float HeightMargin = 10f;
        public const float DepthMargin = 3f;
        public const float StairsDistance = 5f;
        public const float CorridorHalfWidth = 2f;
        public const float CeilingHeight = 5f;
        public const float CorridorDisambiguationOffset = 0.15f;
        public const float ElevatorOffset = CorridorHalfWidth;
        public const float BezierSegmentMaxLength = 4.0f;
        public const float BezierSegmentMaxLengthSq = BezierSegmentMaxLength * BezierSegmentMaxLength;
        public const float TerminalLaneDepth = 2f;
        public const float EyeHeight = 2f;

        public static readonly Size3 LeafHalfSize = new Size3(3, EyeHeight, 3);
        public static readonly Vector3 StartLeafCornerLaneOffset = new Vector3(-0.5f, 0, -0.5f);
        public static readonly Vector3 EndLeafCornerLaneOffset = new Vector3(0.5f, 0, -0.5f);
    }
}