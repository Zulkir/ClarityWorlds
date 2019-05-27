namespace IritNet
{
    public unsafe struct TrimSrfStruct
    {
        public  TrimSrfStruct* Pnext;
        public IPAttributeStruct* Attr;
        public int Tags;
        public CagdSrfStruct* Srf;                          /* Surface trimmed by TrimCrvList. */
        public TrimCrvStruct* TrimCrvList;                         /* List of trimming curves. */
    }
}
