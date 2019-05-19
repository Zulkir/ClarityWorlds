namespace IritNet
{
    public unsafe struct UserCAPtStruct
    {
        public int Idx;                        /* In the points' array, -1 if inactive. */
        public IrtPtType Pt, XYPt;
        /* Refs to curves whose end points are at this pt: */
        public  UserCACrvStructPtrArray10 RefCrvs;
        public int NumOfRefCrvs;
    }
}