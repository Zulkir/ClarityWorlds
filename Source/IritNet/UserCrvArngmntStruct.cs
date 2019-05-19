namespace IritNet
{
    public unsafe struct UserCrvArngmntStruct
    {
        public  CagdCrvStruct *CagdCrvs;
        public  UserCAPtStruct *Pts;
        public  UserCACrvStruct *Crvs;
        public  UserCARegionStruct **Regions;
        public  IPObjectStruct *Output;               /* CA output is kept here. */
             
        public double EndPtEndPtTol;  /* Tolerance to consider crvs' end points same. */
        public double InternalTol;    /* Internal tolerance for CCI, inflections etc. */
        public double PlanarityTol;   /* Tolerance of curves to be considered planar. */
        public IrtHmgnMatType XYZ2XYMat, XY2XYZMat;     /* General plane <--> XY plane. */
        public IrtPlnType CrvsPlane;
        public int ProjectOnPlane;/* TRUE to force crvs to be planar, on computed plane.*/
        public int AllocSizePts;      /* Size of the allocated vectors of Pts and Crvs. */
        public int AllocSizeCrvs;
        public int NumOfPts;
        public int NumOfOrigCrvs;                     /* Number of curves in the input. */
        public int NumOfCrvs;                                /* Current number of curves. */
        public int NumOfRegions;                               /* Current number of regions. */
        public  byte *Error; /* Last error string description will be placed here. */
    }
}
