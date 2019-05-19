namespace IritNet
{
    public unsafe struct SymbArcStruct
    {
        public  SymbArcStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public int Arc;
        public IrtPtType Pt1, Cntr, Pt2;
    }
}
