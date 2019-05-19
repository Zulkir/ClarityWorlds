namespace IritNet
{
    public unsafe struct TrimCrvSegStruct
    {
        public  TrimCrvSegStruct *Pnext;
        public IPAttributeStruct *Attr;
        public CagdCrvStruct *UVCrv;    /* Trimming crv segment in srf's param. domain. */
        public CagdCrvStruct *EucCrv;       /* Trimming curve as an E3 Euclidean curve. */
    }
}
