namespace IritNet
{
    public unsafe struct MvarMVGradientStruct
    {
        public int Dim;
        public int IsRational, HasOrig;
        public MvarMVStruct* MV;                               /* The original multivariate. */
        public MvarMVStruct* MVGrad;                    /* The gradient if not rational. */
        public MvarMVStructPtrArray20 MVRGrad;  /* The grad. if rational. */
    }
}
