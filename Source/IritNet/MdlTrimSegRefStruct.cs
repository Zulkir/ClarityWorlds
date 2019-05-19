namespace IritNet
{
    public unsafe struct MdlTrimSegRefStruct
    {
        public  MdlTrimSegRefStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public MdlTrimSegStruct *TrimSeg;
        public byte Reversed;
        public byte Tags;
    }
}
