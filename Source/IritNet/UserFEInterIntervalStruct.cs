namespace IritNet
{
    public unsafe struct UserFEInterIntervalStruct
    {
        public  UserFEInterIntervalStruct* Pnext;
        public double T1Min, T1Max;                    /* Interval of overlap in Crv1. */
        public double T2Min, T2Max;                    /* Interval of overlap in Crv2. */
        public double Antipodal1, Antipodal2;  /* Locations of maximal penetration. */
        public IrtVecType ProjNrml;                    /* Direction to project penetration on. */
    }
}
