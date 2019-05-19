namespace IritNet
{
    public unsafe struct IRndrEdgeStruct
    {
        public int x;/* Current X coordinate on the scan line, the lowest end at start. */
        public int dx, dy, Inc; /* Scan line converting integer algorithm data members. */
        public int YMin;                             /* The lowest endpoint coordinate. */
        public IRndrInterpolStruct Value;    /* Starting and later crnt interpo. value. */
        public IRndrInterpolStruct dValue;         /* Increment of interpolation value. */
    }
}