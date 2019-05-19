namespace IritNet
{
    public unsafe struct SymbMultiResCrvStruct
    {
        public  SymbMultiResCrvStruct *Pnext;
        public CagdCrvStruct **HieCrv;
        public int RefineLevel, Levels, Periodic;
    }
}
