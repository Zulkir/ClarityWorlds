namespace IritNet
{
    public unsafe struct MdlLoopStruct
    {
        public  MdlLoopStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public MdlTrimSegRefStruct *SegRefList;
    }
}
