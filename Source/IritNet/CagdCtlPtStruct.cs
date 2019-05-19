namespace IritNet
{
    public unsafe struct CagdCtlPtStruct
    {
        public CagdCtlPtStruct* Pnext;
        public IPAttributeStruct* Attr;
        public fixed double Coords[Irit.CAGD_MAX_PT_SIZE];
        public CagdPointType PtType;
        public int align8;
    }
}