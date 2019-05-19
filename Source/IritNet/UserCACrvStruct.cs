namespace IritNet
{
    public unsafe struct UserCACrvStruct
    {
        public int Idx;                        /* In the curves' array, -1 if inactive. */
        public int NumOfCmplxLoops;          /*# of loops this curve is in: 0, 1, or 2. */
        public UserCAObjType Type;
        public CagdCrvStruct* Crv, XYCrv;	       /* Original and rotated to XY curves. */
        public UserCAPtStruct* RefStartPt, RefEndPt;
    }
}