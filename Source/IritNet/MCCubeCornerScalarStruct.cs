namespace IritNet
{
    public unsafe struct MCCubeCornerScalarStruct
    {
        public IrtPtType Vrtx0Lctn;              /* Lowest corner position. */
        public IrtPtType CubeDim;                  /* Width, Depth, Height. */
        public fixed double Corners[8];           /* Scalar values of corners. */
        public fixed double GradientX[8];         /* Optional gradient at corners. */
        public fixed double GradientY[8];
        public fixed double GradientZ[8];
        public int HasGradient;             /* True if Gradient? are set. */

        /* Used internally. */
        public int _Intersect;
        public IrtPtTypeArray8 _VrtxPos;
        public _MCInterStructArray12 _Inter;
    }
}