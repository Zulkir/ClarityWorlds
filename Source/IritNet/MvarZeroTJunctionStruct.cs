namespace IritNet
{
    public unsafe struct MvarZeroTJunctionStruct
    {
        public MvarZeroTJunctionStruct *Pnext;
        public int IsHandled;             /* Already found a polyline and added. */
        public MvarPtStruct* TJuncPrev;        /* The point before the T-Junction. */
        public MvarPtStruct* TJunc;           /* The split point, refined to solution. */
        public MvarPtStruct* TJuncNext;         /* The point after the T-Junction. */
        public MvarPtStruct* OrigSplitPt;          /* The unrefined curve split point. */
    }
}