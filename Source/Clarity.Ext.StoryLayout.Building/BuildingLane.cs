using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.Ext.StoryLayout.Building
{
    public struct BuildingLane
    {
        public Pair<int> Edge;
        public int Disambiguator;
        public int CurveIndexBeforeFwdFork;
        public int CurveIndexAfterBwdFork;
        public IReadOnlyList<BezierQuadratic3> GlobalPath;
    }
}