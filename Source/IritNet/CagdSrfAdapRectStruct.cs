namespace IritNet
{
    public unsafe struct CagdSrfAdapRectStruct
    {
        public  CagdSrfAdapRectStruct *Pnext;
        public int UIndexBase;
        public int UIndexSize;
        public int VIndexBase;
        public int VIndexSize;
        public void * AuxSrfData;
        public double Err;
    }
}
