using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Engine.Media.Text.Rich
{
    [TrwSerialize]
    public struct RtPosition : IEquatable<RtPosition>, IComparable<RtPosition>
    {
        public int ParaIndex;
        public int SpanIndex;
        public int CharIndex;

        public RtPosition(int paraIndex, int spanIndex, int charIndex)
        {
            ParaIndex = paraIndex;
            SpanIndex = spanIndex;
            CharIndex = charIndex;
        }
        
        #region Equatable
        public bool Equals(RtPosition other) => 
            ParaIndex == other.ParaIndex && 
            SpanIndex == other.SpanIndex && 
            CharIndex == other.CharIndex;

        public override bool Equals(object obj) => obj is RtPosition && Equals((RtPosition)obj);
        public override int GetHashCode() => ParaIndex ^ (SpanIndex << 8) ^ (CharIndex << 16);
        public static bool operator ==(RtPosition pos1, RtPosition pos2) => pos1.Equals(pos2);
        public static bool operator !=(RtPosition pos1, RtPosition pos2) => !(pos1 == pos2);
        #endregion

        #region Comparable
        public int CompareTo(RtPosition other)
        {
            var paraIndexComparison = ParaIndex.CompareTo(other.ParaIndex);
            if (paraIndexComparison != 0) 
                return paraIndexComparison;
            var spanIndexComparison = SpanIndex.CompareTo(other.SpanIndex);
            if (spanIndexComparison != 0) 
                return spanIndexComparison;
            return CharIndex.CompareTo(other.CharIndex);
        }

        public static bool operator >(RtPosition pos1, RtPosition pos2) => pos1.CompareTo(pos2) > 0;
        public static bool operator <(RtPosition pos1, RtPosition pos2) => pos1.CompareTo(pos2) < 0;
        public static bool operator >=(RtPosition pos1, RtPosition pos2) => pos1.CompareTo(pos2) >= 0;
        public static bool operator <=(RtPosition pos1, RtPosition pos2) => pos1.CompareTo(pos2) <= 0;
        #endregion

        public RtPosition WithPara(int val) => new RtPosition(val, SpanIndex, CharIndex);
        public RtPosition WithSpan(int val) => new RtPosition(ParaIndex, val, CharIndex);
        public RtPosition WithChar(int val) => new RtPosition(ParaIndex, SpanIndex, val);
        public RtPosition WithCharPlus(int val) => new RtPosition(ParaIndex, SpanIndex, CharIndex + val);
    }
}