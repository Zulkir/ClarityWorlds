namespace IritNet
{
    public unsafe struct TrimCrvStruct
    {
        public  TrimCrvStruct *Pnext;
        public IPAttributeStruct *Attr;
        public TrimCrvSegStruct *TrimCrvSegList;    /* List of trimming curve segments. */
    }
}
