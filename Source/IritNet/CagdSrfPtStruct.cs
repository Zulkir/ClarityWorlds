namespace IritNet
{
    public unsafe struct CagdSrfPtStruct
    {
        public  CagdSrfPtStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public IrtUVType Uv;
        public IrtPtType Pt;
        public IrtVecType Nrml;
    }
}
