namespace IritNet
{
    public unsafe struct MdlTrimSegStruct
    {
        public  MdlTrimSegStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public  MdlTrimSrfStruct *SrfFirst;
        public  MdlTrimSrfStruct *SrfSecond;
        public CagdCrvStruct *UVCrvFirst;   /* Trim crv segment in srf's param. domain. */
        public CagdCrvStruct *UVCrvSecond;  /* Trim crv segment in srf's param. domain. */
        public CagdCrvStruct *EucCrv;       /* Trimming curve as an E3 Euclidean curve. */
        public byte Tags;
    }
}
